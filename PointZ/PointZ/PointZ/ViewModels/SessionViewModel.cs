﻿using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private readonly IInputEventHandlerService inputEventHandlerService;
        private readonly IPlatformEventService platformEventService;
        private readonly ISettingsService settingsService;
        private readonly IPlatformSettingsService platformSettingsService;

        private string customEditorText = "";
        private double touchpadHeight;
        private bool editorFocused;
        private bool directInputDisabled;
        private bool customEditorTextTransform;


        public SessionViewModel(
            IInputEventHandlerService inputEventHandlerService, IPlatformSettingsService platformSettingsService,
            IPlatformEventService platformEventService, ISettingsService settingsService)
        {
            this.inputEventHandlerService = inputEventHandlerService;
            this.platformSettingsService = platformSettingsService;
            this.platformEventService = platformEventService;
            this.settingsService = settingsService;
            KeyboardButtonCommand = new Command(OnKeyboardButtonPressed);
            ChangeInputModeCommand = new Command(OnInputModeButtonPressed);
            SendTextCommand = new Command(OnSendButtonPressed, CanPressSendButton);
        }


        public override Task InitializeAsync(object parameter)
        {
            IPEndPoint serverIpEndPoint = (IPEndPoint)parameter;
            this.settingsService.ServerIpEndPoint = serverIpEndPoint;
            this.platformSettingsService.SetSoftInputModeAdjustResize();
            OnViewAppearing(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public ICommand KeyboardButtonCommand { get; set; }
        public ICommand ChangeInputModeCommand { get; set; }
        public ICommand SendTextCommand { get; set; }

        public bool DirectInputDisabled
        {
            get => this.directInputDisabled;
            set
            {
                this.directInputDisabled = value;
                OnPropertyChanged();
            }
        }

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

        public bool CustomEditorTextTransform
        {
            get => this.customEditorTextTransform;
            set
            {
                this.customEditorTextTransform = value;
                OnPropertyChanged();
            }
        }

        public string CustomEditorText
        {
            get => this.customEditorText;
            set
            {
                if (!DirectInputDisabled)
                {
                    if (value.Length > CustomEditorText.Length)
                    {
                        this.inputEventHandlerService.KeyboardCommandSender.SendTextEntryAsync(value[^1]);
                    }
                    else
                    {
                        this.inputEventHandlerService.KeyboardCommandSender.SendKeyboardCommandAsync(
                            KeyboardCommand.KeyDown, KeyCodeAction.Del);
                    }
                }
                else
                {
                    OnPropertyChanged();
                }

                this.customEditorText = value;
            }
        }

        private void AddPlatformListeners()
        {
            this.platformEventService.CustomEditorBackPressed += OnCustomEditorBackPressed;
            this.platformEventService.ScreenTouched += OnScreenTouched;
            this.platformEventService.BackPressed += OnBackPressed;
        }

        private void RemovePlatformListeners()
        {
            this.platformEventService.CustomEditorBackPressed -= OnCustomEditorBackPressed;
            this.platformEventService.ScreenTouched -= OnScreenTouched;
            this.platformEventService.BackPressed -= OnBackPressed;
        }

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

        private void OnKeyboardButtonPressed()
        {
            Debug.WriteLine($"OnKeyboardButtonPressed");
            this.platformEventService.OnCustomEditorFocusRequested();
        }

        private void OnInputModeButtonPressed()
        {
            this.platformSettingsService.DisplayPopupHint(
                !DirectInputDisabled ? "Input mode: Direct" : "Input mode: Text", 0);
            DirectInputDisabled = !DirectInputDisabled;
            CustomEditorTextTransform = DirectInputDisabled;
            this.platformEventService.OnCustomEditorInputModeChanged();
        }

        private async void OnScreenTouched(object sender, TouchEventArgs e)
        {
            if (EditorFocused) return;

            if (e.Y > TouchpadHeight)
                if (e.TouchAction != TouchAction.Move)
                    return;

            await this.inputEventHandlerService.HandleTouchEventAsync(e);
        }

        private void OnCustomEditorBackPressed(object sender, KeyEventArgs e)
        {
            this.inputEventHandlerService.HandleKeyEventAsync(e);
        }

        private bool CanPressSendButton(object o) => DirectInputDisabled;

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
    }
}