namespace TwitchLib.Services.CommandService.Modules
{ 
    public static class ModuleExtensions
    {
        public static ICommandServiceClient UsingModules(this CommandServiceClient client)
        {
            client.AddService(new ModuleService());
            return client;
        }

        public static void AddModule(this ICommandServiceClient client, IModule instance, string name = null)
        {
            client.GetService<ModuleService>().Add(instance, name);
        }
        public static void AddModule<T>(this ICommandServiceClient client, string name = null)
            where T : class, IModule, new()
        {
            client.GetService<ModuleService>().Add<T>(name);
        }
        public static void AddModule<T>(this ICommandServiceClient client, T instance, string name = null)
            where T : class, IModule
        {
            client.GetService<ModuleService>().Add(instance, name);
        }
        public static ModuleManager<T> GetModule<T>(this ICommandServiceClient client)
            where T : class, IModule
            => client.GetService<ModuleService>().Get<T>();
    }
}
