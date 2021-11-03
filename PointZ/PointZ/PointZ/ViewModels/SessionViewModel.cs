using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using PointZ.Models.CustomEditor;
using PointZ.Models.Input;
using PointZ.Models.KeyEvent;
using PointZ.Models.TouchEvent;
using PointZ.Services.InputEventHandler;
using PointZ.Services.PlatformEventService;
using PointZ.Services.PlatformSettings;
using PointZ.Services.Settings;
using PointZ.ViewModels.Base;
using Xamarin.Forms;

namespace PointZ.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private const string PopUpHintSwitchInputMode = "Mode: ";

        private readonly IInputEventHandlerService inputEventHandlerService;
        private readonly IPlatformEventService platformEventService;
        private readonly ISettingsService settingsService;
        private readonly IPlatformSettingsService platformSettingsService;

        private string customEditorText = "";
        private string customEditorMessageModeText = "";
        private double touchpadHeight;
        private bool customEditorIsFocused;
        private bool directInputDisabled;
        private bool keyboardKeysVisible;

        public SessionViewModel(
            IInputEventHandlerService inputEventHandlerService, IPlatformSettingsService platformSettingsService,
            IPlatformEventService platformEventService, ISettingsService settingsService)
        {
            this.inputEventHandlerService = inputEventHandlerService;
            this.platformSettingsService = platformSettingsService;
            this.platformEventService = platformEventService;
            this.settingsService = settingsService;
            SoftKeyboardButtonCommand = new Command(OnSoftKeyboardButtonPressed);
            SwitchInputModeCommand = new Command(OnSwitchInputModeButtonPressed);
            SendTextCommand = new Command(OnSendButtonPressed);
            KeyboardButtonCommand = new Command(OnKeyboardButtonPressed);

            // Media keys (excessive)
            MediaRewind = new Command(OnMediaRewind);
            MediaPlayPause = new Command(OnMediaPlayPause);
            MediaForward = new Command(OnMediaForward);
            MediaPrevious = new Command(OnMediaPrevious);
            MediaStop = new Command(OnMediaStop);
            MediaNext = new Command(OnMediaNext);
            VolumeDown = new Command(OnVolumeDown);
            Mute = new Command(OnMute);
            VolumeUp = new Command(OnVolumeUp);
        }

        public override Task InitializeAsync(object parameter)
        {
            IPEndPoint serverIpEndPoint = (IPEndPoint)parameter;
            this.settingsService.ServerIpEndPoint = serverIpEndPoint;
            this.platformSettingsService.SetSoftInputModeAdjustResize();
            OnViewAppearing(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        #region Command Members

        public ICommand KeyboardButtonCommand { get; set; }
        public ICommand SoftKeyboardButtonCommand { get; set; }
        public ICommand SwitchInputModeCommand { get; set; }
        public ICommand SendTextCommand { get; set; }

        // Media Commands (excessive)
        public ICommand MediaRewind { get; set; }
        public ICommand MediaPlayPause { get; set; }
        public ICommand MediaForward { get; set; }
        public ICommand MediaPrevious { get; set; }
        public ICommand MediaStop { get; set; }
        public ICommand MediaNext { get; set; }
        public ICommand VolumeDown { get; set; }
        public ICommand Mute { get; set; }
        public ICommand VolumeUp { get; set; }

        #endregion

        #region Properties

        public bool KeyboardKeysVisible
        {
            get => this.keyboardKeysVisible;
            set
            {
                this.keyboardKeysVisible = value;
                OnPropertyChanged();
            }
        }

        public double TouchpadHeight
        {
            get => this.touchpadHeight;
            set => this.touchpadHeight = value * this.platformSettingsService.DisplayDensity;
        }

        public bool CustomEditorIsFocused
        {
            get => this.customEditorIsFocused;
            set
            {
                // When CustomEditor.Focus() is invoked, this property will be invoked as well.
                // The line underneath is for skipping unnecessary code execution.
                if (this.customEditorIsFocused && value) return;
                this.customEditorIsFocused = value;
                OnPropertyChanged();
            }
        }

        public bool DirectInputDisabled
        {
            get => this.directInputDisabled;
            set
            {
                if (value)
                {
                    CustomEditorText = this.customEditorMessageModeText; 
                    this.customEditorMessageModeText = string.Empty;
                    this.platformSettingsService.DisplayPopupHint(PopUpHintSwitchInputMode + "Message", 0);
                    this.platformEventService.OnCustomEditorSetInputType(
                        new CustomEditorEventArgs(TextInputTypes.ClassText | TextInputTypes.TextFlagMultiLine));
                }
                else
                {
                    this.customEditorMessageModeText = CustomEditorText;
                    CustomEditorText = string.Empty;
                    this.platformSettingsService.DisplayPopupHint(PopUpHintSwitchInputMode + "Direct", 0);
                    this.platformEventService.OnCustomEditorSetInputType(
                        new CustomEditorEventArgs(TextInputTypes.TextVariationVisiblePassword));
                }

                this.directInputDisabled = value;
                OnPropertyChanged();
            }
        }

        public string CustomEditorText
        {
            get => this.customEditorText;
            set
            {
                if (!DirectInputDisabled && this.customEditorMessageModeText.Length == 0)
                {
                    if (value.Length == CustomEditorText.Length + 1)
                    {
                        this.inputEventHandlerService.KeyboardCommandSender.SendTextEntryAsync(value[^1]);
                    }
                    else if (CustomEditorText.Length == value.Length + 1)
                    {
                        this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(
                            KeyboardCommand.KeyDown, KeyCodeAction.Del);
                    }
                }

                this.customEditorText = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Command Methods

        private void OnSoftKeyboardButtonPressed() => CustomEditorIsFocused = true;
        private void OnSwitchInputModeButtonPressed() => DirectInputDisabled = !DirectInputDisabled;
        private void OnKeyboardButtonPressed() => KeyboardKeysVisible = !KeyboardKeysVisible;

        private async void OnSendButtonPressed(object o)
        {
            try
            {
                if (o != null)
                {
                    string editorText = o.ToString();

                    if (editorText != string.Empty)
                    {
                        await this.inputEventHandlerService.KeyboardCommandSender.SendTextEntryAsync(editorText);
                        CustomEditorText = string.Empty;
                        return;
                    }
                }

                await this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(
                    KeyboardCommand.KeyPress, KeyCodeAction.Enter);
            }
            catch (Exception e)
            {
                this.platformSettingsService.DisplayPopupHint(e.Message, 1);
                Debug.WriteLine(e);
            }
        }

        // Media Keys (excessive)
        private void OnMediaRewind() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                KeyCodeAction.MediaRewind);

        private void OnMediaPlayPause() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                KeyCodeAction.MediaPlayPause);

        private void OnMediaForward() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                KeyCodeAction.MediaFastForward);

        private void OnMediaPrevious() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                KeyCodeAction.MediaPrevious);

        private void OnMediaStop() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                KeyCodeAction.MediaStop);

        private void OnMediaNext() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                KeyCodeAction.MediaNext);

        private void OnVolumeDown() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                KeyCodeAction.VolumeDown);

        private void OnMute() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                KeyCodeAction.Mute);

        private void OnVolumeUp() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                KeyCodeAction.VolumeUp);

        #endregion

        #region Event Members

        private void OnViewAppearing(object sender, EventArgs e)
        {
            AddPlatformListeners();
            this.platformEventService.ViewDisappearing += OnViewDisappearing;
            this.platformEventService.ViewAppearing -= OnViewAppearing;
        }

        private void OnViewDisappearing(object sender, EventArgs e)
        {
            RemovePlatformListeners();
            this.platformEventService.ViewDisappearing -= OnViewDisappearing;
            this.platformEventService.ViewAppearing += OnViewAppearing;
        }

        private void OnBackPressed(object sender, EventArgs e)
        {
            RemovePlatformListeners();
            this.platformEventService.ViewAppearing -= OnViewAppearing;
            this.platformEventService.ViewDisappearing -= OnViewDisappearing;
            base.NavigationService.NavigateBackAsync();
        }

        private async void OnScreenTouched(object sender, TouchEventArgs e)
        {
            if (CustomEditorIsFocused)
                if (DirectInputDisabled)
                    return;
            if (e.Y > TouchpadHeight)
                return;

            await this.inputEventHandlerService.HandleTouchEventAsync(e);
        }

        private void OnCustomEditorAction(object sender, KeyEventArgs e)
        {
            this.inputEventHandlerService.HandleKeyEventAsync(e);
        }

        #endregion

        private void AddPlatformListeners()
        {
            this.platformEventService.CustomEditorAction += OnCustomEditorAction;
            this.platformEventService.ScreenTouched += OnScreenTouched;
            this.platformEventService.BackPressed += OnBackPressed;
        }

        private void RemovePlatformListeners()
        {
            this.platformEventService.CustomEditorAction -= OnCustomEditorAction;
            this.platformEventService.ScreenTouched -= OnScreenTouched;
            this.platformEventService.BackPressed -= OnBackPressed;
        }
    }
}