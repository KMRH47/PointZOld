using System;
using System.Globalization;
using System.Net.Sockets;
using System.Reflection;
using PointZ.Services.CommandSender;
using PointZ.Services.DeviceUserInterface;
using PointZ.Services.Logger;
using PointZ.Services.Navigation;
using PointZ.Services.TouchEventService;
using PointZ.Services.UdpListener;
using PointZ.TinyIoC;
using Xamarin.Forms;

namespace PointZ.ViewModels.Base
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
            Container.Register<ILogger, ConsoleLogger>();
            Container.Register<INavigationService, NavigationService>();
            Container.Register<ICommandSenderService, CommandSenderService>();
            Container.Register(DependencyService.Resolve<ITouchEventService>());
            Container.Register(DependencyService.Resolve<IPlatformNavigationService>());
            Container.Register(DependencyService.Resolve<IDeviceUserInterfaceService>());
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