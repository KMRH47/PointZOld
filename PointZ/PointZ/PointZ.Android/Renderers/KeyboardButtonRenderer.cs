using Android.Content;
using PointZ.Android.Renderers;
using PointZ.Controls;
using PointZ.Services.PlatformEventService;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(KeyboardButton), typeof(KeyboardButtonRenderer))]

namespace PointZ.Android.Renderers
{
    public class KeyboardButtonRenderer : ButtonRenderer
    {
        private readonly IPlatformEventService platformEventService;

        public KeyboardButtonRenderer(Context context) : base(context)
        {
            this.platformEventService = DependencyService.Resolve<IPlatformEventService>();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            KeyboardButton keyboardButton = (KeyboardButton)this.Element;

            if (Control == null) return;
            if (keyboardButton == null) return;
            
            keyboardButton.Pressed += (sender, args) =>
            {
                keyboardButton.Unfocus();
                this.platformEventService.NotifyOnCustomEditorFocusRequested();
            };
        }
    }
}