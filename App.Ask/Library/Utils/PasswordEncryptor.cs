using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App.Ask.Library.Utils
{
    /// <summary>
    /// 密码加密器
    /// </summary>
    public static class PasswordEncryptor
    {
        private static readonly string _key = "@ab+&?###";
        /// <summary>
        /// 加盐的MD5
        /// </summary>
        public static string Hash(string text)
        {
            text = text + _key;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}