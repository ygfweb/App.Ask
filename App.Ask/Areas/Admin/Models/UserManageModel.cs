using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using SiHan.Libs.Utils.Paging;
using SiHan.Libs.Utils.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Ask.Areas.Admin.Models
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserManageModel
    {
        public PagingResult<PersonView> PagingResult { get; set; }
        public string Search { get; set; }
        public RoleFilterType RoleFilter { get; set; }
        public List<SelectListItem> SelectListItems { get; }

        public UserManageModel(PagingResult<PersonView> pagingResult, string search, RoleFilterType roleFilter)
        {
            PagingResult = pagingResult;
            Search = search;
            RoleFilter = roleFilter;
            this.SelectListItems = EnumUitls.ToSelectListItems<RoleFilterType>(false);
        }
    }
}
