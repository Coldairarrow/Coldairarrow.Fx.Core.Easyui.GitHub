using Coldairarrow.Util;
using Coldairarrow.Util.Sockets;
using System;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Coldairarrow.Util.RPC
{
    class RPCClientProxy : DynamicObject
    {
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
        public string ServiceName { get; set; }
        public Type ServiceType { get; set; }
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                AutoResetEvent waitEvent = new AutoResetEvent(false);
                ResponseModel response = null;
                TcpSocketClient socketClient = new TcpSocketClient(ServerIp, ServerPort)
                {
                    HandleRecMsg = (a, bytes) =>
                    {
                        response = bytes.ToString(Encoding.UTF8).ToObject<ResponseModel>();
                        waitEvent.Set();
                    },
                    RecLength = 1024 * 1024,
                    HandleException = new Action<Exception>(ex=> {
                        Console.WriteLine(ExceptionHelper.GetExceptionAllMsg(ex));
                    })
                };
                socketClient.StartClient();
                RequestModel requestModel = new RequestModel
                {
                    ServiceName = ServiceName,
                    MethodName = binder.Name,
                    Paramters = args.ToList()
                };
                socketClient.Send(requestModel.ToJson().ToBytes(Encoding.UTF8));
                waitEvent.WaitOne(new TimeSpan(0,0,5));
                socketClient.Close();
                if (response == null)
                    throw new Exception("服务器超时未响应");
                else if (response.Success)
                {
                    result = response.Data.ToObject(ServiceType.GetMethod(binder.Name).ReturnType);
                    return true;
                }
                else
                    throw new Exception($"服务器异常，错误消息：{response.Msg}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
