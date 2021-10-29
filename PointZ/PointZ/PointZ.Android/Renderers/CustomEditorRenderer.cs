using System;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using PointZ.Android.Renderers;
using PointZ.Controls;
using PointZ.Models.CustomEditor;
using PointZ.Models.KeyEvent;
using PointZ.Services.PlatformEventService;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]

namespace PointZ.Android.Renderers
{
    public sealed class CustomEditorRenderer : EditorRenderer
    {
        private readonly IPlatformEventService platformEventService;

        public CustomEditorRenderer(Context context) : base(context)
        {
            this.platformEventService = DependencyService.Resolve<IPlatformEventService>();
            this.platformEventService.CustomEditorFocusRequested += OnFocusRequested;
            this.platformEventService.CustomEditorSetInputType += OnSetInputType;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control == null) return;
            if (Element == null) return;

            Control.SetBackground(null);
            Control.InputType = InputTypes.TextVariationVisiblePassword;

            Control.KeyPress += (_, args) =>
            {
                if (args?.Event == null) return;
                switch (args.Event.Action)
                {
                    case KeyEventActions.Down:
                        if (args.KeyCode == Keycode.Del)
                        {
                            Models.KeyEvent.KeyEventArgs keyEventArgs = new(KeyAction.Down, KeyCodeAction.Del);
                            this.platformEventService.OnCustomEditorAction(keyEventArgs);
                            System.Diagnostics.Debug.WriteLine(
                                $"CustomEditorRenderer->Control.KeyPress(args: {args.Event})");
                        }

                        Control.OnKeyDown(args.KeyCode, args.Event); // Resume event
                        break;
                    case KeyEventActions.Multiple:
                        Control.OnKeyMultiple(args.KeyCode, args.Event.RepeatCount, args.Event);
                        break;
                    case KeyEventActions.Up:
                        Control.OnKeyUp(args.KeyCode, args.Event);
                        break;
                }
            };

            Control.EditorAction += (o, args) =>
            {
                System.Diagnostics.Debug.WriteLine(
                    $"OnEditorAction... Event: {args.Event}, ActionID: {args.ActionId}, Handled: {args.Handled}");
                if (args.ActionId == ImeAction.Done)
                {
                    // Direct mode
                    Models.KeyEvent.KeyEventArgs keyEventArgs = new(KeyAction.Down, KeyCodeAction.Enter);
                    this.platformEventService.OnCustomEditorAction(keyEventArgs);
                }
                else
                {
                    // Text mode
                    switch (args.Event.Action)
                    {
                        case KeyEventActions.Down:
                            Element.Text += "\n";
                            break;
                    }
                }
            };
        }

        private void OnFocusRequested(object sender, EventArgs e) => Element.Focus();

        private void OnSetInputType(object sender, CustomEditorEventArgs e)
        {
            InputTypes inputType = (InputTypes)e.TextInputTypes;
            Control.InputType = inputType;
            Control.SetSelection(Element.Text.Length);
        }
    }
}