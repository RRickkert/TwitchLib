using System;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Events.Services.CommandService;
using TwitchLib.Services.CommandService;
using TwitchLib.Services.CommandService.Commands;

namespace NetCore.Client.Modules.Utilities.Commands
{
    public class Uptime : TwitchCommand
    {
        public Uptime(TwitchModule module) : base(module)
        {
        }

        public override void Init(CommandGroupBuilder cgb)
        {
            cgb.CreateCommand(Module.Prefix + "uptime")
                .Description("Show Stream Uptime")
                .Do(DoUptime());
        }

        private Func<CommandEventArgs, Task> DoUptime() =>
            async e => {

                var foundChannel = await Program.Api.Users.v5.GetUserByNameAsync(Program.Channel);
                var channel = foundChannel.Matches.FirstOrDefault();

                if (channel != null)
                {
                    var online = await Program.Api.Streams.v5.BroadcasterOnlineAsync(channel.Id);
                    if (!online)
                    {
                        TwitchClient.SendMessage(".me says Streamer isn't streaming right now Ooops!");
                        return;
                    }

                    var uptime = await Program.Api.Streams.v5.GetUptimeAsync(channel.Id);
                    if (!uptime.HasValue)
                    {
                        TwitchClient.SendMessage(".me says Error getting uptime :v");
                        return;
                    }
                    TwitchClient.SendMessage($".me says Streamer Live for {uptime.Value.Hours} {(uptime.Value.Hours == 1 ? "hour" : "hours")} {uptime.Value.Minutes} {(uptime.Value.Minutes == 1 ? "minute" : "minutes")}.");
                }
            };
    }
}
