using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using App.Ask.Library.Auth;
using App.Ask.Library.BBL;
using App.Ask.Library.DAL;
using App.Ask.Library.Services;
using App.Ask.Library.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.WebEncoders;
using Newtonsoft.Json.Serialization;

namespace App.Ask.Library.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加cookie认证
        /// </summary>
        public static void AddCookieAuth(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
             {
                 options.Cookie.Name = "app.AuthCookie";
                 options.Cookie.HttpOnly = true;
                 options.EventsType = typeof(CustomCookieAuthenticationEvents);
             });
            services.AddAntiforgery(options => options.Cookie.Name = "app.CSRF");
            services.AddScoped<CustomCookieAuthenticationEvents>();
        }

        /// <summary>
        /// 增加Session服务
        /// </summary>
        public static void AddSiteSession(this IServiceCollection services, int IdleTimeout = 60 * 30)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false; //关闭GDPR规范    
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDistributedMemoryCache(); // 使用内存作为session的缓存
            services.AddSession(options =>
            {
                options.Cookie.Name = "app.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(IdleTimeout);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; //强制存储cookie
            });
        }
        public static void AddSiteMvc(this IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(setupAction =>
            {
                setupAction.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).AddMvcOptions(o =>
            {
                // 模型绑定字符串默认为null，改为默认空字符
                o.ModelMetadataDetailsProviders.Add(new BlankMetadataProvider());
            }).AddSessionStateTempDataProvider();
            services.Configure(delegate (WebEncoderOptions options)
            {
                // 解决中文Title问题
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs);
            });
        }

        /// <summary>
        /// 添加网站服务
        /// </summary>
        public static void AddSiteService(this IServiceCollection services)
        {
            services.AddScoped<EncryptService>();
            services.AddScoped<HtmlService>();
            services.AddScoped<DbFactory>();
            services.AddScoped<DbService>();
            services.AddScoped<PersonService>();
            services.AddScoped<ConfigService>();
            services.AddScoped<TopicService>();
            services.AddScoped<PostService>();
            services.AddScoped<TagService>();
            services.AddScoped<UploadService>();
            services.AddScoped<CommentService>();
            services.AddScoped<ZanService>();
            services.AddScoped<FavoriteService>();
            services.AddScoped<StatisticsService>();
            services.AddScoped<ActivityService>();
            services.AddScoped<InviteService>();
        }
    }
}