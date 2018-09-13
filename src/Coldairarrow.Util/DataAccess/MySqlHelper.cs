using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace Coldairarrow.Util
{
    /// <summary>
    /// MySql数据库操作帮助类
    /// </summary>
    public class MySqlHelper : DbHelper
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nameOrConStr">数据库连接名或连接字符串</param>
        public MySqlHelper(string nameOrConStr)
            : base(DatabaseType.MySql, nameOrConStr)
        {
        }

        #endregion

        #region 私有成员

        protected override Dictionary<string, Type> DbTypeDic => throw new NotImplementedException();

        #endregion

        #region 外部接口

        /// <summary>
        /// 获取数据库中的所有表
        /// </summary>
        /// <param name="schemaName">模式（架构）</param>
        /// <returns></returns>
        public override List<DbTableInfo> GetDbAllTables(string schemaName = "public")
        {
            string sql = @"";
            return GetListBySql<DbTableInfo>(sql);
        }

        /// <summary>
        /// 通过连接字符串和表名获取数据库表的信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public override List<TableInfo> GetDbTableInfo(string tableName)
        {
            string sql = @"";
            return GetListBySql<TableInfo>(sql, new List<DbParameter> { new SqlParameter("@table_name", tableName) });
        }

        /// <summary>
        /// 生成实体文件
        /// </summary>
        /// <param name="infos">表字段信息</param>
        /// <param name="tableName">表名</param>
        /// <param name="tableDescription">表描述信息</param>
        /// <param name="filePath">文件路径（包含文件名）</param>
        /// <param name="nameSpace">实体命名空间</param>
        /// <param name="schemaName">架构（模式）名</param>
        public override void SaveEntityToFile(List<TableInfo> infos, string tableName, string tableDescription, string filePath, string nameSpace, string schemaName = null)
        {
            base.SaveEntityToFile(infos, tableName, tableDescription, filePath, nameSpace, schemaName);
        }

        #endregion
    }
}
