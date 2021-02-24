using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Library.Utils
{
    /// <summary>
    /// HTML响应结果
    /// </summary>
    public class HtmlResult : ContentResult
    {
        /// <summary>
        /// HTML响应结果
        /// </summary>
        /// <param name="html">Html文本</param>
        /// <param name="statusCode">响应代码</param>
        public HtmlResult(string html, int statusCode = 200)
        {
            this.Content = html;
            this.ContentType = "text/html";
            this.StatusCode = statusCode;
        }
    }
}
