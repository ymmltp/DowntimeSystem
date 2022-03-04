using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowntimeSystem.Models;

namespace DowntimeSystem.Controllers
{
    public class Basic : Controller
    {
        private static ADHelper ad = new ADHelper();
        private static string domain = "corp.jabil.org";

        // GET: Basic
        public ActionResult Auth(string ntid,string password)
        {
            try
            {
                ad.Domain = domain;
                if (ad.TryAuthenticate(domain, ntid, password)) { 
                    UserInfo user = ad.GetADUserEntity(ntid);
                    return Json(user);
                }
                else
                    return Json(false);
            }
            catch(Exception ex) {
                return Json(false);
            }
        }
    }
}
