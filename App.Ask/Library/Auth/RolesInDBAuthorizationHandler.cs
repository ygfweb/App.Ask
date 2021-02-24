using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Ask.Library.BBL;
using App.Ask.Library.DAL;
using App.Ask.Library.Entity;
using App.Ask.Library.Enums;
using App.Ask.Library.Extensions;
using SiHan.Libs.Utils.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace App.Ask.Library.Auth
{
    /// <summary>
    /// 用户角色检查
    /// </summary>
    public class RolesInDBAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        private readonly DbFactory dbFactory;

        public RolesInDBAuthorizationHandler(DbFactory dbFactory)
        {
            this.dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
            }
            else
            {
                if (requirement.AllowedRoles == null || requirement.AllowedRoles.Any() == false)
                {
                    // 意味着任何登录用户都可以访问该资源
                    context.Succeed(requirement);
                }
                else
                {
                    var userId = context.User.GetUserId();
                    var roles = requirement.AllowedRoles;
                    if (userId != null)
                    {
                        using (var work = this.dbFactory.StartWork())
                        {
                            PersonView person = await work.PersonView.GetByIdAsync(userId.Value);
                            if (person == null)
                            {
                                context.Fail();
                            }
                            else
                            {
                                string roleName = EnumHelper<RoleType>.GetDescription(person.RoleType);
                                if (roles.Contains(roleName))
                                {
                                    context.Succeed(requirement);
                                }
                                else
                                {
                                    context.Fail();
                                }
                            }
                        }
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }

        }
    }
}
