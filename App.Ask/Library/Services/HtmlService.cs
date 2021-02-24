using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Ganss.XSS;

namespace App.Ask.Library.Services
{
    /// <summary>
    /// HTML解析服务
    /// </summary>
    public class HtmlService
    {
        private HtmlSanitizer HtmlSanitizer { get; }

        public HtmlService()
        {
            List<string> allowedTags = new List<string>();
            allowedTags.AddRange(new string[] { "div", "span", "a" });
            // 文本标签
            allowedTags.AddRange(new string[] { "p", "small", "del", "s", "ins", "u", "strong", "em", "bold", "italic", "mark", "abbr", "blockquote", "footer" });
            // 列表标签
            allowedTags.AddRange(new string[] { "ul", "li", "ol", "dl", "dt", "dd" });
            // 代码块
            allowedTags.AddRange(new string[] { "code", "pre", "kbd", "samp" });
            // 图片
            allowedTags.Add("img");
            // 表格
            allowedTags.AddRange(new string[] { "table", "thead", "tr", "td", "th", "tbody" });
            this.HtmlSanitizer = new HtmlSanitizer(allowedTags: allowedTags);
            this.HtmlSanitizer.AllowedAttributes.Add("class");//标签属性白名单,默认没有class标签属性  
            this.HtmlSanitizer.AllowedAttributes.Remove("style");
            this.HtmlSanitizer.AllowedAttributes.Remove("height");
            this.HtmlSanitizer.AllowedAttributes.Remove("width");
            this.HtmlSanitizer.RemovingTag += HtmlSanitizer_RemovingTag;
        }

        private void HtmlSanitizer_RemovingTag(object sender, RemovingTagEventArgs e)
        {
            if (e.Tag.NodeName.Equals("IFRAME", StringComparison.OrdinalIgnoreCase))
            {
                string src = e.Tag.GetAttribute("src");
                if (!string.IsNullOrEmpty(src) && src.StartsWith("https://www.youtube.com/"))
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// HTML内容转纯文本（即去掉所有HTML标签）
        /// </summary>
        public string HtmlToText(string html)
        {
            var parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(html);
            return document.Body.Text();
        }

        /// <summary>
        /// 清理HTML内容
        /// </summary>
        public string ClearHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return "";
            }
            else
            {
                return this.HtmlSanitizer.Sanitize(html);
            }
        }
    }
}
