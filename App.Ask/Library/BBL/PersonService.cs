using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using App.Ask.Library.Extensions;
using App.Ask.Library.Services;
using App.Ask.Library.Setting;
using App.Ask.Library.Utils;
using App.Ask.Models;
using SiHan.Libs.Utils.Paging;
using SiHan.Libs.Utils.Reflection;
using SiHan.Libs.Utils.Text;
using Microsoft.AspNetCore.Http;

namespace App.Ask.Library.BBL
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class PersonService
    {
        private readonly DbFactory dbFactory;
        private readonly EncryptService encryptService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PersonService(DbFactory dbFactory, EncryptService encryptService, IHttpContextAccessor httpContextAccessor)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            this.encryptService = encryptService ?? throw new ArgumentNullException(nameof(encryptService));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// 获取当前登录的用户，未登录将返回NULL
        /// </summary>
        public async Task<PersonView> GetCurrentPersonViewAsync()
        {
            Guid? id = this.httpContextAccessor.HttpContext.User.GetUserId();
            if (id == null)
            {
                return null;
            }
            else
            {
                using (var work = this.dbFactory.StartWork())
                {
                    return await work.PersonView.GetByIdAsync(id.Value);
                }
            }
        }

        /// <summary>
        /// 获取用户（如果用户被软删除同样将返回NULL）
        /// </summary>
        public async Task<PersonView> GetPersonViewAsync(Guid id)
        {
            using (var work = this.dbFactory.StartWork())
            {
                PersonView person = await work.PersonView.GetByIdAsync(id);
                if (person == null || person.IsDelete)
                {
                    return null;
                }
                else
                {
                    return person;
                }
            }
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        public async Task<Person> Register(AccountRegisterModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            using (UnitOfWork work = this.dbFactory.StartWork())
            {
                RegisterConfig config = await work.Config.GetRegisterConfigAsync();
                #region 检查邀请码
                if (config.IsEnableInviteCode)
                {
                    if (string.IsNullOrWhiteSpace(model.InviteCode))
                    {
                        throw new ModelException(nameof(model.InviteCode), "请填写邀请码");
                    }
                    else
                    {
                        string inputCode = model.InviteCode.Trim();
                        if (!(await work.InviteCode.IsExistCodeAsync(inputCode)))
                        {
                            throw new ModelException(nameof(model.InviteCode), "该邀请码不存在，请重新输入");
                        }
                    }
                }
                #endregion
                #region 检查账户
                if (model.UserName.ToLower().Contains("admin"))
                {
                    throw new ModelException(nameof(model.UserName), "该账户被系统保留，禁止使用该名称");
                }
                if (await work.Person.IsExistNameAsync(model.UserName))
                {
                    throw new ModelException(nameof(model.UserName), "该账户已被注册");
                }
                #endregion
                Role role = await work.Role.GetSingleAsync(Enums.RoleType.User);
                DateTime now = DateTime.Now;
                using (var trans = work.BeginTransaction())
                {
                    try
                    {
                        Person person = new Person
                        {
                            Id = GuidHelper.CreateSequential(),
                            AccountName = model.UserName.Trim(),
                            CreateTime = now,
                            LastUpdated = now,
                            NickName = model.UserName.Trim(),
                            RoleId = role.Id,
                            Password = this.encryptService.PasswordHash(model.Password),
                            Avatar = GlobalVariable.DefaultAvatar
                        };
                        await work.Person.InsertAsync(person, trans);
                        PersonData personData = new PersonData()
                        {
                            Id = GuidHelper.CreateSequential(),
                            AnswerNum = 0,
                            ArticleNum = 0,
                            AskNum = 0,
                            LikeNum = 0,
                            PersonId = person.Id,
                            Score = 0
                        };
                        await work.PersonData.InsertAsync(personData, trans);
                        trans.Commit();
                        return person;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 登录账号
        /// </summary>
        public async Task Login(AccountLoginModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            using (var work = this.dbFactory.StartWork())
            {
                Person person = await work.Person.GetByAccountNameAsync(model.UserName);
                if (person == null || person.IsDelete)
                {
                    throw new ModelException(nameof(model.Password), "登录账号或密码错误");
                }
                else
                {
                    string password = this.encryptService.PasswordHash(model.Password);
                    if (person.Password != password)
                    {
                        throw new ModelException(nameof(model.Password), "登录账号或密码错误");
                    }
                    else
                    {
                        await this.httpContextAccessor.HttpContext.LoginAsync(person, model.IsRememberMe);
                    }
                }
            }
        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        public async Task<PagingResult<PersonView>> SearchAsync(string text, int currentPage, RoleFilterType roleFilter = RoleFilterType.All, SearchType searchType = SearchType.Visible)
        {
            using (var work = this.dbFactory.StartWork())
            {
                PageConfig config = await work.Config.GetConfigAsync<PageConfig>();
                return await work.PersonView.SearchAsync(text, searchType, roleFilter, currentPage, config.PageSize);
            }
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        public async Task<int> ModifyRoleAsync(Guid personId, RoleType roleType)
        {
            using (var work = this.dbFactory.StartWork())
            {
                Role role = await work.Role.GetSingleAsync(roleType);

                return await work.Person.ModifyRole(personId, role);
            }
        }

        /// <summary>
        /// 软删除用户
        /// </summary>
        public async Task<int> RemoveAsync(Guid personId)
        {
            using (var work = this.dbFactory.StartWork())
            {
                PersonView person = await work.PersonView.GetByIdAsync(personId);
                if (person == null)
                {
                    throw new Exception("该用户不存在");
                }
                if (person.RoleType != RoleType.User)
                {
                    throw new Exception("删除该用户前需要将其角色修改普通会员");
                }
                return await work.Person.DeleteByIdAsync(personId);
            }
        }

        /// <summary>
        /// 软删除用户
        /// </summary>
        public async Task<int> RestoreRemoveAsync(Guid personId)
        {
            using (var work = this.dbFactory.StartWork())
            {
                PersonView person = await work.PersonView.GetByIdAsync(personId);
                if (person == null)
                {
                    throw new Exception("该用户不存在");
                }
                return await work.Person.RestoreRemoveByIdAsync(personId);
            }
        }

        /// <summary>
        /// 禁言用户
        /// </summary
        public async Task<int> MuteByIdAsync(Guid personId)
        {
            using (var work = this.dbFactory.StartWork())
            {
                PersonView person = await work.PersonView.GetByIdAsync(personId);
                if (person == null)
                {
                    throw new Exception("该用户不存在");
                }
                if (person.RoleType != RoleType.User)
                {
                    throw new Exception("操作该用户前需要将其角色修改普通会员");
                }
                return await work.Person.MuteByIdAsync(personId);
            }
        }

        /// <summary>
        /// 恢复被禁言的用户
        /// </summary
        public async Task<int> RestoreMuteByIdAsync(Guid personId)
        {
            using (var work = this.dbFactory.StartWork())
            {
                PersonView person = await work.PersonView.GetByIdAsync(personId);
                if (person == null)
                {
                    throw new Exception("该用户不存在");
                }
                return await work.Person.RestoreMuteByIdAsync(personId);
            }
        }

        /// <summary>
        /// 修改用户昵称
        /// </summary>
        public async Task<int> ModifyNickNameAsync(ProfileInfoEditModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            model.NickName = model.NickName.Trim();
            Guid? id = this.httpContextAccessor.HttpContext.User.GetUserId();
            if (id == null)
            {
                throw new Exception("用户未登录");
            }
            using (var work = this.dbFactory.StartWork())
            {
                Person dbPerson = await work.Person.SingleByIdAsync(id.Value);
                if (string.Equals(dbPerson.NickName, model.NickName, StringComparison.OrdinalIgnoreCase) || string.Equals(dbPerson.AccountName, model.NickName, StringComparison.OrdinalIgnoreCase))
                {
                    return await work.Person.ModifyNickNameAsync(dbPerson.Id, model.NickName);
                }
                else
                {
                    if (await work.Person.IsExistNameAsync(model.NickName))
                    {
                        throw new ModelException(nameof(model.NickName), "该昵称已存在，请重新输入");
                    }
                    return await work.Person.ModifyNickNameAsync(dbPerson.Id, model.NickName);
                }
            }
        }

        /// <summary>
        /// 修改用户头像
        /// </summary>
        public async Task<int> ModifyAvatarAsync(PersonView person, UploadInfo uploadInfo)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            if (uploadInfo == null)
            {
                throw new ArgumentNullException(nameof(uploadInfo));
            }
            using (var work = this.dbFactory.StartWork())
            {
                return await work.Person.ModifyAvatarAsync(person.Id, uploadInfo.UrlPath);
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        public async Task ModifyPasswordAsync(ProfileModifyPasswordModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            string pwdHash = this.encryptService.PasswordHash(model.NewPassword);
            PersonView person = await this.GetCurrentPersonViewAsync();
            using (var work = this.dbFactory.StartWork())
            {
                if (person.Password != this.encryptService.PasswordHash(model.Password))
                {
                    throw new ModelException(nameof(model.Password), "登录密码错误");
                }
                using (var trans = work.BeginTransaction())
                {
                    try
                    {
                        await work.Person.ModifyPasswordAsync(person.Id, pwdHash, trans);
                        await work.Person.UpdateLastTimeAsync(person.Id, trans);
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
    }
}
