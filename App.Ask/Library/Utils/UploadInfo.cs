using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Ask.Library.Utils
{
    /// <summary>
    /// 上传信息
    /// </summary>
    public class UploadInfo
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 磁盘路径
        /// </summary>
        public string DiskPath { get; set; }
        /// <summary>
        /// URL路径
        /// </summary>
        public string UrlPath { get; set; }
    }
}