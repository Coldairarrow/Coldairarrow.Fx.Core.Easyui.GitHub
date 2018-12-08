using ImpromptuInterface;

namespace Coldairarrow.Util.RPC
{
    public class RPCClientFactory
    {
        public static T GetClient<T>(string serverIp, int port) where T : class
        {
            var clientProxy = new RPCClientProxy
            {
                _serverIp = serverIp,
                _serverPort = port,
                ServiceType = typeof(T),
                ServiceName = typeof(T).FullName
            };

            return clientProxy.ActLike<T>();
        }
    }
}
