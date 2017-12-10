using TwitchLib.Services.CommandService;
using TwitchLib.Services.CommandService.Modules;

namespace NetCore.Client.Modules.Utilities
{
    public class UtilitiesModule : TwitchModule
    {
        public UtilitiesModule(ICommandServiceClient client) : base(client)
        {
            commands.Add(new Commands.Uptime(this));
            commands.Add(new Commands.Caster(this));
        }

        public override string Prefix { get; } = "!";

        public override void Install(ModuleManager manager)
        {
            manager.CreateCommands("", cgb =>
            {
                foreach (var cmd in commands)
                {
                    cmd.Init(cgb);
                }
            });
        }
    }
}
