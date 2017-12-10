using TwitchLib.Services.CommandService;

namespace TwitchLib.Services
{
    public interface IService
    {
        void Install(ICommandServiceClient client);
    }
}