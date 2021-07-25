namespace PointZClient.Dependency
{
    public interface IDependencyService
    {
        public T Get<T>() where T : class;
    }
}