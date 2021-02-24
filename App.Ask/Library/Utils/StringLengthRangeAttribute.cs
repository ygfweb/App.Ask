using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Utils
{
    /// <summary>
    /// 字符串长度验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class StringLengthRangeAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' 长度请保持在 {1}-{2} 个字符之间";

        public StringLengthRangeAttribute(int minLength, int maxLength)
            : base(_defaultErrorMessage)
        {
            if (minLength < 0)
                throw new ArgumentOutOfRangeException("minLength", minLength, "字符串最小长度不能小于0");
            if (maxLength < 0)
                throw new ArgumentOutOfRangeException("maxLength", maxLength, "字符串最大长度不能小于0");
            if (maxLength <= minLength)
                throw new ArgumentOutOfRangeException("maxLength", maxLength, "字符串最大长度必须大于最小长度");
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            if (String.IsNullOrEmpty(valueAsString)) return true;
            return valueAsString.Length >= MinLength
                && valueAsString.Length <= MaxLength;
        }
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name, MinLength, MaxLength);
        }
        public int MaxLength { get; private set; }

        public int MinLength { get; private set; }
    }
}
