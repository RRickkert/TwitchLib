using System;
using System.Collections;
using System.Collections.Generic;

namespace TwitchLib.Services.CommandService
{
    public class ServiceCollection : IEnumerable<IService>
    {
        private readonly Dictionary<Type, IService> _services;

        internal CommandServiceClient Client { get; }

        internal ServiceCollection(CommandServiceClient client)
        {
            Client = client;
            _services = new Dictionary<Type, IService>();
        }

        public T Add<T>(T service)
            where T : class, IService
        {
            _services.Add(typeof(T), service);
            service.Install(Client);
            return service;
        }
        
        public T Get<T>(bool isRequired = true)
            where T : class, IService
        {
            T singletonT = null;

            if (_services.TryGetValue(typeof(T), out IService service))
                singletonT = service as T;

            if (singletonT == null && isRequired)
                throw new InvalidOperationException($"This operation requires {typeof(T).Name} to be added to {nameof(Client)}.");
            return singletonT;
        }

        public IEnumerator<IService> GetEnumerator() => _services.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _services.Values.GetEnumerator();
    }
}