using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Ask.Library.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// 获取用户的Id
        /// </summary>
        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            Claim claim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return null;
            }
            else
            {
                return new Guid(claim.Value);
            }
        }

        /// <summary>
        /// 获取用户的账号名称
        /// </summary>
        public static string GetUserName(this ClaimsPrincipal user)
        {
            Claim claim = user.FindFirst(ClaimTypes.Name);
            if (claim == null)
            {
                return "";
            }
            else
            {
                return claim.Value;
            }
        }

        /// <summary>
        /// 获取用户的角色名称
        /// </summary>
        public static string GetUserRoleName(this ClaimsPrincipal user)
        {
            Claim claim = user.FindFirst(ClaimTypes.Role);
            if (claim == null)
            {
                return "";
            }
            else
            {
                return claim.Value;
            }
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        public static bool IsLogin(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }
    }
}
