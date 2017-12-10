using System;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Events.Services.CommandService;
using TwitchLib.Services.CommandService;
using TwitchLib.Services.CommandService.Commands;

namespace NetCore.Client.Modules.Utilities.Commands
{
    public class Caster : TwitchCommand
    {
        public Caster(TwitchModule module) : base(module)
        { }

        public override void Init(CommandGroupBuilder cgb)
        {
            cgb.CreateCommand(Module.Prefix + "caster")
                       .Alias(Module.Prefix + "shoutout")
                       .Parameter("target", ParameterType.Required)
                       .Description("Shoutout a Caster - Go Give a Follow")
                       .Do(DoCaster());
        }

        private Func<CommandEventArgs, Task> DoCaster() =>
            async e =>
            {
                if (!e.IsBroadcaster && !e.IsModerator) return;
                var streamer = e.GetArg("target");


                var foundChannel = await Program.Api.Users.v5.GetUserByNameAsync(streamer);
                var channel = foundChannel.Matches.FirstOrDefault();

                if (channel == null)
                {
                    TwitchClient.SendMessage($".me says You should give @{streamer.ToUpper()} a follow over at http://www.twitch.tv/{streamer}");
                    return;
                }

                var channelDetails = await Program.Api.Channels.v5.GetChannelByIDAsync(channel.Id);

                if (channelDetails != null)
                    TwitchClient.SendMessage($".me says You should give @{streamer.ToUpper()} a follow over at http://www.twitch.tv/{streamer} - They were last playing: {channelDetails.Game}");
                else
                    TwitchClient.SendMessage($".me says You should give @{streamer.ToUpper()} a follow over at http://www.twitch.tv/{streamer}");
            };
    }
}