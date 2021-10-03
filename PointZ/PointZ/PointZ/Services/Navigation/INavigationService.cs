using System.Threading.Tasks;
using PointZ.ViewModels.Base;

namespace PointZ.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        Task NavigateBackAsync();
    }
}