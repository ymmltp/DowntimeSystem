using DowntimeSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using DowntimeSystem.Models.Unitity;

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

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult NoAccess()
        {
            return View();
        }

        #region task
        public IActionResult Query()
        {
            return View();
        }
        public IActionResult Query_test()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [ApproveAuthorize(Roles = 2) ]
        public IActionResult Task_ReviewRCCA()
        {
            return View();
        }

        public IActionResult IssueSummary()
        {
            return View();
        }
        #endregion

        #region query
        public IActionResult QEQSparepartChangeHistory()
        {
            return View();
        }
        public IActionResult Page_RCCA()
        {
            return View();
        }
        #endregion

        #region Management
        public IActionResult Manage_Resource_EQID_PN()
        {
            return View();
        }
        public IActionResult Manage_RCCAWI()
        {
            return View();
        }
        #endregion

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
        public IActionResult Maintenance()
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

        #region Test Page
        public IActionResult testDashboardPage()
        {
            return View();
        }
        public IActionResult Task_CreateIncident()
        {
            return View();
        }
        #endregion

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
                UserInfo ui = ad.GetADUserEntity(ntid);
                HttpContext.Request.Cookies.TryGetValue("dt-ntid", out string value);
                if (string.IsNullOrEmpty(value))
                {
                    HttpContext.Response.Cookies.Append("dt-ntid", ntid, new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(120)
                    });
                    HttpContext.Response.Cookies.Append("dt-displayname", ui.DisplayName, new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(120)
                    });
                    HttpContext.Response.Cookies.Append("dt-email", ui.Email, new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(120)
                    }); 
                } 
                return Json(ui.DisplayName);
            }
            catch (Exception err)
            {
                return new BadRequestResult();
            }
        }
    }
}
