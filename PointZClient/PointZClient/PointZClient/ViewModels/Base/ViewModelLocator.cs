using System;
using System.Globalization;
using System.Reflection;
using PointZClient.Services.UdpListener;
using TinyIoC;
using Xamarin.Forms;

namespace PointZClient.ViewModels.Base
{
    public class ViewModelLocator
    {
         private static readonly TinyIoCContainer Container;

        static ViewModelLocator()
        {
            Container = new TinyIoCContainer();

            // Object registration
            // By default, concrete classes are registered as instances and interfaces implementations as singletons.

            // View models
            Container.Register<DiscoverViewModel>();
            Container.Register<SessionViewModel>();

            // Services
            Container.Register<IUdpListenerService, UdpListenerService>();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached(
                "AutoWireViewModel",
                typeof(bool),
                typeof(ViewModelLocator),
                default(bool),
                propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable) =>
            (bool) bindable.GetValue(AutoWireViewModelProperty);

        public static void SetAutoWireViewModel(BindableObject bindable, bool value) =>
            bindable.SetValue(AutoWireViewModelProperty, value);

        public static T Resolve<T>() where T : class => Container.Resolve<T>();

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Element view = bindable as Element;
            Type viewType = view?.GetType();
            if (viewType?.FullName == null) return;

            string viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            string viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            string viewModelName =
                string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            Type viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null) return;

            object viewModel = Container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}