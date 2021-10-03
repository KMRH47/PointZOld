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
        private bool entryEnabled = true;
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
            ToggleSendEveryKeyPressCommand = new Command(OnToggleSendEveryKeyPress);
            ReturnPressedCommand = new Command(OnReturnPressed);
            EntryFocusedCommand = new Command(OnEntryFocused);
            EntryUnfocusedCommand = new Command(OnEntryUnfocused);
        }


        public ICommand ToggleSendEveryKeyPressCommand { get; set; }
        public ICommand ReturnPressedCommand { get; set; }
        public ICommand EntryFocusedCommand { get; set; }
        public ICommand EntryUnfocusedCommand { get; set; }


        public bool EntryEnabled
        {
            get => this.entryEnabled;
            set
            {
                this.entryEnabled = value;
                RaisePropertyChanged(() => EntryEnabled);
            }
        }

        public string EntryText
        {
            get => this.entryText;
            set
            {
                this.entryTextPrevious = this.entryText;
                this.entryText = value;
                OnTextChanged();
                RaisePropertyChanged(() => EntryText);
            }
        }

        public string EntryPlaceholderText
        {
            get => this.entryPlaceholderText;
            set
            {
                this.entryPlaceholderText = value;
                RaisePropertyChanged(() => EntryPlaceholderText);
            }
        }

        public double EntryHeight
        {
            get => this.entryHeight * this.platformInterfaceService.DisplayDensity;
            set
            {
                this.entryHeight = value;
                Debug.WriteLine($"EntryHeight is: {value}");
                RaisePropertyChanged(() => EntryHeight);
            }
        }


        public override Task InitializeAsync(object parameter)
        {
            ServerData serverData = (ServerData)parameter;
            IPAddress ipAddress = IPAddress.Parse(serverData.Address);
            this.sessionTouchEventHandlerService.Bind(ipAddress);
            OnViewAppearing(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        private void OnViewAppearing(object sender, EventArgs e) => AddPlatformListeners();
        private void OnViewDisappearing(object sender, EventArgs e)
        {
            RemovePlatformListeners();
            this.platformEventService.OnViewAppearing += OnViewAppearing;
        }

        private void AddPlatformListeners()
        {
            this.platformEventService.OnScreenTouched += OnScreenTouched;
            this.platformEventService.OnBackButtonPressed += OnBackButtonPressed;
            this.platformEventService.OnViewDisappearing += OnViewDisappearing;
        }

        private void RemovePlatformListeners()
        {
            this.platformEventService.OnScreenTouched -= OnScreenTouched;
            this.platformEventService.OnBackButtonPressed -= OnBackButtonPressed;
            this.platformEventService.OnViewDisappearing -= OnViewDisappearing;
        }
        
        private void RemoveAllPlatformListeners()
        {
            RemovePlatformListeners();
            this.platformEventService.OnViewAppearing -= OnViewAppearing;
        }

        private async void OnScreenTouched(object sender, TouchEventArgs e)
        {
            DisplayDimensionData displayDimensions = this.platformInterfaceService.GetDisplayDimensions();
            double screenHeight = displayDimensions.Height;

            bool withinBounds = e.Y < screenHeight - EntryHeight;
            if (!withinBounds) return;

            await this.sessionTouchEventHandlerService.HandleAsync(e);
        }

        private void OnToggleSendEveryKeyPress() { EntryEnabled = !EntryEnabled; }

        private async void OnTextChanged()
        {
            Debug.WriteLine($"OnTextChanged VIEWMODEL");

            if (EntryEnabled) return;

            bool backSpacePressed = this.entryText.Length < this.entryTextPrevious.Length;
            bool anyText = this.entryText.Length > 0;

            if (!anyText) return;

            if (backSpacePressed)
            {
                KeyEventArgs e = new(KeyAction.Down, KeyCodeAction.Del.ToString());
                await this.sessionKeyEventHandlerService.HandleAsync(e);
            }
            else
            {
                char lastChar = this.entryText[this.entryTextPrevious.Length];
                Debug.WriteLine($"Character: {lastChar}");
            }
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
            RemoveAllPlatformListeners();
            base.NavigationService.NavigateBackAsync();
        }

        private void OnEntryUnfocused() => EntryPlaceholderText = EntryPlaceholderTextConst;
        private void OnEntryFocused() => EntryPlaceholderText = string.Empty;
    }
}