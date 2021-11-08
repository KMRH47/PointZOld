using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using PointZ.Models.AndroidKeyEvent;
using PointZ.Models.AndroidTouchEvent;
using PointZ.Models.CustomEditor;
using PointZ.Models.Input;
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
            MediaRewindCommand = new Command(OnMediaRewind);
            MediaPlayPauseCommand = new Command(OnMediaPlayPause);
            MediaForwardCommand = new Command(OnMediaForward);
            MediaPreviousCommand = new Command(OnMediaPrevious);
            MediaStopCommand = new Command(OnMediaStop);
            MediaNextCommand = new Command(OnMediaNext);
            VolumeDownCommand = new Command(OnVolumeDown);
            MuteCommand = new Command(OnMute);
            VolumeUpCommand = new Command(OnVolumeUp);

            // Keyboard Misc Keys (excessive)
            ArrowLeftCommand = new Command(OnArrowLeft);
            ArrowUpCommand = new Command(OnArrowUp);
            ArrowRightCommand = new Command(OnArrowRight);
            ArrowDownCommand = new Command(OnArrowDown);
            PageUpCommand = new Command(OnPageUp);
            PageDownCommand = new Command(OnPageDown);
            CtrlCommand = new Command(OnCtrl);
            WinCmdSupCommand = new Command(OnWinCmdSup);
            AltOptCommand = new Command(OnAltOpt);
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
        public ICommand MediaRewindCommand { get; set; }
        public ICommand MediaPlayPauseCommand { get; set; }
        public ICommand MediaForwardCommand { get; set; }
        public ICommand MediaPreviousCommand { get; set; }
        public ICommand MediaStopCommand { get; set; }
        public ICommand MediaNextCommand { get; set; }
        public ICommand VolumeDownCommand { get; set; }
        public ICommand MuteCommand { get; set; }
        public ICommand VolumeUpCommand { get; set; }

        // Keyboard Misc Keys (excessive)
        public ICommand ArrowLeftCommand { get; set; }
        public ICommand ArrowUpCommand { get; set; }
        public ICommand ArrowRightCommand { get; set; }
        public ICommand ArrowDownCommand { get; set; }
        public ICommand PageUpCommand { get; set; }
        public ICommand PageDownCommand { get; set; }
        public ICommand CtrlCommand { get; set; }
        public ICommand WinCmdSupCommand { get; set; }
        public ICommand AltOptCommand { get; set; }

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
                    this.platformSettingsService.DisplayPopupHint(PopUpHintSwitchInputMode + "Message", true);
                    this.platformEventService.OnCustomEditorSetInputType(
                        new CustomEditorEventArgs(TextInputTypes.ClassText | TextInputTypes.TextFlagMultiLine));
                }
                else
                {
                    this.customEditorMessageModeText = CustomEditorText;
                    CustomEditorText = string.Empty;
                    this.platformSettingsService.DisplayPopupHint(PopUpHintSwitchInputMode + "Direct", true);
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
                            KeyboardCommand.KeyDown, AndroidKeyCodeAction.Del);
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
                    KeyboardCommand.KeyPress, AndroidKeyCodeAction.Enter);
            }
            catch (Exception e)
            {
                this.platformSettingsService.DisplayPopupHint(e.Message, false);
                Debug.WriteLine(e);
            }
        }

        // Media Keys (excessive)
        private void OnMediaRewind() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.MediaRewind);

        private void OnMediaPlayPause() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.MediaPlayPause);

        private void OnMediaForward() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.MediaFastForward);

        private void OnMediaPrevious() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.MediaPrevious);

        private void OnMediaStop() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.MediaStop);

        private void OnMediaNext() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.MediaNext);

        private void OnVolumeDown() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.VolumeDown);

        private void OnMute() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.Mute);

        private void OnVolumeUp() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.VolumeUp);


        private void OnArrowLeft() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.DpadLeft);
        private void OnArrowUp() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.DpadUp);
        
        private void OnArrowRight() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.DpadRight);
        private void OnArrowDown() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.DpadDown);
        private void OnPageUp() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.PageUp);
        private void OnPageDown() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.PageDown);
        private void OnCtrl() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.CtrlLeft);
        /// <summary>
        /// Can't find any answer in the documentation that provides a Windows/Super/Command key equivalent.
        /// </summary>
        private void OnWinCmdSup() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.Window);
        private void OnAltOpt() =>
            this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(KeyboardCommand.KeyDown,
                AndroidKeyCodeAction.AltLeft);
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

        private async void OnTouchpadGridTouched(object sender, AndroidTouchEventArgs e)
        {
            if (CustomEditorIsFocused)
                if (DirectInputDisabled)
                    return;

            await this.inputEventHandlerService.HandleTouchEventAsync(e);
        }

        private void OnCustomEditorAction(object sender, AndroidKeyEventArgs e)
        {
            this.inputEventHandlerService.HandleKeyEventAsync(e);
        }

        #endregion

        private void AddPlatformListeners()
        {
            this.platformEventService.CustomEditorAction += OnCustomEditorAction;
            this.platformEventService.TouchpadGridTouched += OnTouchpadGridTouched;
            this.platformEventService.BackPressed += OnBackPressed;
        }

        private void RemovePlatformListeners()
        {
            this.platformEventService.CustomEditorAction -= OnCustomEditorAction;
            this.platformEventService.TouchpadGridTouched -= OnTouchpadGridTouched;
            this.platformEventService.BackPressed -= OnBackPressed;
        }
    }
}