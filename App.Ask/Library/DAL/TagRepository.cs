using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Ask.Library.Entity;
using SiHan.Libs.Ado;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Ask.Library.DAL
{
    public class TagRepository : BaseRepository<Tag>
    {
        public TagRepository(DbConnection connection) : base(connection) { }

        /// <summary>
        /// 获取所有推荐的标签
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tag>> GetBestAsync()
        {
            string sql = "SELECT * FROM tag Where is_best = true;";
            return await this.DbConnection.SelectAsync<Tag>(sql);
        }

        /// <summary>
        /// 搜索标签，如果标签名为空，将抛出异常
        /// </summary>
        public async Task<List<Tag>> SearchAsync(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new ArgumentNullException(nameof(tagName));
            }
            tagName = tagName.Trim().ToUpper();
            string sql = "select * from tag where name ilike @tagName;"; // ilike 不区分大小写
            return await this.DbConnection.SelectAsync<Tag>(sql, new { tagName = $"%{tagName}%" });
        }



        public override async Task<int> InsertAsync(Tag entity, DbTransaction transaction = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            entity.Name = entity.Name.Trim();
            Tag dbTag = await this.GetByNameAsync(entity.Name);
            if (dbTag != null)
            {
                throw new Exception("该标签已存在");
            }
            return await base.InsertAsync(entity, transaction);
        }

        /// <summary>
        /// 通过名称获取标签，不存在则返回NULL
        /// </summary>
        public async Task<Tag> GetByNameAsync(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new ArgumentNullException(nameof(tagName));
            }
            string sql = "SELECT * FROM tag WHERE upper(name) = @name  LIMIT 1;";
            return await this.DbConnection.FirstOrDefaultAsync<Tag>(sql, new { name = tagName.Trim().ToUpper() });
        }

        /// <summary>
        /// 是否存在该标签名
        /// </summary>
        public async Task<bool> IsExistNameAsync(string tagName)
        {
            Tag tag = await this.GetByNameAsync(tagName);
            return tag != null;
        }

        /// <summary>
        /// 将标签列表转换成字符串列表
        /// </summary>
        public List<string> ToStringList(List<Tag> tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }
            List<string> result = new List<string>();
            foreach (var item in tags)
            {
                result.Add(item.Name);
            }
            return result;
        }

        /// <summary>
        /// 将标签列表转换成多选项
        /// </summary>
        public List<SelectListItem> ToSelectListItems(List<Tag> tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var tag in tags)
            {
                SelectListItem item = new SelectListItem(tag.Name, tag.Name);
                items.Add(item);
            }
            return items;
        }

        /// <summary>
        /// 去掉重复标签
        /// </summary>
        public List<string> RemoveDuplication(List<string> tags)
        {
            List<string> newList = new List<string>();
            foreach (var tag in tags)
            {
                if (newList.Contains(tag))
                {
                    continue;
                }
                else
                {
                    newList.Add(tag);
                }
            }
            return newList;
        }
    }
}
