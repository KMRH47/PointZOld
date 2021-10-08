using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using PointZ.Models.DisplayDimensions;
using PointZ.Models.PlatformEvent;
using PointZ.Models.Server;
using PointZ.Services.PlatformEvent;
using PointZ.Services.PlatformInterface;
using PointZ.Services.SessionEventHandler;
using PointZ.Services.SessionMessageHandler;
using PointZ.ViewModels.Base;
using Xamarin.Forms;

namespace PointZ.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly ISessionEventHandlerService<TouchEventArgs> sessionTouchEventHandlerService;
        private readonly ISessionEventHandlerService<KeyEventArgs> sessionKeyEventHandlerService;
        private readonly ISessionMessageHandlerService sessionMessageHandlerService;
        private readonly IPlatformInterfaceService platformInterfaceService;
        private readonly IPlatformEventService platformEventService;

        private const string EntryPlaceholderTextConst = "\uf11c";
        private bool entryFocused = true;
        private string entryText = "";
        private string entryTextPrevious = "";

        private string entryPlaceholderText = EntryPlaceholderTextConst;
        private double entryHeight;

        public SessionViewModel(
            ISessionEventHandlerService<TouchEventArgs> sessionTouchEventHandlerService,
            ISessionEventHandlerService<KeyEventArgs> sessionKeyEventHandlerService,
            ISessionMessageHandlerService sessionMessageHandlerService,
            IPlatformInterfaceService platformInterfaceService,
            IPlatformEventService platformEventService)
        {
            this.sessionTouchEventHandlerService = sessionTouchEventHandlerService;
            this.sessionKeyEventHandlerService = sessionKeyEventHandlerService;
            this.sessionMessageHandlerService = sessionMessageHandlerService;
            this.platformInterfaceService = platformInterfaceService;
            this.platformEventService = platformEventService;
            ReturnPressedCommand = new Command(OnReturnPressed);
            EntryFocusedCommand = new Command(OnEntryFocused);
            EntryUnfocusedCommand = new Command(OnEntryUnfocused);
        }

        public override Task InitializeAsync(object parameter)
        {
            ServerData serverData = (ServerData)parameter;
            this.sessionTouchEventHandlerService.Bind(serverData.IpEndPoint);
            this.sessionKeyEventHandlerService.Bind(serverData.IpEndPoint);
            OnViewAppearing(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public ICommand ReturnPressedCommand { get; set; }
        public ICommand EntryFocusedCommand { get; set; }
        public ICommand EntryUnfocusedCommand { get; set; }


        public bool EntryFocused
        {
            get => this.entryFocused;
            set
            {
                this.entryFocused = value;
                OnPropertyChanged();
            }
        }

        public string EntryText
        {
            get => this.entryText;
            set
            {
                this.entryTextPrevious = this.entryText;
                this.entryText = value;
                OnPropertyChanged();
            }
        }

        public string EntryPlaceholderText
        {
            get => this.entryPlaceholderText;
            set
            {
                this.entryPlaceholderText = value;
                OnPropertyChanged();
            }
        }

        public double EntryHeight
        {
            get => this.entryHeight * this.platformInterfaceService.DisplayDensity;
            set
            {
                this.entryHeight = value;
                Debug.WriteLine($"EntryHeight is: {value}");
                OnPropertyChanged();
            }
        }

        private void OnViewAppearing(object sender, EventArgs e)
        {
            AddPlatformListeners();
            this.platformEventService.OnViewDisappearing += OnViewDisappearing;
            this.platformEventService.OnViewAppearing -= OnViewAppearing;
        }

        private void OnViewDisappearing(object sender, EventArgs e)
        {
            RemovePlatformListeners();
            this.platformEventService.OnViewDisappearing -= OnViewDisappearing;
            this.platformEventService.OnViewAppearing += OnViewAppearing;
        }

        private void AddPlatformListeners()
        {
            this.platformEventService.OnScreenTouched += OnScreenTouched;
            this.platformEventService.OnBackButtonPressed += OnBackButtonPressed;
            this.platformEventService.OnKeyDown += OnKeyDown;
        }

        private void RemovePlatformListeners()
        {
            this.platformEventService.OnScreenTouched -= OnScreenTouched;
            this.platformEventService.OnBackButtonPressed -= OnBackButtonPressed;
            this.platformEventService.OnKeyDown -= OnKeyDown;

        }

        private async void OnScreenTouched(object sender, TouchEventArgs e)
        {
            DisplayDimensionData displayDimensions = this.platformInterfaceService.GetDisplayDimensions();
            double screenHeight = displayDimensions.Height;

            bool withinBounds = e.Y < screenHeight - EntryHeight;
            if (!withinBounds) return;

            await this.sessionTouchEventHandlerService.HandleAsync(e);
        }

        private async void OnKeyDown(object o, KeyEventArgs e)
        {
            Debug.WriteLine($"OnTextChanged VIEWMODEL");

            if (!EntryFocused) return;
            
            await this.sessionKeyEventHandlerService.HandleAsync(e);
        }

        private async void OnReturnPressed(object o)
        {
            Debug.WriteLine($"OnReturnPressed");
            string message = o.ToString();
            await this.sessionMessageHandlerService.SendMessageAsync(message);
            EntryText = string.Empty;
        }

        /// <summary>
        /// Not called when a view has focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBackButtonPressed(object sender, EventArgs e)
        {
            RemovePlatformListeners();
            this.platformEventService.OnViewAppearing -= OnViewAppearing;
            this.platformEventService.OnViewDisappearing -= OnViewDisappearing;
            base.NavigationService.NavigateBackAsync();
        }

        private void OnEntryUnfocused()
        {
            EntryFocused = false;
            EntryPlaceholderText = EntryPlaceholderTextConst;
        }

        private void OnEntryFocused()
        {
            EntryFocused = true;
            EntryPlaceholderText = string.Empty;
        }
    }
}