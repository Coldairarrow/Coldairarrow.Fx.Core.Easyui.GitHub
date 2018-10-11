using Coldairarrow.Business.Base_SysManage;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Coldairarrow.ConsoleApp
{
    class Program
    {
        public static IQueryable<T> OrderBy<T>(IQueryable<T> source,params KeyValuePair<string,string>[] sort)
        {
            var parameter = Expression.Parameter(typeof(T), "o");

            sort.ForEach((aSort, index) =>
            {
                //根据属性名获取属性
                var property = typeof(T).GetProperty(aSort.Key);
                //创建一个访问属性的表达式
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);

                string OrderName = "";
                if (index > 0)
                {
                    OrderName = aSort.Value.ToLower() == "desc" ? "ThenByDescending" : "ThenBy";
                }
                else
                    OrderName = aSort.Value.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

                MethodCallExpression resultExp = Expression.Call(typeof(Queryable), OrderName, new Type[] { typeof(T), property.PropertyType }, source.Expression, Expression.Quote(orderByExp));

                source = source.Provider.CreateQuery<T>(resultExp);
            });

            return source;
        }
        static void Main(string[] args)
        {
            Base_UserBusiness bus = new Base_UserBusiness();
            bus.Service.HandleSqlLog = log =>
            {
                Console.WriteLine(log);
            };
            bus.GetDataList(null, null, new Pagination());
            //bus.GetDataList(null, null, new Pagination());

            //bus.GetIQueryable().OrderBy(new KeyValuePair<string,string>("Id","asc")).Skip(0).Take(30).ToList();
            //bus.GetIQueryable().OrderBy(new KeyValuePair<string, string>("Id", "asc")).Skip(0).Take(30).ToList();

            Console.WriteLine("完成");
            Console.ReadLine();
        }
    }
}
