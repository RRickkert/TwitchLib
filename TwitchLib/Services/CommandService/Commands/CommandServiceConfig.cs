using System;
using TwitchLib.Events.Services.CommandService;
using TwitchLib.Models.Client;

namespace TwitchLib.Services.CommandService.Commands
{
    public class CommandServiceConfigBuilder
    {
		public char? PrefixChar { get; set; } = null;
        public Func<ChatMessage, int> CustomPrefixHandler { get; set; } = null;
        public EventHandler<CommandEventArgs> ExecuteHandler { get; set; }
        public EventHandler<CommandErrorEventArgs> ErrorHandler { get; set; }

        public CommandServiceConfig Build() => new CommandServiceConfig(this);
    }

    public class CommandServiceConfig
    {
        public char? PrefixChar { get; }
        public Func<ChatMessage, int> CustomPrefixHandler { get; }

        internal CommandServiceConfig(CommandServiceConfigBuilder builder)
        {
            PrefixChar = builder.PrefixChar;
            CustomPrefixHandler = builder.CustomPrefixHandler;
        }
    }
}
