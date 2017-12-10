using TwitchLib.Logging;
using TwitchLib.Services.CommandService.Commands;

namespace TwitchLib.Services.CommandService
{
    public abstract class TwitchCommand
    {
        /// <summary>
        /// Parent module
        /// </summary>
        protected TwitchModule Module { get; }
        protected ICommandServiceClient Client { get; }
        protected ITwitchClient TwitchClient { get; }
        protected ILogger Logger;

        /// <summary>
        /// Creates a new instance of twitch command,
        /// use ": base(module)" in the derived class'
        /// constructor to make sure module is assigned
        /// </summary>
        /// <param name="module">Module this command resides in</param>
        public TwitchCommand(TwitchModule module)
        {
            Module = module;
            Client = module.Client;
            TwitchClient = Client.TwitchClient;
            Logger = TwitchClient.Logger;
        }

        public virtual void Init(CommandGroupBuilder cgb)
        { }
    }
}
