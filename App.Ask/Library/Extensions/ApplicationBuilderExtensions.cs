using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;

namespace App.Ask.Library.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 静态文件缓存，默认24小时
        /// </summary>
        public static void UseStaticFilesWithCache(this IApplicationBuilder app, int cacheSeconds = 60 * 60 * 24)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = delegate (StaticFileResponseContext ctx)
                {
                    ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=" + cacheSeconds.ToString();
                }
            });
        }

        public static void UseSiteSession(this IApplicationBuilder app)
        {
            app.UseSession();
            app.Use(async (context, next) =>
            {
                context.Session.SetBoolean("init", true);
                await next();
            });
        }
    }
}