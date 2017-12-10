using System;
using TwitchLib.Services.CommandService.Commands;

namespace TwitchLib.Services.CommandService.Modules
{
    public class ModuleManager<T> : ModuleManager
        where T : class, IModule
    {
        public new T Instance => base.Instance as T;

        internal ModuleManager(ICommandServiceClient client, T instance, string name)
            : base(client, instance, name)
        {
        }
    }

    public class ModuleManager
	{
        private readonly object _lock;

        public ICommandServiceClient Client { get; }
        public IModule Instance { get; }
        public string Name { get; }
		public string Id { get; }
        
		internal ModuleManager(ICommandServiceClient client, IModule instance, string name)
		{
            Client = client;
            Instance = instance;
            Name = name;

            Id = name.ToLowerInvariant();
            _lock = new object();
		}

		public void CreateCommands(string prefix, Action<CommandGroupBuilder> config)
		{
			var commandService = Client.GetService<Commands.CommandService>();
			commandService.CreateGroup(prefix, x =>
			{
				x.Category(Name);
				config(x);
            });

		}
	}
}
