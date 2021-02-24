using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Library.Info;
using App.Ask.Library.Setting;
using App.Ask.Library.Utils;
using SiHan.Libs.Ado;
using SiHan.Libs.Utils.Text;

namespace App.Ask.Library.DAL
{
    /// <summary>
    /// 网站配置存储库
    /// </summary>
    public class ConfigRepository : BaseRepository<Config>
    {
        private const string DBVERSION = "dbVersion";
        public ConfigRepository(DbConnection connection) : base(connection) { }

        /// <summary>
        /// 通过名称获取配置
        /// </summary>
        public async Task<Config> GetConfigFormKeyAsync(string key)
        {
            return await this.DbConnection.FirstOrDefaultAsync<Config>("SELECT * FROM config WHERE config_key = @key;", new { key = key });
        }

        /// <summary>
        /// 获取数据库版本
        /// </summary>
        public async Task<Version> GetDbVersionAsync()
        {
            Config config = await this.GetConfigFormKeyAsync(DBVERSION);
            if (config == null)
            {
                return new Version(1, 0, 0, 0);
            }
            else
            {
                return new Version(config.ConfigValue);
            }
        }
        /// <summary>
        /// 设置数据库版本
        /// </summary>
        public async Task<int> SetDbVersionAsync(Version version, DbTransaction transaction = null)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }
            Config config = new Config
            {
                Id = GuidHelper.CreateSequential(),
                ConfigKey = DBVERSION,
                ConfigValue = version.ToString()
            };
            return await this.InsertAsync(config, transaction);
        }

        /// <summary>
        /// 设置配置项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<int> SetConfigAsync<T>(T obj, DbTransaction transaction = null) where T : class, new()
        {
            string keyName = typeof(T).Name;
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            string json = JsonHelper.ToJson<T>(obj);
            Config config = await this.GetConfigFormKeyAsync(keyName);
            if (config == null)
            {
                config = new Config
                {
                    Id = GuidHelper.CreateSequential(),
                    ConfigKey = keyName,
                    ConfigValue = json
                };
                return await this.InsertAsync(config, transaction);
            }
            else
            {
                config.ConfigValue = json;
                return await this.UpdateAsync(config, transaction);
            }
        }
        /// <summary>
        /// 获取配置项
        /// </summary>
        public async Task<T> GetConfigAsync<T>() where T : class, new()
        {
            string keyName = typeof(T).Name;
            Config config = await this.GetConfigFormKeyAsync(keyName);
            if (config == null || string.IsNullOrWhiteSpace(config.ConfigValue))
            {
                return new T();
            }
            else
            {
                return JsonHelper.FromJson<T>(config.ConfigValue);
            }
        }

        /// <summary>
        /// 获取首页配置
        /// </summary>
        public async Task<HomeConfig> GetHomeConfigAsync()
        {
            return await this.GetConfigAsync<HomeConfig>();
        }

        /// <summary>
        /// 设置首页配置
        /// </summary>
        public async Task<int> SetHomeConfigAsync(HomeConfig homeConfig, DbTransaction transaction = null)
        {
            return await this.SetConfigAsync<HomeConfig>(homeConfig, transaction);
        }

        /// <summary>
        /// 设置注册选项
        /// </summary>
        public async Task<int> SetRegisterConfigAsync(RegisterConfig register, DbTransaction transaction = null)
        {
            return await this.SetConfigAsync<RegisterConfig>(register, transaction);
        }

        /// <summary>
        /// 获取注册选项
        /// </summary>
        public async Task<RegisterConfig> GetRegisterConfigAsync()
        {
            return await this.GetConfigAsync<RegisterConfig>();
        }

        /// <summary>
        /// 设置页面配置
        /// </summary>
        public async Task<int> SetPageConfigAsync(PageConfig config, DbTransaction transaction = null)
        {
            return await this.SetConfigAsync<PageConfig>(config, transaction);
        }
        /// <summary>
        /// 获取页面选项
        /// </summary>
        public async Task<PageConfig> GetPageConfigAsync()
        {
            return await this.GetConfigAsync<PageConfig>();
        }


    }
}
