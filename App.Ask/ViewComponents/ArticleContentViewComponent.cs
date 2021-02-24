using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Ask.ViewComponents
{
    /// <summary>
    /// 文章内容模型
    /// </summary>
    public class ArticleContentViewComponent : ViewComponent
    {
        private readonly PersonService personService;
        private readonly TagService tagService;
        private readonly TopicService topicService;
        private readonly PostService postService;
        private readonly ZanService zanService;
        private readonly FavoriteService favoriteService;

        public ArticleContentViewComponent(PersonService personService, TagService tagService, TopicService topicService, PostService postService, ZanService zanService, FavoriteService favoriteService)
        {
            this.personService = personService ?? throw new ArgumentNullException(nameof(personService));
            this.tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
            this.topicService = topicService ?? throw new ArgumentNullException(nameof(topicService));
            this.postService = postService ?? throw new ArgumentNullException(nameof(postService));
            this.zanService = zanService ?? throw new ArgumentNullException(nameof(zanService));
            this.favoriteService = favoriteService ?? throw new ArgumentNullException(nameof(favoriteService));
        }

        public async Task<IViewComponentResult> InvokeAsync(Post pPost)
        {
            if (pPost == null)
            {
                throw new ArgumentNullException(nameof(pPost));
            }
            PersonView loginUser = await this.personService.GetCurrentPersonViewAsync();
            PersonView publicUser = await this.personService.GetPersonViewAsync(pPost.PersonId);
            List<Tag> tags = await this.tagService.GetTagsByPostId(pPost.Id);
            ArticleContentComponentModel model = new ArticleContentComponentModel()
            {
                LoginPerson = loginUser,
                Post = pPost,
                PublicPerson = publicUser,
                Tags = tags,
                Topic = await topicService.GetByIdAsync(pPost.TopicId),
                PostData = await postService.GetPostDataAsync(pPost),
                IsLikePost = await this.zanService.IsPostZanAsync(pPost.Id),
                CurrentUrl = this.HttpContext.Request.Path,
                IsFavoritePost = await this.favoriteService.IsFavorite(pPost.Id)
            };
            return View(model);
        }
    }
}
