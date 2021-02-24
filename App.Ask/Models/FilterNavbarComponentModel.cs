using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using App.Ask.Library.Enums;
using App.Ask.Library.Utils;
using SiHan.Libs.Utils.Reflection;
using Microsoft.AspNetCore.Http;

namespace App.Ask.Models
{
    /// <summary>
    /// 过滤导航组件模型
    /// </summary>
    public class FilterNavbarComponentModel
    {
        public FilterType Current { get; set; }
        public Dictionary<FilterType, string> Data { get; set; } = new Dictionary<FilterType, string>();
        public HttpContext HttpContext { get; set; }
        private const string KeyName = "filter";

        public FilterNavbarComponentModel(HttpContext httpContext)
        {
            HttpContext = httpContext;
            this.Data = EnumHelper<FilterType>.ToDictionary();
            QueryWrapper query = new QueryWrapper(this.HttpContext);
            string value = query.GetValue(KeyName);
            if (!string.IsNullOrWhiteSpace(value))
            {
                Current = (FilterType)Convert.ToInt32(value);
            }
            else
            {
                Current = FilterType.New;
            }
        }

        /// <summary>
        /// 获取URL
        /// </summary>
        public string GetUrl(FilterType filterType)
        {
            QueryWrapper query = new QueryWrapper(this.HttpContext);
            if (filterType == FilterType.New)
            {
                return query.RemoveValue(KeyName);
            }
            else
            {
                int i = (int)filterType;
                return query.SetValue(KeyName, i.ToString());
            }
        }

        /// <summary>
        /// 获取CSS样式
        /// </summary>
        public string GetCss(FilterType filter)
        {
            if (filter == Current)
            {
                return "nav-link active";
            }
            else
            {
                return "nav-link";
            }
        }
    }
}
