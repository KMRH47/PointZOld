namespace PointZClient.Dependency
{
    public class DependencyService : IDependencyService
    {
        public T Get<T>() where T : class => Xamarin.Forms.DependencyService.Get<T>();
    }
}