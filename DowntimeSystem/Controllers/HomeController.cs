using DowntimeSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using DowntimeSystem.Models;

namespace DowntimeSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ADHelper ad = new ADHelper();
        private static string domain = "corp.jabil.org";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Query()
        {
            return View();
        }

        public IActionResult IssueSummary()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Manage_Resource_EQID_PN()
        {
            return View();
        }


        #region dashboard
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Dashboard_Downtime_OverView()
        {
            return View();
        }
        public IActionResult Dashboard_Downtime_SubPage()
        {
            return View();
        }
        public IActionResult Dashboard_MTTRandMTBF()
        {
            return View();
        }
        public IActionResult Dashboard_MTTRandMTBF_OverView()
        {
            return View();
        }
        public IActionResult Dashboard_MTTRandMTBF_SubPage()
        {
            return View();
        }

        public IActionResult Dashboard_EmployeeWork()
        {
            return View();
        }
        public IActionResult Dashboard_Predictive()
        {
            return View();
        }
        public IActionResult Coming()
        {
            return View();
        }
        #endregion

        #region Unused
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        //Test Page
        public IActionResult testDashboardPage()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //获取本地windows 登录信息，并写入cookie中
        [HttpGet]
        public ActionResult GetUserName()
        {
            try
            {
                ad.Domain = domain;
                string identityName = HttpContext.User.Identity.Name;
                int splitIndex = identityName.IndexOf('\\');
                string ntid = splitIndex > -1 ? identityName.Substring(splitIndex + 1) : identityName;
                string displayname = ad.GetADDispalyName(ntid);
                HttpContext.Request.Cookies.TryGetValue("dt-ntid", out string value);
                if (!string.IsNullOrEmpty(value))
                {
                    HttpContext.Response.Cookies.Delete("dt-displayname");
                    HttpContext.Response.Cookies.Delete("dt-ntid");             
                }
                HttpContext.Response.Cookies.Append("dt-ntid", ntid, new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(120)
                });
                HttpContext.Response.Cookies.Append("dt-displayname", displayname, new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(120)
                });
                //UserInfo ui = ad.GetADUserEntity(ntid);
                return Json(displayname);
            }
            catch (Exception err)
            {
                return new BadRequestResult();
            }
        }
    }
}
