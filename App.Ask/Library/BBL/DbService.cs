using System;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using App.Ask.Library.Services;
using App.Ask.Library.Setting;
using App.Ask.Library.Utils;
using SiHan.Libs.Ado;
using SiHan.Libs.Utils.IO;
using SiHan.Libs.Utils.Text;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace App.Ask.Library.BBL
{
    /// <summary>
    /// 网站数据服务
    /// </summary>
    public class DbService
    {
        private readonly DbFactory dbFactory;
        private readonly ILogger<DbService> logger;
        private readonly EncryptService encryptService;
        public DbService(DbFactory factory, ILogger<DbService> logger, EncryptService encrypt)
        {
            this.dbFactory = factory;
            this.logger = logger;
            this.encryptService = encrypt;
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public async Task InitAsync()
        {
            using (DbConnection conn = this.dbFactory.Create())
            {
                conn.Open();
                long count = await conn.ScalarAsync<long>("select count(*) from pg_tables where schemaname='public';");
                if (count > 0)
                {
                    this.logger.LogInformation("The data table already exists in the database, the initialization process will be skipped.");
                    // 如果数据库存在数据表，则不跳过初始化
                    return;
                }
                else
                {
                    // 创建数据表
                    this.logger.LogInformation("Start creating data table");
                    await conn.ExecuteNonQueryAsync(App.Ask.Properties.Resources.CreateTables);
                    await conn.ExecuteNonQueryAsync(Properties.Resources.createIndex);
                    // 创建角色
                    Role superAdminRole = new Role() { Id = GuidHelper.CreateSequential(), Name = "网站管理员", RoleType = RoleType.SuperAdmin };
                    Role adminRole = new Role() { Id = GuidHelper.CreateSequential(), Name = "管理员", RoleType = RoleType.Admin };
                    Role masterRole = new Role() { Id = GuidHelper.CreateSequential(), Name = "话题版主", RoleType = RoleType.Master };
                    Role userRole = new Role() { Id = GuidHelper.CreateSequential(), Name = "注册用户", RoleType = RoleType.User };
                    DateTime now = DateTime.Now;
                    Person superAdmin = new Person()
                    {
                        Id = GuidHelper.CreateSequential(),
                        AccountName = "admin",
                        RoleId = superAdminRole.Id,
                        NickName = "admin",
                        Password = this.encryptService.PasswordHash("12345678"),
                        LastUpdated = now,
                        CreateTime = now
                    };
                    PersonData adminData = new PersonData
                    {
                        Id = GuidHelper.CreateSequential(),
                        AnswerNum = 0,
                        ArticleNum = 0,
                        AskNum = 0,
                        LikeNum = 0,
                        PersonId = superAdmin.Id
                    };
                    Topic topic = new Topic
                    {
                        Id = GuidHelper.CreateSequential(),
                        OrderNum = 0,
                        IsAnnounce = true,
                        IsHide = false,
                        Name = "公告"
                    };

                    DbConfig dbConfig = new DbConfig { Version = new Version(1, 0, 0, 0) };
                    Config config = new Config
                    {
                        Id = GuidHelper.CreateSequential(),
                        ConfigKey = nameof(DbConfig),
                        ConfigValue = JsonHelper.ToJson<DbConfig>(dbConfig)
                    };
                    using (DbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            this.logger.LogInformation("Start initializing data");
                            await conn.InsertAsync<Role>(superAdminRole, trans);
                            await conn.InsertAsync(adminRole, trans);
                            await conn.InsertAsync(masterRole, trans);
                            await conn.InsertAsync(userRole, trans);
                            await conn.InsertAsync(superAdmin, trans);
                            await conn.InsertAsync(adminData, trans);
                            await conn.InsertAsync(topic, trans);
                            await conn.InsertAsync(config, trans);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Initialization data error");
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
    }
}