using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App.Ask.Library.Utils
{
    /// <summary>
    /// 哈希值帮助类
    /// </summary>
    public static class HashHelper
    {
        public static string Sha256(string text)
        {
            if (String.IsNullOrEmpty(text)) return string.Empty;
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(text);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
