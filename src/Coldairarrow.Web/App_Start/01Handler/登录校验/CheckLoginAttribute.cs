using Coldairarrow.Business.Common;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;

namespace Coldairarrow.Web
{
    /// <summary>
    /// 校验登录
    /// </summary>
    public class CheckLoginAttribute :Attribute, IActionFilter
    {
        /// <summary>
        /// Action执行之前执行
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //若为本地测试，则不需要登录
            if (GlobalSwitch.RunModel == RunModel.LocalTest)
            {
                return;
            }
            //判断是否需要登录
            List<string> attrList = FilterHelper.GetFilterList(filterContext);
            bool needLogin = attrList.Contains(typeof(CheckLoginAttribute).FullName) && !attrList.Contains(typeof(IgnoreLoginAttribute).FullName);

            //转到登录
            if (needLogin && !Operator.Logged())
            {
                UrlHelper urlHelper = new UrlHelper(filterContext);
                string loginUrl = urlHelper.Content("~/Home/Login");
                string script =$@"    
<html>
    <script>
        top.location.href = '{loginUrl}';
    </script>
</html>
";
                filterContext.Result = new ContentResult { Content = script,ContentType= "text/html;charset=utf-8" };
            }
            else
                return;
        }

        /// <summary>
        /// Action执行完毕之后执行
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}