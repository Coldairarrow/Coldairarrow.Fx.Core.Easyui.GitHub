using System;
using Coldairarrow.Util.Sockets;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Diagnostics;
using Coldairarrow.Entity.Base_SysManage;
using System.Text;
using System.Threading.Tasks;
using Coldairarrow.DotNettyRPC;

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
        static void DotNettyRPCTest()
        {
            int threadCount = 1;
            int port = 9999;
            int count = 10000;
            int errorCount = 0;
            RPCServer rPCServer = new RPCServer(port);
            rPCServer.RegisterService<IHello, Hello>();
            rPCServer.Start();
            IHello client = null;
            client = RPCClientFactory.GetClient<IHello>("127.0.0.1", port);
            client.SayHello("aaa");
            Stopwatch watch = new Stopwatch();
            List<Task> tasks = new List<Task>();
            watch.Start();
            LoopHelper.Loop(threadCount, () =>
            {
                tasks.Add(Task.Run(() =>
                {
                    LoopHelper.Loop(count, () =>
                    {
                        string msg = string.Empty;
                        try
                        {
                            msg = client.SayHello("Hello");
                            //Console.WriteLine($"{DateTime.Now.ToCstTime().ToString("yyyy-MM-dd HH:mm:ss:fff")}:{msg}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ExceptionHelper.GetExceptionAllMsg(ex));
                        }
                    });
                }));
            });
            Task.WaitAll(tasks.ToArray());
            watch.Stop();
            Console.WriteLine($"并发数:{threadCount},运行:{count}次,每次耗时:{(double)watch.ElapsedMilliseconds / count}ms");
            Console.WriteLine($"错误次数：{errorCount}");
        }
        static void Main(string[] args)
        {
            DotNettyRPCTest();

            Console.ReadLine();
        }
    }
}
