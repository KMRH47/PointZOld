using Android.Content;
using PointZ.Android.Renderers;
using PointZ.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(KeyboardButton), typeof(KeyboardButtonRenderer))]

namespace PointZ.Android.Renderers
{
    public class KeyboardButtonRenderer : ButtonRenderer
    {
        public KeyboardButtonRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            
            KeyboardButton keyboardButton = (KeyboardButton)this.Element;

            if (Control == null) return;
            if (keyboardButton == null) return;

            keyboardButton.Pressed += (_, _) => keyboardButton.Unfocus();
        }
    }
}