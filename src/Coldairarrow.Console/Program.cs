using System;
using Coldairarrow.Util.Sockets;
using Coldairarrow.Util;
using System.Collections.Generic;

namespace Coldairarrow.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var type = TypeBuilderHelper.BuildType("Test", "zxzx", new List<PropertyConfig> { new PropertyConfig { PropertyName = "Name", PropertyType = typeof(string) } });
            //var property = type.GetProperty("Name");
            //var obj = (ITest)Activator.CreateInstance(type);
            //obj.SetPropertyValue("Name", "小明");
            var type = typeof(ITest);
            var method = type.GetMethod("Hello").GetParameters();

            Console.WriteLine("完成");
            Console.ReadLine();
        }
    }
}
