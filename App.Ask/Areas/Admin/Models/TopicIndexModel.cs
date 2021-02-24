using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;

namespace App.Ask.Areas.Admin.Models
{
    public class TopicIndexModel
    {
        public List<Topic> Topics { get; set; } = new List<Topic>();
        public TopicEditModel InsertModel { get; set; } = new TopicEditModel();
        public TopicEditModel ModifyModel { get; set; } = new TopicEditModel();
    }
}
