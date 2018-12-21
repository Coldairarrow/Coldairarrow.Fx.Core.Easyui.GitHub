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
            int port = 11111;
            int count = 1;

            RPCServer rPCServer = new RPCServer(port);
            rPCServer.RegisterService<IHello, Hello>();
            rPCServer.Start();
            Stopwatch watch = new Stopwatch();

            var client = RPCClientFactory.GetClient<IHello>("127.0.0.1", port);
            client.SayHello("Hello");

            watch.Start();
            LoopHelper.Loop(count, () =>
            {
                var res = client.SayHello("Hello");
                //Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:fff")}客户端:{res}");
            });
            watch.Stop();

            Console.WriteLine($"耗时:{(double)watch.ElapsedMilliseconds/count}ms");

            Console.ReadLine();
        }
    }
}
