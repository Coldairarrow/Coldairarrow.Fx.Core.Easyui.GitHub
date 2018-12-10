using Coldairarrow.Util.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.Util.RPC
{
    public class RPCServer
    {
        #region 构造函数

        public RPCServer(int port)
        {
            _port = port;
        }

        #endregion

        #region 私有成员

        private int _port { get; set; }
        private TcpSocketServer _socketServer { get; set; }
        private Dictionary<string, Type> _serviceHandle { get; set; } = new Dictionary<string, Type>();

        #endregion

        #region 外部接口

        public void Start()
        {
            _socketServer = new TcpSocketServer(_port)
            {
                HandleRecMsg = (a, b, bytes) =>
                {
                    ResponseModel response = new ResponseModel();

                    try
                    {
                        var requestModel = bytes.ToString(Encoding.UTF8).ToObject<RequestModel>();
                        if (!_serviceHandle.ContainsKey(requestModel.ServiceName))
                            throw new Exception("未找到该服务");
                        var serviceType = _serviceHandle[requestModel.ServiceName];
                        var service = Activator.CreateInstance(serviceType);
                        var method = serviceType.GetMethod(requestModel.MethodName);
                        if (method == null)
                            throw new Exception("未找到该方法");
                        var res = method.Invoke(service, requestModel.Paramters.ToArray());

                        response.Success = true;
                        response.Data = res.ToJson();
                        response.Msg = "请求成功";
                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Msg = ExceptionHelper.GetExceptionAllMsg(ex);
                    }
                    finally
                    {
                        b.Send(response.ToJson().ToBytes(Encoding.UTF8));
                        b.Close();
                    }
                },
                RecLength = 1024 * 1024,
                HandleException = this.HandleException == null ? null : new Action<Exception>(this.HandleException)
            };
            _socketServer.StartServer();
        }

        public void RegisterService<IService, Service>() where Service : class, IService where IService : class
        {
            _serviceHandle.Add(typeof(IService).FullName, typeof(Service));
        }

        public void Stop()
        {
            _socketServer.StopServer();
        }

        public Action<Exception> HandleException { get; set; }

        #endregion
    }
}
