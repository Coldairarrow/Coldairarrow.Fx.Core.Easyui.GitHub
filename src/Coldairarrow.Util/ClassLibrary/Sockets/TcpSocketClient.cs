using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Coldairarrow.Util.Sockets
{
    /// <summary>
    /// TcpSocket客户端
    /// </summary>
    public class TcpSocketClient
    {
        #region 构造函数

        /// <summary>
        /// 构造函数,连接服务器IP地址默认为本机127.0.0.1
        /// </summary>
        /// <param name="port">监听的端口</param>
        public TcpSocketClient(int port)
        {
            _ip = "127.0.0.1";
            _port = port;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">监听的IP地址</param>
        /// <param name="port">监听的端口</param>
        public TcpSocketClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        #endregion

        #region 内部成员

        private Socket _socket = null;
        private string _ip = "";
        private int _port = 0;
        private bool _isRec = true;

        /// <summary>
        /// 开始接受客户端消息
        /// </summary>
        public void StartRecMsg()
        {
            try
            {
                byte[] container = new byte[1024];
                _socket.BeginReceive(container, 0, container.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = _socket.EndReceive(asyncResult);

                        //马上进行下一轮接受，增加吞吐量
                        if (length > 0 && _isRec && IsSocketConnected())
                            StartRecMsg();

                        if (length > 0)
                        {
                            byte[] recBytes = new byte[length];
                            Array.Copy(container, 0, recBytes, 0, length);

                            //处理消息
                            HandleRecMsg?.BeginInvoke(this, recBytes, null, null);
                        }
                        else
                            Close();
                    }
                    catch (Exception ex)
                    {
                        HandleException?.BeginInvoke(ex, null, null);
                        Close();
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.BeginInvoke(ex, null, null);
                Close();
            }
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 开始服务，连接服务端
        /// </summary>
        public void StartClient()
        {
            try
            {
                //实例化 套接字 （ip4寻址协议，流式传输，TCP协议）
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //创建 ip对象
                IPAddress address = IPAddress.Parse(_ip);
                //创建网络节点对象 包含 ip和port
                IPEndPoint endpoint = new IPEndPoint(address, _port);
                //将 监听套接字  绑定到 对应的IP和端口
                _socket.BeginConnect(endpoint, asyncResult =>
                {
                    try
                    {
                        _socket.EndConnect(asyncResult);
                        //开始接受服务器消息
                        StartRecMsg();

                        HandleClientStarted?.BeginInvoke(this, null, null);
                    }
                    catch (Exception ex)
                    {
                        HandleException?.BeginInvoke(ex, null, null);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.BeginInvoke(ex, null, null);
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bytes">数据字节</param>
        public void Send(byte[] bytes)
        {
            try
            {
                _socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = _socket.EndSend(asyncResult);
                        HandleSendMsg?.BeginInvoke(this, bytes, null, null);
                    }
                    catch (Exception ex)
                    {
                        HandleException?.BeginInvoke(ex, null, null);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.BeginInvoke(ex, null, null);
            }
        }

        /// <summary>
        /// 发送字符串（默认使用UTF-8编码）
        /// </summary>
        /// <param name="msgStr">字符串</param>
        public void Send(string msgStr)
        {
            Send(Encoding.UTF8.GetBytes(msgStr));
        }

        /// <summary>
        /// 发送字符串（使用自定义编码）
        /// </summary>
        /// <param name="msgStr">字符串消息</param>
        /// <param name="encoding">使用的编码</param>
        public void Send(string msgStr, Encoding encoding)
        {
            Send(encoding.GetBytes(msgStr));
        }

        /// <summary>
        /// 传入自定义属性
        /// </summary>
        public object Property { get; set; }

        /// <summary>
        /// 关闭与服务器的连接
        /// </summary>
        public void Close()
        {
            try
            {
                _isRec = false;
                _socket.BeginDisconnect(false, asyncCallback =>
                {
                    try
                    {
                        _socket.EndDisconnect(asyncCallback);
                    }
                    catch (Exception ex)
                    {
                        HandleException?.BeginInvoke(ex, null, null);
                    }
                    finally
                    {
                        _socket.Dispose();
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.BeginInvoke(ex, null, null);
            }
            finally
            {
                HandleClientClose?.BeginInvoke(this, null, null);
            }
        }

        /// <summary>
        /// 判断是否处于已连接状态
        /// </summary>
        /// <returns></returns>
        public bool IsSocketConnected()
        {
            return !((_socket.Poll(1000, SelectMode.SelectRead) && (_socket.Available == 0)) || !_socket.Connected);
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 客户端连接建立后回调
        /// </summary>
        public Action<TcpSocketClient> HandleClientStarted { get; set; }

        /// <summary>
        /// 处理接受消息的委托
        /// </summary>
        public Action<TcpSocketClient, byte[]> HandleRecMsg { get; set; }

        /// <summary>
        /// 客户端连接发送消息后回调
        /// </summary>
        public Action<TcpSocketClient, byte[]> HandleSendMsg { get; set; }

        /// <summary>
        /// 客户端连接关闭后回调
        /// </summary>
        public Action<TcpSocketClient> HandleClientClose { get; set; }

        /// <summary>
        /// 异常处理程序
        /// </summary>
        public Action<Exception> HandleException { get; set; }

        #endregion
    }
}
