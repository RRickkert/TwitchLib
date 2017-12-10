using System.Collections.Generic;
using TwitchLib.Services.CommandService.Modules;

namespace TwitchLib.Services.CommandService
{
    public abstract class TwitchModule : IModule
    {
        protected readonly HashSet<TwitchCommand> commands = new HashSet<TwitchCommand>();

        public abstract string Prefix { get; }

        public abstract void Install(ModuleManager manager);

        public readonly ICommandServiceClient Client;

        public TwitchModule(ICommandServiceClient client)
        {
            Client = client;
        }
    }
}
