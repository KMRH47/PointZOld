using PointZClient.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace PointZClient
{
    public partial class App : Application
    {
        public App()
        {
            Device.SetFlags(new[] {"Shapes_Experimental", "Brush_Experimental", "AppTheme_Experimental"});
            InitializeComponent();
            MainPage = new DiscoverView();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}