using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Areas.Admin.Models;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Utils;
using SiHan.Libs.Mapper;
using SiHan.Libs.Utils.Paging;
using SiHan.Libs.Utils.Text;
using Microsoft.CodeAnalysis;

namespace App.Ask.Library.BBL
{
    /// <summary>
    /// 标签服务
    /// </summary>
    public class TagService
    {
        private readonly DbFactory dbFactory;

        public TagService(DbFactory dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tag>> GetAllAsync()
        {
            using (var work = this.dbFactory.StartWork())
            {
                List<Tag> tags = await work.Tag.GetAllAsync();
                return tags.OrderByDescending(p => p.IsBest).ToList();
            }
        }

        public async Task<List<Tag>> SearchAsync(string tagName)
        {
            using (var work = this.dbFactory.StartWork())
            {
                List<Tag> tags = new List<Tag>();
                if (string.IsNullOrWhiteSpace(tagName))
                {
                    tags = await work.Tag.GetAllAsync();
                }
                else
                {
                    tags = await work.Tag.SearchAsync(tagName);
                }
                return tags.OrderByDescending(p => p.IsBest).ToList();
            }
        }

        /// <summary>
        /// 修改标签是否被推荐的状态
        /// </summary>
        public async Task<Tag> ChangeBestStatusAsync(Tag tag, bool isBest)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            using (var work = this.dbFactory.StartWork())
            {
                Tag dbTag = await work.Tag.SingleByIdAsync(tag.Id);
                if (dbTag == null)
                {
                    throw new Exception("该标签不存在，或已被删除");
                }
                dbTag.IsBest = isBest;
                int row = await work.Tag.UpdateAsync(dbTag);
                if (row == 1)
                {
                    return dbTag;
                }
                else
                {
                    throw new Exception("记录未更新");
                }
            }
        }

        /// <summary>
        /// 通过ID获取标签
        /// </summary>
        public async Task<Tag> SingleByIdAsync(Guid id)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Tag.SingleByIdAsync(id);
            }
        }


        /// <summary>
        /// 获取所有推荐标签
        /// </summary>
        public async Task<List<Tag>> GetBestTagsAsync()
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Tag.GetBestAsync();
            }
        }

        /// <summary>
        /// 插入单个标签，如果该标签已存在，则抛出异常
        /// </summary>
        public async Task<Tag> CreateAsync(TagEditModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ModelException(nameof(model.Name), "标签名称不能为空");
            }
            model.Name = model.Name.Trim();
            using (var work = this.dbFactory.StartWork())
            {
                Tag oldTag = await work.Tag.GetByNameAsync(model.Name);
                if (oldTag != null)
                {
                    throw new ModelException(nameof(model.Name), "该标签名称已存在");
                }
                else
                {
                    Tag tag = new Tag
                    {
                        Id = GuidHelper.CreateSequential(),
                        Name = model.Name,
                        IsBest = model.IsBest,
                        IsSystem = true
                    };
                    await work.Tag.InsertAsync(tag);
                    return tag;
                }
            }
        }

        /// <summary>
        /// 修改标签
        /// </summary>
        public async Task<Tag> ModifyAsync(TagEditModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ModelException(nameof(model.Name), "标签名称不能为空");
            }
            if (model.Id == null)
            {
                throw new Exception("标签ID不能为空");
            }
            model.Name = model.Name.Trim();
            using (var work = this.dbFactory.StartWork())
            {
                Tag dbTag = await work.Tag.SingleByIdAsync(model.Id.Value);
                if (dbTag == null)
                {
                    throw new ModelException(nameof(model.Name), "该标签不存在，或已被删除");
                }
                if (dbTag.Id != model.Id)
                {
                    throw new ModelException(nameof(model.Name), "该标签名称已被占用");
                }
                else
                {
                    dbTag.IsBest = model.IsBest;
                    dbTag.Name = model.Name;
                    await work.Tag.UpdateAsync(dbTag);
                    return dbTag;
                }
            }
        }


        /// <summary>
        /// 删除标签(同时删除标签与帖子的关联)
        /// </summary>
        public async Task DeleteAsync(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            using (var work = this.dbFactory.StartWork())
            {
                using (var trans = work.Connection.BeginTransaction())
                {
                    try
                    {
                        await work.Tag.DeleteAsync(tag, trans); // 删除标签
                        await work.PostTag.DeleteByTagAsync(tag, trans);  // 删除帖子与标签的关联
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 获取帖子的标签
        /// </summary>
        public async Task<List<Tag>> GetTagsByPostId(Guid postId)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.PostTag.GetTagsByPostAsync(postId);
            }
        }

        /// <summary>
        /// 获取帖子的标签
        /// </summary>
        public async Task<List<Tag>> GetTagsByPost(PostView post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            using (var work = this.dbFactory.StartWork())
            {
                return await work.PostTag.GetTagsByPostAsync(post.Id);
            }
        }
    }
}

