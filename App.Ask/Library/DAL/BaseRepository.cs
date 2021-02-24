using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using SiHan.Libs.Ado;

namespace App.Ask.Library.DAL
{
    public abstract class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbConnection DbConnection { get; set; }

        public BaseRepository(DbConnection dbConnection)
        {
            DbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        /// <summary>
        /// 插入实体，返回受影响的行数
        /// </summary>
        public virtual async Task<int> InsertAsync(TEntity entity, DbTransaction transaction = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return await this.DbConnection.InsertAsync(entity, transaction);
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        public virtual async Task<int> InsertMoreAsync(List<TEntity> entities, DbTransaction transaction)
        {
            if (entities == null || entities.Count == 0)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            return await this.DbConnection.InsertAsync(entities, transaction);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        public virtual async Task<int> DeleteAsync(TEntity entity, DbTransaction transaction = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return await this.DbConnection.DeleteAsync(entity, transaction);
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        public virtual async Task<int> DeleteMoreAsync(List<TEntity> entities, DbTransaction transaction)
        {
            if (entities == null || entities.Count == 0)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            return await this.DbConnection.DeleteAsync(entities, transaction);
        }

        /// <summary>
        /// 通过ID删除实体
        /// </summary>
        public virtual async Task<int> DeleteByIdAsync(Guid id, DbTransaction transaction = null)
        {
            return await this.DbConnection.DeleteByIdAsync<TEntity>(id, transaction);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        public virtual async Task<int> UpdateAsync(TEntity entity, DbTransaction transaction = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return await this.DbConnection.UpdateAsync<TEntity>(entity, transaction);
        }

        /// <summary>
        /// 批量更新实体
        public virtual async Task<int> UpdateMoreAsync(List<TEntity> entities, DbTransaction transaction)
        {
            if (entities == null || entities.Count == 0)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            return await this.DbConnection.UpdateAsync<TEntity>(entities, transaction);
        }

        /// <summary>
        /// 使用ID获取实体，若不存在，则返回NULL
        /// </summary>
        public virtual async Task<TEntity> SingleByIdAsync(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await this.DbConnection.SingleByIdAsync<TEntity>(id);
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await this.DbConnection.GetAllAsync<TEntity>();
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public virtual async Task<int> CountAsync()
        {
            return await this.DbConnection.CountAsync<TEntity>();
        }
    }
}
