using System;
using TwitchLib.Services.CommandService;

namespace TwitchLib.Events.Services.CommandService
{
    public enum CommandErrorType { Exception, UnknownCommand, BadArgCount, InvalidInput }
    public class CommandErrorEventArgs : CommandEventArgs
    {
        public CommandErrorType ErrorType { get; }
        public Exception Exception { get; }

        public CommandErrorEventArgs(CommandErrorType errorType, CommandEventArgs baseArgs, ICommandServiceClient client, Exception ex)
            : base(baseArgs.Message, baseArgs.Command, client, baseArgs.Args)
        {
            Exception = ex;
            ErrorType = errorType;
        }
    }
}
