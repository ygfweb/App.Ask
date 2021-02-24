using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;

namespace App.Ask.Models
{
    public class QuestionItemComponentModel
    {
        public PostView PostView { get; }

        public QuestionItemComponentModel(PostView postView)
        {
            PostView = postView ?? throw new ArgumentNullException(nameof(postView));
        }
    }
}
