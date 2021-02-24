using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Paging;

namespace App.Ask.Models
{
    public class TagDetailsModel
    {
        public Tag Tag { get; set; }
        public PagingResult<PostView> PagingResult { get; set; }

    }
}
