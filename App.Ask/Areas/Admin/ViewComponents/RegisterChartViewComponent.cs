using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Areas.Admin.Models;
using App.Ask.Library.BBL;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Areas.Admin.ViewComponents
{
    /// <summary>
    /// 注册用户趋势图表
    /// </summary>
    public class RegisterChartViewComponent : ViewComponent
    {
        private readonly StatisticsService statisticsService;

        public RegisterChartViewComponent(StatisticsService statisticsService)
        {
            this.statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            RegisterChartComponentModel model = new RegisterChartComponentModel
            {
                Data = await this.statisticsService.GetWeekRegisterAsync()
            };
            return View(model);
        }
    }
}
