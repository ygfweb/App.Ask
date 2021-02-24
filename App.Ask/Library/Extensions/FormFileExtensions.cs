using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace App.Ask.Library.Extensions
{
    /// <summary>
    /// IFormFile扩展类
    /// </summary>
    public static class FormFileExtensions
    {
        /// <summary>
        /// 获取上传文件的内容
        /// </summary>
        public static async Task<byte[]> GetBytes(this IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 将上传文件拷贝到新的文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath">待创建的文件名称</param>
        /// <returns></returns>
        public static async Task CopyToNewAsync(this IFormFile file, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
