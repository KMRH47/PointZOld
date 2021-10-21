using System;
using System.Globalization;
using System.Reflection;
using PointZ.Models.PlatformEvent;
using PointZ.Services.InputCommandSender;
using PointZ.Services.InputEventHandler;
using PointZ.Services.Logger;
using PointZ.Services.Navigation;
using PointZ.Services.PlatformEventService;
using PointZ.Services.PlatformSettings;
using PointZ.Services.Settings;
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
            Container.Register<IUdpListenerService, UdpListenerService>().AsSingleton();
            Container.Register<ILogger, ConsoleLogger>().AsSingleton();;
            Container.Register<INavigationService, NavigationService>().AsSingleton();;
            Container.Register<IInputEventHandler<TouchEventArgs>, TouchEventHandler>().AsSingleton();;
            Container.Register<IInputEventHandler<KeyEventArgs>, KeyboardEventHandler>().AsSingleton();;
            Container.Register<IKeyboardCommandSender, KeyboardCommandSender>().AsSingleton();;
            Container.Register<IMouseCommandSender, MouseCommandSender>().AsSingleton();;
            Container.Register<ISettingsService, SettingsService>().AsSingleton();;
            Container.Register(DependencyService.Resolve<IPlatformEventService>());
            Container.Register(DependencyService.Resolve<IPlatformSettingsService>());
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