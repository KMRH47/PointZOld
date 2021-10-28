using System;
using Android.Content;
using Android.Text;
using Android.Views;
using PointZ.Android.Renderers;
using PointZ.Controls;
using PointZ.Models.PlatformEvent;
using PointZ.Services.PlatformEventService;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]

namespace PointZ.Android.Renderers
{
    public sealed class CustomEditorRenderer : EditorRenderer
    {
        private readonly IPlatformEventService platformEventService;
        private Editor editor;

        public CustomEditorRenderer(Context context) : base(context)
        {
            this.platformEventService = DependencyService.Resolve<IPlatformEventService>();
            this.platformEventService.CustomEditorFocusRequested += OnFocusRequested;
            this.platformEventService.CustomEditorInputModeChanged += OnInputModeChanged;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            Editor editor = this.Element;

            if (Control == null) return;
            if (editor == null) return;

            Control.SetBackground(null);
            this.editor = editor;
            

            Control.KeyPress += (_, args) =>
            {
                if (args?.Event == null) return; switch (args.Event.Action)
                {
                    case KeyEventActions.Down:
                        if (args.KeyCode == Keycode.Del)
                        {
                            Models.PlatformEvent.KeyEventArgs keyEventArgs = new(KeyAction.Down,
                                KeyCodeAction.Del.ToString());
                            this.platformEventService.OnCustomEditorBackPressed(keyEventArgs);
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
        }

        private void OnFocusRequested(object sender, EventArgs e) => this.editor.Focus();

        private void OnInputModeChanged(object sender, EventArgs e)
        {
            this.Control.InputType = InputTypes.TextVariationVisiblePassword;
        }
    }
}