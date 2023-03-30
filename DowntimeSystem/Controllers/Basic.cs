using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowntimeSystem.Models;
using System.Globalization;

namespace DowntimeSystem.Controllers
{
    public class Basic : Controller
    {
        private static ADHelper ad = new ADHelper();
        private static string domain = "corp.jabil.org";
        private GregorianCalendar gc = new GregorianCalendar();

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

        public ActionResult GetCurrentWeek(string date)
        {
            var a = gc.GetWeekOfYear(Convert.ToDateTime(date), CalendarWeekRule.FirstDay, DayOfWeek.Sunday).ToString();
            return Json(a);
        }
    }
}
