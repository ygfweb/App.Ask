using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;

namespace App.Ask.Models
{
    public class NewBestPostComponentModel
    {
        public List<PostView> Posts { get; set; } = new List<PostView>();
    }
}
