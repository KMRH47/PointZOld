using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
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
        private readonly ISessionPlatformInterfaceService sessionPlatformInterfaceService;
        private readonly ISessionPlatformEventService sessionPlatformEventService;

       // private const string EntryPlaceholderTextConst = "&#xf11c;";
        private const string EntryPlaceholderTextConst = "\uf11c";
        private bool entryEnabled = true;
        private string entryText = "";
        private string entryTextPrevious = "";

        private double buttonHeight;
        private string entryPlaceholderText = EntryPlaceholderTextConst;

        public SessionViewModel(
            ISessionEventHandlerService<TouchEventArgs> sessionTouchEventHandlerService,
            ISessionEventHandlerService<KeyEventArgs> sessionKeyEventHandlerService,
            ISessionMessageHandlerService sessionMessageHandlerService,
            ISessionPlatformInterfaceService sessionPlatformInterfaceService,
            ISessionPlatformEventService sessionPlatformEventService)
        {
            this.sessionTouchEventHandlerService = sessionTouchEventHandlerService;
            this.sessionKeyEventHandlerService = sessionKeyEventHandlerService;
            this.sessionMessageHandlerService = sessionMessageHandlerService;
            this.sessionPlatformInterfaceService = sessionPlatformInterfaceService;
            this.sessionPlatformEventService = sessionPlatformEventService;
            ToggleKeyboardCommand = new Command(OnToggleKeyboard);
            ToggleSendEveryKeyPressCommand = new Command(OnToggleSendEveryKeyPress);
            ReturnPressedCommand = new Command(OnReturnPressed);
            EntryFocusedCommand = new Command(OnEntryFocused);
            EntryUnfocusedCommand = new Command(OnEntryUnfocused);
        }


        public ICommand ToggleKeyboardCommand { get; set; }
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

        public double ButtonHeightPixels
        {
            get
            {
                double l = this.buttonHeight * Math.PI;
                Debug.WriteLine($"buttonHeight: {this.buttonHeight}");
                Debug.WriteLine($"buttonHeight * PI: {l}");
                return this.buttonHeight * Math.PI;
            }
            set => this.buttonHeight = value;
        }

        public override Task InitializeAsync(object parameter)
        {
            ServerData serverData = (ServerData)parameter;
            IPAddress ipAddress = IPAddress.Parse(serverData.Address);
            this.sessionTouchEventHandlerService.Bind(ipAddress);
            OnViewAppearing(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        private void OnViewAppearing(object sender, EventArgs e)
        {
            Debug.WriteLine($"OnViewAppearing");
            this.sessionPlatformEventService.OnScreenTouched += OnScreenTouched;
            this.sessionPlatformEventService.OnKeyDown += OnKeyDown;
            this.sessionPlatformEventService.OnBackButtonPressed += OnBackButtonPressed;
            this.sessionPlatformEventService.OnViewDisappearing += OnViewDisappearing;
        }

        private void OnViewDisappearing(object sender, EventArgs e)
        {
            Debug.WriteLine($"OnViewDisappearing");
            this.sessionPlatformEventService.OnScreenTouched -= OnScreenTouched;
            this.sessionPlatformEventService.OnKeyDown -= OnKeyDown;
            this.sessionPlatformEventService.OnBackButtonPressed -= OnBackButtonPressed;
            this.sessionPlatformEventService.OnViewDisappearing -= OnViewDisappearing;
        }

        private async void OnScreenTouched(object sender, TouchEventArgs e)
        {
            DisplayDimensionData displayDimensions = this.sessionPlatformInterfaceService.GetDisplayDimensions();
            int screenHeight = displayDimensions.Height;

            bool withinBounds = e.Y < screenHeight - ButtonHeightPixels;
            if (!withinBounds) return;

            await this.sessionTouchEventHandlerService.HandleAsync(e);
        }

        private async void OnKeyDown(object sender, KeyEventArgs e)
        {
            await this.sessionKeyEventHandlerService.HandleAsync(e);
        }

        private void OnToggleSendEveryKeyPress() { EntryEnabled = !EntryEnabled; }

        private void OnTextChanged()
        {
            Debug.WriteLine($"OnTextChanged VIEWMODEL");

            if (EntryEnabled) return;

            bool backSpacePressed = this.entryText.Length < this.entryTextPrevious.Length;
            bool anyText = this.entryText.Length > 0;

            if (!anyText) return;

            if (backSpacePressed)
            {
                KeyEventArgs e = new(KeyAction.Down, KeyCodeAction.Del.ToString());
                OnKeyDown(this, e);
            }
            else
            {
                char lastChar = this.entryText[this.entryTextPrevious.Length];
                Debug.WriteLine($"Character: {lastChar}");
            }
        }

        private async void OnReturnPressed(object o)
        {
            string message = o.ToString();
            await this.sessionMessageHandlerService.SendMessageAsync(message);
            EntryText = string.Empty;
        }

        private void OnToggleKeyboard()
        {
            Debug.WriteLine($"FocusChangeRequested...!");
            //this.sessionPlatformInterfaceService.ToggleKeyboard();
        }

        private void OnBackButtonPressed(object sender, EventArgs e)
        {
            base.NavigationService.RemoveLastFromBackStackAsync();
            OnViewDisappearing(sender, e);
        }

        private void OnEntryUnfocused() => EntryPlaceholderText = EntryPlaceholderTextConst;
        private void OnEntryFocused() => EntryPlaceholderText = string.Empty;
    }
}