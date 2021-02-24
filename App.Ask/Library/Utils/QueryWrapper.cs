using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace App.Ask.Library.Utils
{
    /// <summary>
    /// 查询字符串包装器
    /// </summary>
    public class QueryWrapper
    {
        private readonly HttpContext HttpContext;

        public QueryWrapper(HttpContext httpContext)
        {
            HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        }

        /// <summary>
        /// 获取当前访问的URL
        /// </summary>
        public string GetCurrentUrl()
        {
            return this.HttpContext.Request.GetDisplayUrl();
        }

        /// <summary>
        /// 获取URL中查询字符串的值
        /// </summary>
        public string GetValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            key = key.ToLower();
            var query = this.HttpContext.Request.Query;
            if (query.ContainsKey(key))
            {
                return query[key].ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 设置查询字符串，返回一个新的URL
        /// </summary>
        public string SetValue(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            key = key.ToLower();
            Uri uri = new Uri(this.GetCurrentUrl());
            var baseUri = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);
            var query = QueryHelpers.ParseQuery(uri.Query);
            var items = query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();
            items.RemoveAll(x => x.Key == key);
            var qb = new QueryBuilder(items);
            qb.Add(key, value);
            var fullUri = baseUri + qb.ToQueryString();
            return fullUri;
        }

        /// <summary>
        /// 从URL中移除指定查询，返回一个新的URL
        /// </summary>
        public string RemoveValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            key = key.ToLower();
            Uri uri = new Uri(this.GetCurrentUrl());
            var baseUri = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);
            var query = QueryHelpers.ParseQuery(uri.Query);
            var items = query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();
            items.RemoveAll(x => x.Key == key);
            var qb = new QueryBuilder(items);
            var fullUri = baseUri + qb.ToQueryString();
            return fullUri;
        }
    }
}
