using Coldairarrow.Entity.Base_SysManage;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coldairarrow.Business.Base_SysManage
{
    public class Base_SysLogBusiness : BaseBusiness<Base_SysLog>
    {
        #region 外部接口

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="logType">日志类型</param>
        /// <param name="opUserName">操作人用户名</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public List<Base_SysLog> GetLogList(
            string logContent,
            string logType,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime,
            Pagination pagination)
        {
            var whereExp = LinqHelper.True<Base_SysLog>();
            if (!logContent.IsNullOrEmpty())
                whereExp = whereExp.And(x => x.LogContent.Contains(logContent));
            if (!logType.IsNullOrEmpty())
                whereExp = whereExp.And(x => x.LogType == logType);
            if (!opUserName.IsNullOrEmpty())
                whereExp = whereExp.And(x => x.OpUserName.Contains(opUserName));
            if (!startTime.IsNullOrEmpty())
                whereExp = whereExp.And(x => x.OpTime >= startTime);
            if (!endTime.IsNullOrEmpty())
                whereExp = whereExp.And(x => x.OpTime <= endTime);

            return GetIQueryable().Where(whereExp).GetPagination(pagination).ToList();
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}