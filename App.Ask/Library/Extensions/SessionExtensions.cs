using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Utils;
using Microsoft.AspNetCore.Http;

namespace App.Ask.Library.Extensions
{
    /// <summary>
    /// ISession扩展类
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// 设置对象
        /// </summary>
        public static void SetObject<T>(this ISession session, string key, T value) where T : class, new()
        {
            session.SetString(key, JsonHelper.ToJson<T>(value));
        }
        /// <summary>
        /// 获取对象，，不存在则返回null
        /// </summary>
        public static T GetObject<T>(this ISession session, string key) where T : class, new()
        {
            string str = session.GetString(key);
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            else
            {
                return JsonHelper.FromJson<T>(str);
            }
        }

        /// <summary>
        /// 设置boolean值
        /// </summary>
        public static void SetBoolean(this ISession session, string key, bool value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 获取boolean值，不存在则返回null
        /// </summary>
        public static bool? GetBoolean(this ISession session, string key)
        {
            byte[] numArray = session.Get(key);
            if (numArray == null)
            {
                return null;
            }
            else
            {
                return BitConverter.ToBoolean(numArray, 0);
            }
        }
        /// <summary>
        /// 设置double值
        /// </summary>
        public static void SetDouble(this ISession session, string key, double value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// 获取double值，不存在则返回null
        /// </summary>
        public static double? GetDouble(this ISession session, string key)
        {
            byte[] numArray = session.Get(key);
            if (numArray == null)
            {
                return null;
            }
            else
            {
                return BitConverter.ToDouble(numArray, 0);
            }
        }
    }
}
