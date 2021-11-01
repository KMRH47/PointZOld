using Android.Content;
using PointZ.Android.Renderers;
using PointZ.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SoftKeyboardButton), typeof(SoftKeyboardButtonRenderer))]

namespace PointZ.Android.Renderers
{
    public class SoftKeyboardButtonRenderer : ButtonRenderer
    {
        public SoftKeyboardButtonRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            
            SoftKeyboardButton softKeyboardButton = (SoftKeyboardButton)this.Element;

            if (Control == null) return;
            if (softKeyboardButton == null) return;

            softKeyboardButton.Pressed += (_, _) => softKeyboardButton.Unfocus();
        }
    }
}