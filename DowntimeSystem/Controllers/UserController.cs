using Microsoft.AspNetCore.Mvc;
using DowntimeSystem.Models;

namespace DowntimeSystem.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        // GET: User/GetUserEntity
        public ActionResult GetUserEntity(string ntid)
        {
            ADHelper ad = new ADHelper();
            string domain = "corp.jabil.org";
            ad.Domain = domain;
            UserInfo userinfo = ad.GetADUserEntity(ntid);
            return Json(userinfo);
        }
    }
}