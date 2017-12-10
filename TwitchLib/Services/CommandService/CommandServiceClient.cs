namespace TwitchLib.Services.CommandService
{
    public class CommandServiceClient : ICommandServiceClient
    {
        private readonly ServiceCollection _services;
        public ITwitchClient TwitchClient { get; }

        public CommandServiceClient(ITwitchClient client)
        {
            _services = new ServiceCollection(this);
            TwitchClient = client;
        }

        #region Services
        public T AddService<T>(T instance)
            where T : class, IService
            => _services.Add(instance);
        public T AddService<T>()
            where T : class, IService, new()
            => _services.Add(new T());
        public T GetService<T>(bool isRequired = true)
            where T : class, IService
            => _services.Get<T>(isRequired);
        #endregion
    }
}