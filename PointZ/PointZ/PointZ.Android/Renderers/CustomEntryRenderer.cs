using Android.Content;
using PointZ.Android.Renderers;
using PointZ.Controls;
using PointZ.Models.PlatformEvent;
using PointZ.Services.PlatformEventService;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]

namespace PointZ.Android.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        private readonly IPlatformEventService platformEventService;

        public CustomEntryRenderer(Context context) : base(context)
        {
           this.platformEventService = DependencyService.Resolve<IPlatformEventService>();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            CustomEntry entry = (CustomEntry)this.Element;

            if (Control == null) return;
            if (entry == null) return;
            
            // Called when a keypress that doesn't alter the Text is pressed while the Entry is empty.
            // E.g.: return, backspace, etc.
            Control.KeyPress += (_, args) =>
            {
                System.Diagnostics.Debug.WriteLine($"CustomEntryRenderer->KeyPress");
                System.Diagnostics.Debug.WriteLine($"Action: {args.Event.Action}");
                System.Diagnostics.Debug.WriteLine($"KeyCode: {args.KeyCode}");
                KeyAction keyAction = (KeyAction)((ushort)args.Event.Action);
                Models.PlatformEvent.KeyEventArgs keyEventArgs = new (keyAction,args.KeyCode.ToString());
                this.platformEventService.NotifyOnCustomEntryKeyPress(keyEventArgs);
            };
                
            // EditorAction is the method invoked when "Done/Return/Send/etc." is pressed on the soft input.
            Control.EditorAction += (_, _) =>
            {
                // SendCompleted signals that the EditorAction is completed.
                // The normal procedure that hides the soft input and unfocuses this Entry is no longer happening. 
                entry.SendCompleted();  
            };
        }
    }
}