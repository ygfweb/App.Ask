using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Areas.Admin.Models;
using App.Ask.Library.BBL;
using App.Ask.Library.Entity;
using App.Ask.Library.Extensions;
using App.Ask.Library.Utils;
using SiHan.Libs.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.Areas.Admin.Controllers
{
    public class TopicController : BaseController
    {
        private readonly TopicService topicService;

        public TopicController(TopicService topicService)
        {
            this.topicService = topicService ?? throw new ArgumentNullException(nameof(topicService));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Topic> topics = await this.topicService.GetAllAsync(true, Library.Enums.SearchType.ALL);
                AjaxResult result = AjaxResult.CreateByContext(topics);
                return this.Json(result);
            }
            catch (Exception e)
            {
                AjaxResult result = AjaxResult.CreateByMessage(e.Message, false);
                return this.Json(result);
            }
        }

        public ActionResult Create()
        {
            TopicEditModel model = new TopicEditModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TopicEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Topic topic = await topicService.CreateAsync(model);
                    return this.Json(AjaxResult.CreateByContext(topic));
                }
                catch (ModelException ex)
                {
                    return this.Json(ex.ToAjaxResult());
                }
                catch (Exception ex)
                {
                    return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
                }
            }
            else
            {
                return this.Json(ModelState.ToAjaxResult());
            }
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            Topic topic = await this.topicService.GetByIdAsync(id);
            TopicEditModel model = ObjectMapper.Map<Topic, TopicEditModel>(topic);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TopicEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Topic topic = await topicService.ModifyAsync(model);
                    return this.Json(AjaxResult.CreateByContext(topic));
                }
                catch (ModelException ex)
                {
                    return this.Json(ex.ToAjaxResult());
                }
                catch (Exception ex)
                {
                    return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
                }
            }
            else
            {
                return this.Json(ModelState.ToAjaxResult());
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] Topic topic)
        {
            try
            {
                int result = await this.topicService.DeleteAsync(topic);
                return this.Json(AjaxResult.CreateDefaultSuccess());
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromBody] Topic topic)
        {
            try
            {
                int result = await this.topicService.ChangeStatusAsync(topic);
                return this.Json(AjaxResult.CreateDefaultSuccess());
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }
    }
}


