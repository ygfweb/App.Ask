using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Library.Utils
{
    /// <summary>
    /// 图片流响应结果
    /// </summary>
    public class ImageStreamResult : FileStreamResult
    {
        /// <summary>
        /// 图片内容
        /// </summary>
        public byte[] ImageBytes { get; }
        public ImageStreamResult(byte[] imageBytes, string contentType = "image/jpeg") : base(new MemoryStream(imageBytes), contentType)
        {
            this.ImageBytes = imageBytes;
        }

        /// <summary>
        /// 将图片流拷贝到新文件
        /// </summary>
        public async Task SaveToFileAsync(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await this.FileStream.CopyToAsync(stream);
            }
        }
    }
}
