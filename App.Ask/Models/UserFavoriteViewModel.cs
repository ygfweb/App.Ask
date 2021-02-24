using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Paging;

namespace App.Ask.Models
{
    /// <summary>
    /// 用户收藏视图模型
    /// </summary>
    public class UserFavoriteViewModel
    {
        public PagingResult<PostView> PagingResult { get; }
        public PersonView PersonView { get; }

        public UserFavoriteViewModel(PagingResult<PostView> pagingResult, PersonView personView)
        {
            PagingResult = pagingResult ?? throw new ArgumentNullException(nameof(pagingResult));
            PersonView = personView ?? throw new ArgumentNullException(nameof(personView));
        }
    }
}