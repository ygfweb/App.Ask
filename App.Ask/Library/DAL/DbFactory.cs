using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace App.Ask.Library.DAL
{
    /// <summary>
    /// 数据工厂
    /// </summary>
    public class DbFactory
    {
        private readonly IConfiguration Configuration;
        private readonly string ConnString;

        public DbFactory(IConfiguration configuration)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            // 从appsettings配置文件中获取连接字符串
            this.ConnString = this.Configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// 创建新的数据库链接
        /// </summary>
        public DbConnection Create()
        {
            return new NpgsqlConnection(this.ConnString);
        }

        /// <summary>
        /// 启动一个新的工作单元
        /// </summary>
        public UnitOfWork StartWork()
        {
            NpgsqlConnection connection = new NpgsqlConnection(this.ConnString);
            connection.Open();
            connection.ReloadTypes();
            return new UnitOfWork(connection);
        }
    }
}
