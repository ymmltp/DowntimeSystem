using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DowntimeSystem.Models.Unitity
{
    /// <summary> 
    /// 权限验证 
    /// </summary> 
    public class ApproveAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary> 
        /// 角色名称 
        /// </summary> 
        public int Roles { get; set; }
        /// <summary> 
        /// 验证权限（action执行前会先执行这里） 
        /// </summary> 
        /// <param name="filterContext"></param> 
        public override void OnActionExecuting(ActionExecutingContext Context)
        {
            var httpContext = (HttpContext)Context.HttpContext;
            string user = httpContext.Request.Cookies["dt-email"].ToString();
            var task = GetUser(Roles, user);
            task.Wait();
            bool hasPermissions = task.Result;// Block and wai
            if (!hasPermissions)
            {
                Context.Result = new RedirectResult("/Home/NoAccess");
            }
        }

        private static async Task<bool> GetUser(int aplevel,string user)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://cnwuxg0te01:9000/api/DowntimeContact/Query_Access?Email=" + user + "&Level=" + aplevel);
                    response.EnsureSuccessStatusCode(); // 确保响应状态码为成功状态
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody == "[]") 
                    { 
                        return false;
                    }
                    return true;
                }
                catch (HttpRequestException e)
                {
                    return false; 
                }
            }
        }
    }
}