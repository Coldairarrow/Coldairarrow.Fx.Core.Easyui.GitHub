using ImpromptuInterface;
using System.Collections.Concurrent;

namespace Coldairarrow.Util.RPC
{
    public class RPCClientFactory
    {
        private static ConcurrentDictionary<string, object> _services { get; } = new ConcurrentDictionary<string, object>();
        public static T GetClient<T>(string serverIp, int port) where T : class
        {
            T service = null;
            string key = $"{typeof(T).FullName}-{serverIp}-{port}";
            try
            {
                service = (T)_services[key];
            }
            catch
            {
                var clientProxy = new RPCClientProxy
                {
                    ServerIp = serverIp,
                    ServerPort = port,
                    ServiceType = typeof(T),
                    ServiceName = typeof(T).FullName
                };
                service = clientProxy.ActLike<T>();
                _services[key] = service;
            }

            return service;
        }
    }
}
