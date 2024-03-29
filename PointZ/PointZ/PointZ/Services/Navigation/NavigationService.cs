﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using PointZ.ViewModels.Base;
using PointZ.Views;
using Xamarin.Forms;

namespace PointZ.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase =>
            InternalNavigateToAsync(typeof(TViewModel), null);

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase =>
            InternalNavigateToAsync(typeof(TViewModel), parameter);

        public async Task NavigateBackAsync()
        {
            if (Application.Current.MainPage is not CustomNavigationView mainPage) throw new Exception();

            await mainPage.PopAsync();
        }

        private static async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType);

            if (Application.Current.MainPage is CustomNavigationView navigationPage)
            {
                await navigationPage.PushAsync(page);
            }
            else
            {
                Application.Current.MainPage = new CustomNavigationView(page);
            }

            if (page.BindingContext == null) return;

            ViewModelBase viewModelBaseBindingContext = (ViewModelBase) page.BindingContext;
            
            await viewModelBaseBindingContext.InitializeAsync(parameter);
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

        /// <summary>
        /// Creates a page (view) that matches the view model passed.
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Page CreatePage(Type viewModelType)
        {
            try
            {
                Type pageType = GetPageTypeForViewModel(viewModelType);

                if (pageType == null) throw new Exception($"Cannot locate page type for {viewModelType}");

                Page page = Activator.CreateInstance(pageType) as Page;
                return page;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}