using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.Extensions;
using App.Ask.Library.Utils;
using SiHan.Libs.Image;
using SiHan.Libs.Utils.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace App.Ask.Library.Services
{
    /// <summary>
    /// 上传服务
    /// </summary>
    public class UploadService
    {
        /// <summary>
        /// 上传目录
        /// </summary>
        private const string UploadDirectory = "upload";

        private readonly IWebHostEnvironment environment;

        public UploadService(IWebHostEnvironment environment)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public virtual async Task<UploadInfo> UploadImageAsync(IFormFile formFile, bool avatar)
        {
            if (formFile == null)
            {
                throw new Exception("上传文件为空");
            }
            string oldName = formFile.FileName; //原始文件名
            string extName = Path.GetExtension(oldName); // 获取扩展名
            string newName = StringHelper.GetGuidString() + extName; //构建新的文件名
            string webRootPath = this.environment.WebRootPath;
            if (extName != ".jpg" && extName != ".png" && extName != ".gif" && extName != ".jpeg")
            {
                throw new Exception("上传图片仅支持jpg,png,gif格式");
            }
            byte[] bytes = await formFile.GetBytes();
            ImageWrapper image = new ImageWrapper(bytes);
            if (!image.IsReallyImage())
            {
                throw new Exception("上传文件不是真实图片");
            }
            if (avatar)
            {
                // 上传头像
                string updateMark = "avatar";
                string diskPath = Path.Combine(webRootPath, UploadDirectory, updateMark);
                if (!Directory.Exists(diskPath))
                {
                    Directory.CreateDirectory(diskPath);
                }
                string filePath = Path.Combine(diskPath, newName);
                image.Resize(128, 128).SaveToFile(filePath);
                return new UploadInfo
                {
                    DiskPath = filePath,
                    FileName = newName,
                    UrlPath = $"/{UploadDirectory}/{updateMark}/{newName}"
                };
            }
            else
            {
                // 上传图片
                string updateMark = "image";
                string diskPath = Path.Combine(webRootPath, UploadDirectory, updateMark);
                if (!Directory.Exists(diskPath))
                {
                    Directory.CreateDirectory(diskPath);
                }
                string filePath = Path.Combine(diskPath, newName);
                image.SaveToFile(filePath);
                return new UploadInfo
                {
                    DiskPath = filePath,
                    FileName = newName,
                    UrlPath = $"/{UploadDirectory}/{updateMark}/{newName}"
                };
            }
        }
    }
}
