using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Info
{
    public class PersonInfo
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; } = "";
    }
}
