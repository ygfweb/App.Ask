using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App.Ask.Library.Utils
{
    /// <summary>
    /// Ajax结果
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public Dictionary<string, string> ErrorMessages { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 内容
        /// </summary>
        public object Context { get; set; }


        /// <summary>
        /// 添加错误信息,会自动将Success设置为false
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="errorMsg">错误提示</param>
        public void AddErrorMessage(string fieldName, string errorMsg)
        {
            this.ErrorMessages.Add(fieldName, errorMsg);
            this.Success = false;
        }

        public static AjaxResult CreateByMessage(string message, bool success = true)
        {
            return new AjaxResult
            {
                Context = null,
                ErrorMessages = new Dictionary<string, string>(),
                Message = message,
                Success = success
            };
        }

        public static AjaxResult CreateDefaultSuccess()
        {
            return new AjaxResult
            {
                Context = null,
                ErrorMessages = new Dictionary<string, string>(),
                Message = "",
                Success = true
            };
        }

        public static AjaxResult CreateByContext(object context)
        {
            return new AjaxResult
            {
                Context = context,
                ErrorMessages = new Dictionary<string, string>(),
                Message = "",
                Success = true
            };
        }

        public static AjaxResult CreateByModelState(ModelStateDictionary modelState, string message = "")
        {
            var errors = modelState.GetAllErrors();
            bool success = errors.Count <= 0;
            AjaxResult result = new AjaxResult
            {
                Message = message,
                Success = success,
                Context = null,
                ErrorMessages = errors
            };
            return result;
        }
    }
}
