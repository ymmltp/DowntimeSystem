using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowntimeSystem.Models.PMMS;

namespace DowntimeSystem.Controllers
{
    public class PMMSController : Controller
    {
        [HttpGet]
        public IActionResult getEQID()
        {
            using (PMMSContext db = new PMMSContext()) {
                var item = db.Eqs.Select(e => e.Eqid).ToList();
                return Json(item);
            }
        }
    }
}
