using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PointZ.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomNavigationView : NavigationPage
    {
        public CustomNavigationView(Page root) : base(root)
        {
           // SetHasNavigationBar(root, false);
        }
    }
}