using System;
using Coldairarrow.Util.Sockets;
using Coldairarrow.Util;
using System.Collections.Generic;
using Coldairarrow.Util.RPC;
using System.Diagnostics;
using Coldairarrow.Entity.Base_SysManage;
using System.Text;

namespace Coldairarrow.ConsoleApp
{
    public interface IHello
    {
        string SayHello(string msg);
    }
    public class Hello : IHello
    {
        public string SayHello(string msg)
        {
            return msg;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            RPCServer rPCServer = new RPCServer(9000);
            rPCServer.RegisterService<IHello, Hello>();
            rPCServer.Start();
            Stopwatch watch = new Stopwatch();

            var client = RPCClientFactory.GetClient<IHello>("127.0.0.1", 9000);
            client.SayHello("Hello");

            watch.Start();
            var res = client.SayHello("Hello");
            watch.Stop();
            Console.WriteLine($"客户端:{res}");

            Console.WriteLine($"耗时:{watch.ElapsedMilliseconds}ms");

            Console.ReadLine();
        }
    }
}
