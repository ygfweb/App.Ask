using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Utils.Text;

namespace App.Ask.Models
{
    public class UserInfoModel
    {
        public PersonView PersonView { get; set; }
        public bool IsStickyTop { get; set; } = false;

        public string GetRootClasses()
        {
            if (IsStickyTop)
            {
                return "card shadow-sm";
            }
            else
            {
                return "card shadow-sm";
            }
        }

        public string GetCreateTimeString()
        {
            if (PersonView == null)
            {
                return "";
            }
            else
            {
                return FormatHelper.ToDate(this.PersonView.CreateTime);
            }
        }

        public string GetSignature()
        {
            if (PersonView == null || string.IsNullOrWhiteSpace(PersonView.Signature))
            {
                return "暂无";
            }
            else
            {
                return PersonView.Signature.Trim();
            }
        }

        public string GetAskNum()
        {
            if (PersonView == null)
            {
                return "0";
            }
            else
            {
                return FormatHelper.FormatNumber(this.PersonView.AskNum);
            }
        }

        public string GetArticleNum()
        {
            if (PersonView == null)
            {
                return "0";
            }
            else
            {
                return FormatHelper.FormatNumber(this.PersonView.ArticleNum);
            }
        }
        public string GetLikeNum()
        {
            if (PersonView == null)
            {
                return "0";
            }
            else
            {
                return FormatHelper.FormatNumber(this.PersonView.LikeNum);
            }
        }
    }
}
