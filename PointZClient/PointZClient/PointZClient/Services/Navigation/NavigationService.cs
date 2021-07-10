using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using PointZClient.ViewModels.Base;
using PointZClient.Views;
using Xamarin.Forms;

namespace PointZClient.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public ViewModelBase PreviousPageViewModel
        {
            get
            {
                Page mainPage = Application.Current.MainPage;
                bool mainPageIsCustomNavigationView = mainPage is CustomNavigationView;

                if (mainPageIsCustomNavigationView) return null;

                object viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]
                    .BindingContext;
                return viewModel as ViewModelBase;
            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase =>
            InternalNavigateToAsync(typeof(TViewModel), null);

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase =>
            InternalNavigateToAsync(typeof(TViewModel), parameter);

        public Task RemoveLastFromBackStackAsync()
        {
            CustomNavigationView mainPage = Application.Current.MainPage as CustomNavigationView;

            mainPage?.Navigation.RemovePage(
                mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);

            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync()
        {
            if (Application.Current.MainPage is not CustomNavigationView mainPage)
                return Task.FromResult(true);

            for (int i = 0; i < mainPage.Navigation.NavigationStack.Count - 1; i++)
            {
                Page page = mainPage.Navigation.NavigationStack[i];
                mainPage.Navigation.RemovePage(page);
            }

            return Task.FromResult(true);
        }

        private static async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);

            if (page is DiscoverView)
            {
                Application.Current.MainPage = new CustomNavigationView(page);
            }
            else
            {
                if (Application.Current.MainPage is CustomNavigationView navigationPage)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    Application.Current.MainPage = new CustomNavigationView(page);
                }
            }

            if (page.BindingContext == null) return;

            await Task.CompletedTask;
            // ViewModelBase viewModelBaseBindingContext = (ViewModelBase) page.BindingContext;
            //await viewModelBaseBindingContext.InitializeAsync(parameter);
        }

        private static Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (viewModelType.FullName == null)
                throw new ArgumentNullException($"View model '{viewModelType.Name}' does not exist.");

            string viewName = viewModelType.FullName.Replace("Model", string.Empty);
            string viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            string viewAssemblyName =
                string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            Type viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private static Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null) throw new Exception($"Cannot locate page type for {viewModelType}");

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }
    }
}