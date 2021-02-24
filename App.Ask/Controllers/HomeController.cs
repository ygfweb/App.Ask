using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using App.Ask.Library.Extensions;
using App.Ask.Library.Services;
using App.Ask.Library.Utils;
using App.Ask.Models;
using SiHan.Libs.Image;
using SiHan.Libs.Utils.Paging;
using SiHan.Libs.Utils.Reflection;
using SiHan.Libs.Utils.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Ask.Controllers
{
    [Route("[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly UploadService uploadService;
        private readonly PostService postService;
        private readonly PersonService personService;
        public HomeController(ILogger<HomeController> logger, UploadService upload, PostService postService, PersonService personService)
        {
            this.logger = logger;
            this.uploadService = upload;
            this.postService = postService;
            this.personService = personService;
        }

        [Route("/")]
        public async Task<IActionResult> Index(Guid topic, FilterType filter = FilterType.New, int page = 1)
        {
            if (page < 1)
            {
                return NotFound();
            }
            PagingResult<PostView> postViews = await postService.SearchAsync(topic, filter, "", page);
            if (postViews.PageCount > 1 && page > postViews.PageCount)
            {
                return NotFound();
            }
            PersonView person = await this.personService.GetCurrentPersonViewAsync();
            HomeIndexModel model = new HomeIndexModel
            {
                PostViews = postViews,
                LoginPerson = person
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> CodeImage()
        {
            string code = RandomHelper.GetVerifyCode();
            ImageWrapper image = ImageWrapper.CreateByVerifyCode(code);
            await HttpContext.Session.SetVerifyCodeAsync(code);
            return new ImageStreamResult(image.ImageBytes);
        }



        /// <summary>
        /// 上传图片，限制为1M
        /// </summary>
        [HttpPost]
        [Authorize]
        [RequestSizeLimit(1024 * 1024 * 1)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                UploadInfo info = await this.uploadService.UploadImageAsync(file, false);
                return this.Json(AjaxResult.CreateByContext(info.UrlPath));
            }
            catch (Exception e)
            {
                return this.Json(new { uploaded = 0, error = new { message = e.Message } });
            }
        }

        public IActionResult CreatePost()
        {
            return View();
        }
    }
}
