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
        private static string version = "V2.0.0";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Contact()
        {
            ViewData["Version"] = version;
            return View();
        }

        public IActionResult NoAccess()
        {
            ViewData["Version"] = version;
            return View();
        }

        #region task
        public IActionResult Query()
        {
            ViewData["Version"] = version;
            return View();
        }
        public IActionResult Query_test()
        {
            ViewData["Version"] = version;
            return View();
        }
        public IActionResult Create()
        {
            ViewData["Version"] = version;
            return View();
        }
        [ApproveAuthorize(Roles = 2) ]
        public IActionResult Task_ReviewRCCA()
        {
            ViewData["Version"] = version;
            return View();
        }

        public IActionResult Task_IssueSummary()
        {
            ViewData["Version"] = version;
            return View();
        }

        public IActionResult Task_SparepartChange()
        {
            ViewData["Version"] = version;
            return View();
        }
        
        #endregion

        #region query
        public IActionResult QEQSparepartChangeHistory()
        {
            ViewData["Version"] = version;
            return View();
        }
        public IActionResult Page_RCCA()
        {
            ViewData["Version"] = version;
            return View();
        }
        #endregion

        #region Management
        public IActionResult Manage_Resource_EQID_PN()
        {
            ViewData["Version"] = version;
            return View();
        }
        public IActionResult Manage_RCCAWI()
        {
            ViewData["Version"] = version;
            return View();
        }
        
        public IActionResult Manage_EscalateNameList()
        {
            ViewData["Version"] = version;
            return View();
        }
        #endregion

        #region dashboard
        public IActionResult Dashboard()
        {
            ViewData["Version"] = version;
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
            ViewData["Version"] = version;
            return View();
        }
        public IActionResult Dashboard_Predictive()
        {
            ViewData["Version"] = version;
            return View();
        }
        public IActionResult Coming()
        {
            ViewData["Version"] = version;
            return View();
        }   
        public IActionResult Maintenance()
        {
            ViewData["Version"] = version;
            return View();
        }
        #endregion

        #region Unused
        public IActionResult Login()
        {
            ViewData["Version"] = version;
            return View();
        }
        #endregion

        #region Test Page
        public IActionResult testDashboardPage()
        {
            ViewData["Version"] = version;
            return View();
        }
        public IActionResult Task_CreateIncident()
        {
            ViewData["Version"] = version;
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
                HttpContext.Request.Cookies.TryGetValue("dt-ntid", out string value);
                if (string.IsNullOrEmpty(value))
                {
                    HttpContext.Response.Cookies.Append("dt-ntid", "1382919", new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(120)
                    });
                    HttpContext.Response.Cookies.Append("dt-displayname", "Adele Lu", new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(120)
                    });
                    HttpContext.Response.Cookies.Append("dt-email", "Adele_Lu@jabil.com", new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(120)
                    });
                }
                return Json("Adele Lu");
            }
            catch (Exception err)
            {
                return new BadRequestResult();
            }


            // try
            // {
            //     ad.Domain = domain;
            //     string identityName = HttpContext.User.Identity.Name;
            //     int splitIndex = identityName.IndexOf('\\');
            //     string ntid = splitIndex > -1 ? identityName.Substring(splitIndex + 1) : identityName;
            //     UserInfo ui = ad.GetADUserEntity(ntid);
            //     HttpContext.Request.Cookies.TryGetValue("dt-ntid", out string value);
            //     if (string.IsNullOrEmpty(value))
            //     {
            //         HttpContext.Response.Cookies.Append("dt-ntid", ntid, new CookieOptions
            //         {
            //             Expires = DateTime.Now.AddMinutes(120)
            //         });
            //         HttpContext.Response.Cookies.Append("dt-displayname", ui.DisplayName, new CookieOptions
            //         {
            //             Expires = DateTime.Now.AddMinutes(120)
            //         });
            //         HttpContext.Response.Cookies.Append("dt-email", ui.Email, new CookieOptions
            //         {
            //             Expires = DateTime.Now.AddMinutes(120)
            //         });
            //     }
            //     return Json(ui.DisplayName);
            // }
            // catch (Exception err)
            // {
            //     return new BadRequestResult();
            // }
        }
    }
}
