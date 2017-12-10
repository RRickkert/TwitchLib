using System.Linq;
using System;
using TwitchLib.Services.CommandService;
using TwitchLib.Models.Client;
using TwitchLib.Services.CommandService.Commands;

namespace TwitchLib.Events.Services.CommandService
{
    public class CommandEventArgs : EventArgs
    {
        private readonly string[] _args;

        public ChatMessage Message { get; }
        public Command Command { get; }
        public ICommandServiceClient Client { get; private set; }

        public CommandEventArgs(ChatMessage message, Command command, ICommandServiceClient client, string[] args)
        {
            Message = message;
            Command = command;
            Client = client;
            _args = args;
            if (command == null) return;
        }

        public bool IsBroadcaster {
            get
            {
                if (Message == null) return false;
                else
                    return Message.Badges.Any(b => b.Key.Equals("broadcaster", StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public bool IsModerator
        {
            get
            {
                if (Message == null) return false;
                else
                    return Message.IsModerator;
            }
        }

        public string[] Args => _args;
        public string GetArg(int index) => _args[index];
        public string GetArg(string name) => _args[Command[name].Id];
    }
}
