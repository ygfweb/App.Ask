using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Text;
using SiHan.Libs.Utils.Time;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace App.Ask.Library.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 设置验证码
        /// </summary>
        public static async Task SetVerifyCodeAsync(this ISession session, string code)
        {
            session.SetString("_VerifyCode", code);
            await session.CommitAsync();
        }
        /// <summary>
        /// 刷新验证码
        /// </summary>
        public static async Task RefreshVerifyCodeAsync(this ISession session)
        {
            await session.SetVerifyCodeAsync(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        public static string GetVerifyCode(this ISession session)
        {
            string code = session.GetString("_VerifyCode");
            if (string.IsNullOrWhiteSpace(code))
            {
                return "";
            }
            else
            {
                return code;
            }
        }


        /// <summary>
        /// 网站登录（用于cookie认证）
        /// </summary>
        public static async Task LoginAsync(this HttpContext context, Person person, bool isRememberMe = false)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,person.Id.ToString()),
                new Claim(ClaimTypes.Name,person.NickName),
                new Claim(nameof(person.LastUpdated),FormatHelper.ToTime(person.LastUpdated))
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            if (isRememberMe)
            {
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                });
            }
            else
            {
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
                {
                    IsPersistent = false
                });
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public static async Task LogoutAsync(this HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
