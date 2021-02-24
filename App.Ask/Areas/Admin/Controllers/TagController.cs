using System;
using System.Collections.Generic;
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
    public class TagController : BaseController
    {
        private readonly TagService tagService;

        public TagController(TagService tagService)
        {
            this.tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string search)
        {
            try
            {
                List<Tag> tags = await this.tagService.SearchAsync(search);
                return this.Json(AjaxResult.CreateByContext(tags));
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Tag> tags = await this.tagService.GetAllAsync();
                return this.Json(AjaxResult.CreateByContext(tags));
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            TagEditModel model = new TagEditModel();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TagEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Tag tag = await tagService.CreateAsync(model);
                    return this.Json(AjaxResult.CreateByContext(tag));
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
        public async Task<IActionResult> ChangeStatus([FromBody] Tag tag)
        {
            try
            {
                Tag newTag = await tagService.ChangeBestStatusAsync(tag, !tag.IsBest);
                return this.Json(AjaxResult.CreateByContext(newTag));
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            Tag tag = await this.tagService.SingleByIdAsync(id);
            TagEditModel model = ObjectMapper.Map<Tag, TagEditModel>(tag);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TagEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Tag tag = await tagService.ModifyAsync(model);
                    return this.Json(AjaxResult.CreateByContext(tag));
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
        public async Task<ActionResult> Delete([FromBody] Tag tag)
        {
            try
            {
                await this.tagService.DeleteAsync(tag);
                return this.Json(AjaxResult.CreateDefaultSuccess());
            }
            catch (Exception ex)
            {
                return this.Json(AjaxResult.CreateByMessage(ex.Message, false));
            }
        }
    }
}
