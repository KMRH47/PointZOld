﻿using PointZ.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
[assembly: ExportFont("fontello.ttf", Alias = "IconFont")]

namespace PointZ
{
    public partial class App : Application
    {
        public App()
        {
            Device.SetFlags(new[] {"Shapes_Experimental", "Brush_Experimental", "AppTheme_Experimental"});
            InitializeComponent();
            MainPage = new CustomNavigationView(new DiscoverView());
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