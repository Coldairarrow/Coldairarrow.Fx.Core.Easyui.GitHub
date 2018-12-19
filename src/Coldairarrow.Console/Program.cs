using System;
using Coldairarrow.Util.Sockets;
using Coldairarrow.Util;
using System.Collections.Generic;
using Coldairarrow.Util.RPC;
using System.Diagnostics;
using Coldairarrow.Entity.Base_SysManage;
using System.Text;
using System.Threading.Tasks;
using Coldairarrow.Business.Base_SysManage;

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
            int port = 9999;
            int count = 10000*10;
            RPCServer rPCServer = new RPCServer(port);
            rPCServer.HandleException = ex =>
            {
                Console.WriteLine(ExceptionHelper.GetExceptionAllMsg(ex));
            };
            rPCServer.RegisterService<IHello, Hello>();
            rPCServer.Start();
            IHello client = null;
            client = RPCClientFactory.GetClient<IHello>("127.0.0.1", port);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<Task> tasks = new List<Task>();
            LoopHelper.Loop(10, () =>
            {
                tasks.Add( Task.Run(() =>
                {
                    LoopHelper.Loop(count, () =>
                    {
                        string msg = client.SayHello("Hello");
                        //Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}:{msg}");
                    });
                }));
            });
            watch.Stop();
            Task.WaitAll(tasks.ToArray());
            //Console.WriteLine($"每次耗时:{(double)watch.ElapsedMilliseconds / count}ms");


            Console.ReadLine();
        }
    }
}
