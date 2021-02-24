using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Ask.Library.Utils
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 序列化到JSON字符串
        /// </summary>
        public static string ToJson<T>(T obj) where T : class, new()
        {
            if (obj == null)
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(obj);
            }
        }

        /// <summary>
        /// 从JSON反序列化到对象
        /// </summary>
        public static T FromJson<T>(string json) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}
