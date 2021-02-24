using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Areas.Admin.Models;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using App.Ask.Library.Utils;
using SiHan.Libs.Mapper;
using SiHan.Libs.Utils.Text;

namespace App.Ask.Library.BBL
{
    /// <summary>
    /// 话题服务类
    /// </summary>
    public class TopicService
    {
        private readonly DbFactory dbFactory;

        public TopicService(DbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<List<Topic>> GetAllAsync(bool isIncludeAnnounce, SearchType searchType = SearchType.Visible)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Topic.GetAllAsync(searchType, isIncludeAnnounce);
            }
        }

        /// <summary>
        /// 创建话题
        /// </summary>
        public async Task<Topic> CreateAsync(TopicEditModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ModelException(nameof(model.Name), "话题名称不能为空");
            }
            using (var work = this.dbFactory.StartWork())
            {
                Topic old = await work.Topic.GetByNameAsync(model.Name.Trim());
                if (old != null)
                {
                    throw new ModelException(nameof(model.Name), "该话题名称已存在");
                }
                Topic topic = ObjectMapper.Map<TopicEditModel, Topic>(model);
                topic.IsAnnounce = false;
                topic.Id = GuidHelper.CreateSequential();
                await work.Topic.InsertAsync(topic);
                return topic;
            }
        }

        public async Task<Topic> ModifyAsync(TopicEditModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (model.Id == null)
            {
                throw new ArgumentNullException(nameof(model.Id));
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ModelException(nameof(model.Name), "话题名称不能为空");
            }
            using (var work = this.dbFactory.StartWork())
            {
                Topic topic = await work.Topic.SingleByIdAsync(model.Id.Value);
                if (topic == null)
                {
                    throw new ModelException(nameof(model.Name), "该话题不存在或已被删除");
                }
                if (!string.Equals(model.Name, topic.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Topic old = await work.Topic.GetByNameAsync(model.Name.Trim());
                    if (old != null)
                    {
                        throw new ModelException(nameof(model.Name), "该话题名称已存在");
                    }
                }
                if (topic.IsAnnounce)
                {
                    throw new ModelException(nameof(model.Name), "公告为系统默认，不能被修改");
                }
                ObjectMapper.CopyValues<TopicEditModel, Topic>(model, topic);
                topic.IsAnnounce = false;
                await work.Topic.UpdateAsync(topic);
                return await work.Topic.SingleByIdAsync(model.Id.Value);
            }
        }

        /// <summary>
        /// 删除话题
        /// </summary>
        public async Task<int> DeleteAsync(Topic topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic));
            }
            if (topic.Id == null)
            {
                throw new ArgumentException("话题ID不能为空");
            }
            using (var work = this.dbFactory.StartWork())
            {
                Topic oldTopic = await work.Topic.SingleByIdAsync(topic.Id);
                if (oldTopic == null)
                {
                    throw new Exception("该话题不存在，或已被删除");
                }
                if (oldTopic.IsAnnounce)
                {
                    throw new Exception("公告栏不能被删除");
                }
                long count = await work.Post.GetCountByTopicAsync(topic);
                if (count > 0)
                {
                    throw new Exception("不能删除已存在内容的话题，但该话题可以被隐藏");
                }
                else
                {
                    return await work.Topic.DeleteByIdAsync(topic.Id);
                }
            }
        }

        /// <summary>
        /// 切换话题的状态
        /// </summary>
        public async Task<int> ChangeStatusAsync(Topic topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic));
            }
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Topic.SetHideAsync(topic, !topic.IsHide);
            }
        }

        /// <summary>
        /// 通过ID获取话题
        /// </summary>
        public async Task<Topic> GetByIdAsync(Guid id)
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Topic.SingleByIdAsync(id);
            }
        }

        /// <summary>
        /// 获取公告话题
        /// </summary>
        public async Task<Topic> GetAnnounceAsync()
        {
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Topic.GetAnnounce();
            }
        }
    }
}
