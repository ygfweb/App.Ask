using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.Info;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Areas.Admin.Models
{
    /// <summary>
    /// 注册趋势图表组件模型
    /// </summary>
    public class RegisterChartComponentModel
    {
        public List<PersonRegisterInfo> Data { get; set; } = new List<PersonRegisterInfo>();
    }
}
