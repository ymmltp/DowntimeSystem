
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using DowntimeSystem.Models;

namespace DowntimeSystem.Utils
{
    /// <summary>
    /// 权限验证（ASP.NET Core 版）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class UserAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary> 角色名称（单值） </summary>
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 读取 Cookie（ASP.NET Core）
            var userName = context.HttpContext.Request.Cookies["dt-ntid"];
            if (string.IsNullOrWhiteSpace(userName))
            {
                // 没有登录信息，直接拒绝/重定向
                context.Result = new RedirectToActionResult("NoAccess", "Home", null);
                return;
            }

            // 通过依赖注入获得 DbContext（不要手动 new）
            var db = context.HttpContext.RequestServices.GetRequiredService<ECContext>();

            var roleStr = db.Users
                            .Where(e => e.NTID == userName)
                            .Select(e => e.Role) // 注意你的列名是 role
                            .FirstOrDefault();

            var userRoles = (roleStr ?? string.Empty)
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(r => r.Trim())
                            .ToArray();

            // 忽略大小写的比较
            var hasRole = userRoles.Contains(Roles, StringComparer.OrdinalIgnoreCase);

            if (!hasRole)
            {
                // 没权限：重定向或返回 403
                // context.Result = new ForbidResult();
                context.Result = new RedirectToActionResult("NoAccess", "Home", null);
            }
        }
    }
}
