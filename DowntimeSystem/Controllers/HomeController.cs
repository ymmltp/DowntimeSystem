using DowntimeSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Coming()
        {
            return View();
        }

        public IActionResult IssueSummary()
        {
            return View();
        }
        public IActionResult PredictiveDashboard()
        {
            return View();
        }
        

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

        //public ActionResult GetUserName()
        //{
        //    try
        //    {
        //        ad.Domain = 
        //        string ntid = ad.GetADDispalyName();
        //        if (System.Web.HttpContext.Current.Request.Cookies["qn-ntid"] != null)
        //        {
        //            ntid = System.Web.HttpContext.Current.Request.Cookies["qn-ntid"].Value.ToString();
        //        }
        //        UserInfo ui = ADHelper.GetADUserEntity(ntid);
        //        return Content(ui.DisplayName, "text/html");
        //    }
        //    catch (Exception err)
        //    {
        //        return new HttpStatusCodeResult(204, err.Message);
        //    }
        //}
    }
}
