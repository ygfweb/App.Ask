using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App.Ask.Library.Extensions
{
    /// <summary>
    /// ModelStateDictionary扩展类
    /// </summary>
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// 从模型状态中获取错误信息
        /// </summary>
        public static Dictionary<string, string> GetAllErrors(this ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (!modelState.IsValid)
            {
                var errorList = modelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                foreach (var item in errorList)
                {
                    if (item.Value.Length > 0)
                    {
                        errors.Add(item.Key, item.Value[0]);
                    }
                }
            }
            return errors;
        }

        /// <summary>
        /// 转换为AJAX返回对象
        /// </summary>
        public static AjaxResult ToAjaxResult(this ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            if (modelState.IsValid)
            {
                return AjaxResult.CreateByMessage("操作成功");
            }
            else
            {
                Dictionary<string, string> err = modelState.GetAllErrors();
                AjaxResult result = new AjaxResult();
                result.Success = false;
                foreach (var item in err)
                {
                    result.ErrorMessages.Add(item.Key, item.Value);
                }
                return result;
            }
        }
    }
}
