using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using PointZ.Models.Command;
using PointZ.Models.PlatformEvent;
using PointZ.Models.Server;
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
        private readonly IPlatformSettingsService platformSettingsService;
        private readonly ISettingsService settingsService;

        private string entryText;
        private double touchpadHeight;
        private bool touchedVisibleGrid;

        public SessionViewModel(
            IInputEventHandlerService inputEventHandlerService,
            ISettingsService settingsService,
            IPlatformSettingsService platformSettingsService,
            IPlatformEventService platformEventService)
        {
            this.inputEventHandlerService = inputEventHandlerService;
            this.settingsService = settingsService;
            this.platformSettingsService = platformSettingsService;
            this.platformEventService = platformEventService;
            ReturnPressedCommand = new Command(OnReturnPressed);
        }

        public override Task InitializeAsync(object parameter)
        {
            ServerData serverData = (ServerData)parameter;
            this.settingsService.ServerIpEndPoint = serverData.IpEndPoint;
            this.platformSettingsService.SetSoftInputModeAdjustResize();
            OnViewAppearing(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public ICommand ReturnPressedCommand { get; set; }

        public double TouchpadHeight
        {
            get => this.touchpadHeight;
            set => this.touchpadHeight = value * this.platformSettingsService.DisplayDensity;
        }

        public string EntryText
        {
            get => this.entryText;
            set
            {
                this.entryText = value;
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
            this.platformEventService.OnCustomEntryKeyPress += OnCustomEntryKeyPress;
        }

        private void RemovePlatformListeners()
        {
            this.platformEventService.OnScreenTouched -= OnScreenTouched;
            this.platformEventService.OnBackButtonPressed -= OnBackButtonPressed;
            this.platformEventService.OnCustomEntryKeyPress -= OnCustomEntryKeyPress;
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
            if (e.Y > TouchpadHeight) return;
            await this.inputEventHandlerService.HandleTouchEventAsync(e);
        }

        private async void OnCustomEntryKeyPress(object o, KeyEventArgs e) =>
            await this.inputEventHandlerService.HandleKeyEventAsync(e);

        private async void OnReturnPressed(object o)
        {
            try
            {
                string entryText = o.ToString();

                if (entryText != string.Empty)
                {
                    await this.inputEventHandlerService.KeyboardCommandSender.SendTextEntryAsync(entryText);
                    EntryText = string.Empty;
                }
                else
                {
                    await this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(
                        KeyboardCommand.KeyPress, KeyCodeAction.Enter);
                }
            }
            catch (Exception e)
            {
                this.platformSettingsService.DisplayPopupHint(e.Message);
                Debug.WriteLine(e);
            }
        }
    }
}