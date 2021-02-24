using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Utils.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Ask.Library.Enums
{
    public static class EnumUitls
    {
        /// <summary>
        /// 将枚举类型转换为SelectListItem列表
        /// </summary>
        public static List<SelectListItem> ToSelectListItems<T>(bool isCanNull = false) where T : Enum
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            if (isCanNull)
            {
                selectListItems.Add(new SelectListItem("请选择...", ""));
            }
            Type enumType = typeof(T);
            foreach (T item in Enum.GetValues(enumType))
            {
                string text = EnumHelper<T>.GetDescription(item);
                string value = Convert.ToInt32(item).ToString();
                selectListItems.Add(new SelectListItem(text, value));
            }
            return selectListItems;
        }
    }
}
