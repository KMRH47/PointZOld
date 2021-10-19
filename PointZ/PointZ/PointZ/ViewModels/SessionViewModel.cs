using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using PointZ.Models.Command;
using PointZ.Models.DisplayDimensions;
using PointZ.Models.PlatformEvent;
using PointZ.Models.Server;
using PointZ.Models.SoftInput;
using PointZ.Services.InputCommandSender;
using PointZ.Services.InputEventHandler;
using PointZ.Services.PlatformEvent;
using PointZ.Services.PlatformSettings;
using PointZ.Services.Settings;
using PointZ.ViewModels.Base;
using Xamarin.Forms;

namespace PointZ.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private readonly IInputEventHandler<TouchEventArgs> touchEventHandler;
        private readonly IInputEventHandler<KeyEventArgs> keyEventHandler;
        private readonly IKeyboardCommandSender keyboardCommandSender;
        private readonly IPlatformEventService platformEventService;
        private readonly IPlatformSettingsService platformSettingsService;
        private readonly ISettingsService settingsService;

        private const string EntryPlaceholderTextConst = "\uf11c";
        private bool entryFocused;
        private string entryText = "";
        private string entryTextPrevious = "";
        private string entryPlaceholderText = EntryPlaceholderTextConst;
        private double entryHeight;

        public SessionViewModel(
            IInputEventHandler<TouchEventArgs> touchEventHandler,
            IInputEventHandler<KeyEventArgs> keyEventHandler,
            IKeyboardCommandSender keyboardCommandSender,
            ISettingsService settingsService,
            IPlatformSettingsService platformSettingsService,
            IPlatformEventService platformEventService)
        {
            this.touchEventHandler = touchEventHandler;
            this.keyEventHandler = keyEventHandler;
            this.keyboardCommandSender = keyboardCommandSender;
            this.settingsService = settingsService;
            this.platformSettingsService = platformSettingsService;
            this.platformEventService = platformEventService;
            ReturnPressedCommand = new Command(OnReturnPressed);
            EntryFocusedCommand = new Command(OnEntryFocused);
            EntryUnfocusedCommand = new Command(OnEntryUnfocused);
        }

        public override Task InitializeAsync(object parameter)
        {
            ServerData serverData = (ServerData)parameter;
            this.settingsService.ServerIpEndPoint = serverData.IpEndPoint;
            this.platformSettingsService.WindowSoftInputMode(SoftInput.AdjustResize);
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
            get => this.entryHeight * this.platformSettingsService.DisplayDensity;
            set
            {
                this.entryHeight = value;
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
            switch (EntryFocused)
            {
                case true:
                    switch (e.TouchAction)
                    {
                        case TouchAction.Down: return;
                    }

                    goto case false;
                case false:
                    DisplayDimensionData displayDimensions = this.platformSettingsService.GetDisplayDimensions();
                    double screenHeight = displayDimensions.Height;
                    bool withinBounds = e.Y < screenHeight - EntryHeight;

                    if (!withinBounds) return;

                    await this.touchEventHandler.HandleAsync(e);
                    break;
            }
        }

        private async void OnKeyDown(object o, KeyEventArgs e)
        {
            if (!EntryFocused) return;

            await this.keyEventHandler.HandleAsync(e);
        }

        private async void OnReturnPressed(object o)
        {
            Debug.WriteLine($"OnReturnPressed o: {o}");

            string textEntry = o.ToString();

            if (textEntry != string.Empty)
            {
                await this.keyboardCommandSender.SendTextEntryAsync(textEntry);
                EntryText = string.Empty;
            }
            else
            {
                await this.keyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyPress,
                    KeyCodeAction.Enter);
            }

            // Clunky way making the keyboard re-appear, but it works. This will be seamless at some point in the future.
            this.platformSettingsService.ToggleKeyboard();
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