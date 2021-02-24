using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Utils
{
    /// <summary>
    /// 模型异常
    /// </summary>
    public class ModelException : Exception
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; }

        public ModelException(string fieldName, string message) : base(message)
        {
            FieldName = fieldName;
        }
        public AjaxResult ToAjaxResult()
        {
            AjaxResult result = new AjaxResult();
            result.Success = false;
            result.ErrorMessages.Add(this.FieldName, this.Message);
            return result;
        }
    }
}
