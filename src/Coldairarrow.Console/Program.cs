using Coldairarrow.Util;
using System;

namespace Coldairarrow.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbHelper = DbHelperFactory.GetDbHelper(DatabaseType.MySql, "server=127.0.0.1;user id=root;password=root;persistsecurityinfo=True;database=coldairarrow.fx.net.easyui.github");
            var list = dbHelper.GetDbAllTables();

            Console.WriteLine("Hello World!");
        }
    }
}
