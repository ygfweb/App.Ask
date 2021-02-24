using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Utils;
using SiHan.Libs.Utils.Paging;
using Microsoft.AspNetCore.Http;

namespace App.Ask.Models
{
    /// <summary>
    /// bootstrap 分页视图组件
    /// </summary>
    public class BootstrapPagerModel
    {
        /// <summary>
        /// 每页尺寸
        /// </summary>
        public int PageSize { get; } = 10;
        /// <summary>
        /// 总行数
        /// </summary>
        public long RowCount { get; } = 0;

        public HttpContext HttpContext { get; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; } = 1;

        public Pager Pager { get; }

        public BootstrapPagerModel(int pageSize, long rowCount, HttpContext httpContext)
        {
            PageSize = pageSize;
            RowCount = rowCount;
            HttpContext = httpContext;
            QueryWrapper query = new QueryWrapper(httpContext);
            string page = query.GetValue("page");
            if (string.IsNullOrWhiteSpace(page))
            {
                this.CurrentPage = 1;
            }
            else
            {
                this.CurrentPage = Convert.ToInt32(page);
            }
            this.Pager = new Pager(rowCount, this.CurrentPage, pageSize, 5);
        }

        /// <summary>
        /// 获取首页CSS
        /// </summary>
        public string getFirstCss()
        {
            if (CurrentPage > 1)
            {
                return "page-item";
            }
            else
            {
                return "page-item disabled";
            }
        }

        public string GetFirstUrl()
        {
            if (CurrentPage > 1)
            {
                return this.GetUrl(1);
            }
            else
            {
                return "#";
            }
        }

        /// <summary>
        /// 获取上一页CSS
        /// </summary>
        public string getPreviousCss()
        {
            if (Pager.HasPreviousPage)
            {
                return "page-item";
            }
            else
            {
                return "page-item disabled";
            }
        }

        public string getPreviousUrl()
        {
            if (Pager.HasPreviousPage)
            {
                return this.GetUrl(Pager.PreviousPage);
            }
            else
            {
                return "#";
            }
        }

        /// <summary>
        /// 获取普通页码的css
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public string GetCss(int page)
        {
            if (page == CurrentPage)
            {
                return "page-item active";
            }
            else
            {
                return "page-item d-none d-md-block";
            }
        }

        public string GetNextCss()
        {
            if (Pager.HasNextPage)
            {
                return "page-item";
            }
            else
            {
                return "page-item disabled";
            }
        }

        public string GetNextUrl()
        {
            if (Pager.HasNextPage)
            {
                return GetUrl(Pager.NextPage);
            }
            else
            {
                return "#";
            }
        }

        public string GetLastCss()
        {
            if (CurrentPage == Pager.TotalPages)
            {
                return "page-item disabled";
            }
            else
            {
                return "page-item";
            }
        }

        public string GetLastUrl()
        {
            if (CurrentPage == Pager.TotalPages)
            {
                return "#";
            }
            else
            {
                return GetUrl(Pager.TotalPages);
            }
        }

        public string GetUrl(int page)
        {
            QueryWrapper query = new QueryWrapper(this.HttpContext);
            return query.SetValue("page", page.ToString());
        }


    }
}
