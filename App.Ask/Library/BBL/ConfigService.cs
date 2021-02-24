using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.DAL;
using App.Ask.Library.Setting;

namespace App.Ask.Library.BBL
{
    /// <summary>
    /// 站点配置服务
    /// </summary>
    public class ConfigService
    {
        private DbFactory DbFactory { get; set; }


        public ConfigService(DbFactory dbFactory)
        {
            DbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        /// <summary>
        /// 获取首页选项
        /// </summary>
        public async Task<HomeConfig> GetHomeConfigAsync()
        {
            using (var work = this.DbFactory.StartWork())
            {
                return await work.Config.GetHomeConfigAsync();
            }
        }
        /// <summary>
        /// 设置首页选项
        /// </summary>
        public async Task<int> SetHomeConfigAsync(HomeConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            using (var work = this.DbFactory.StartWork())
            {
                return await work.Config.SetHomeConfigAsync(config);
            }
        }

        public async Task<int> SetConfigAsync<T>(T config) where T : class, new()
        {
            using (var work = this.DbFactory.StartWork())
            {
                return await work.Config.SetConfigAsync<T>(config);
            }
        }

        public async Task<T> GetConfigAsync<T>() where T : class, new()
        {
            using (var work = this.DbFactory.StartWork())
            {
                return await work.Config.GetConfigAsync<T>();
            }
        }
    }
}
