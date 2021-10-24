using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using PointZ.Models.Command;
using PointZ.Models.PlatformEvent;
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
        private readonly IInputEventHandlerService inputEventHandlerService;
        private readonly IPlatformEventService platformEventService;
        private readonly ISettingsService settingsService;
        private readonly IPlatformSettingsService platformSettingsService;

        private string editorText;
        private double touchpadHeight;
        private bool editorFocused;

        public SessionViewModel(
            IInputEventHandlerService inputEventHandlerService, IPlatformSettingsService platformSettingsService,
            IPlatformEventService platformEventService, ISettingsService settingsService)
        {
            this.inputEventHandlerService = inputEventHandlerService;
            this.platformSettingsService = platformSettingsService;
            this.platformEventService = platformEventService;
            this.settingsService = settingsService;
            SendEditorTextCommand = new Command(OnEditorTextSend);
        }

        public override Task InitializeAsync(object parameter)
        {
            IPEndPoint serverIpEndPoint = (IPEndPoint)parameter;
            this.settingsService.ServerIpEndPoint = serverIpEndPoint;
            this.platformSettingsService.SetSoftInputModeAdjustResize();
            OnViewAppearing(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public ICommand SendEditorTextCommand { get; set; }

        public bool EditorFocused
        {
            get => this.editorFocused;
            set
            {
                this.editorFocused = value;
                OnPropertyChanged();
            }
        }

        public double TouchpadHeight
        {
            get => this.touchpadHeight;
            set => this.touchpadHeight = value * this.platformSettingsService.DisplayDensity;
        }

        public string EditorText
        {
            get => this.editorText;
            set
            {
                this.editorText = value;
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
            this.platformEventService.OnCustomEditorBackPressed += OnCustomEditorBackPressed;
            this.platformEventService.OnScreenTouched += OnScreenTouched;
            this.platformEventService.OnBackButtonPressed += OnBackButtonPressed;
        }

        private void RemovePlatformListeners()
        {
            this.platformEventService.OnCustomEditorBackPressed -= OnCustomEditorBackPressed;
            this.platformEventService.OnScreenTouched -= OnScreenTouched;
            this.platformEventService.OnBackButtonPressed -= OnBackButtonPressed;
        }

        /// <summary>
        /// Not called when the Entry has focus.
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

        private async void OnScreenTouched(object sender, TouchEventArgs e)
        {
            if (e.Y > TouchpadHeight)
                if (e.TouchAction != TouchAction.Move)
                    return;

            await this.inputEventHandlerService.HandleTouchEventAsync(e);
        }

        private void OnCustomEditorBackPressed(object sender, KeyEventArgs e) =>
            this.inputEventHandlerService.HandleKeyEventAsync(e);

        private async void OnEditorTextSend(object o)
        {
            try
            {
                if (o != null)
                {
                    string editorText = o.ToString();

                    if (editorText != string.Empty)
                    {
                        await this.inputEventHandlerService.KeyboardCommandSender.SendTextEntryAsync(editorText);
                        EditorText = string.Empty;
                        return;
                    }
                }

                await this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(
                    KeyboardCommand.KeyPress, KeyCodeAction.Enter);
            }
            catch (Exception e)
            {
                this.platformSettingsService.DisplayPopupHint(e.Message);
                Debug.WriteLine(e);
            }
        }
    }
}