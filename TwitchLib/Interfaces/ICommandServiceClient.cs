namespace TwitchLib.Services.CommandService
{
    public interface ICommandServiceClient
    {
        ITwitchClient TwitchClient { get; }

        T AddService<T>() where T : class, IService, new();
        T AddService<T>(T instance) where T : class, IService;
        T GetService<T>(bool isRequired = true) where T : class, IService;
    }
}