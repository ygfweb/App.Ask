using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Enums;
using SiHan.Libs.Ado;

namespace App.Ask.Library.Utils
{
    public class PostStatusConvert : BaseValueConvert
    {
        public override object Read(object dbValue)
        {
            return (PostStatus)dbValue;
        }

        public override object Write(object propertyValue)
        {
            return (int)propertyValue;
        }
    }
}
