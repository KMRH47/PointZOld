using Android.Content;
using PointZ.Android.Renderers;
using PointZ.Controls;
using PointZ.Models.AndroidTouchEvent;
using PointZ.Services.PlatformEventService;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TouchpadGrid), typeof(TouchpadGridRenderer))]

namespace PointZ.Android.Renderers
{
    public class TouchpadGridRenderer : ViewRenderer
    {
        private readonly IPlatformEventService platformEventService;
        
        public TouchpadGridRenderer(Context context) : base(context)
        {
            this.platformEventService = DependencyService.Resolve<IPlatformEventService>();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
        
            if (e.NewElement == null) return;
            
            System.Diagnostics.Debug.WriteLine($"TouchpadGridRenderer->OnElementChanged");

            Touch += (_, args) =>
            {
                float x = args.Event.GetX();
                float y = args.Event.GetY();
                AndroidTouchAction androidTouchAction = (AndroidTouchAction)((ushort)args.Event.Action);
                this.platformEventService.OnTouchpadGridTouched(new AndroidTouchEventArgs(x, y, androidTouchAction));
            };
        }
    }
}

