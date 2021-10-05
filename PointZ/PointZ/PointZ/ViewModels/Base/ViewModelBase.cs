using System.Threading.Tasks;
using PointZ.Services.Navigation;
using Xamarin.Forms;

namespace PointZ.ViewModels.Base
{
    public abstract class ViewModelBase : BindableObject
    {
        protected readonly INavigationService NavigationService;

        protected ViewModelBase() => this.NavigationService = ViewModelLocator.Resolve<INavigationService>();

        public virtual Task InitializeAsync(object parameter) => Task.FromResult(false);
    }
}