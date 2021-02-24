using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Paging;

namespace App.Ask.Models
{
    public class UserQuestionViewModel
    {
        public PagingResult<PostView> PagingResult { get; }
        public PersonView PersonView { get; }

        public UserQuestionViewModel(PagingResult<PostView> pagingResult, PersonView personView)
        {
            PagingResult = pagingResult ?? throw new ArgumentNullException(nameof(pagingResult));
            PersonView = personView ?? throw new ArgumentNullException(nameof(personView));
        }
    }
}
