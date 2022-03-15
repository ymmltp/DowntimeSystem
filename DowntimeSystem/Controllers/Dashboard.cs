using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowntimeSystem.Models;

namespace DowntimeSystem.Controllers
{
    public class Dashboard : Controller
    {
        private string[] contains = { "E-Calling", "Sparepart", "FPY", "Downtime System","EPM System" };
        # region 获取Error Code 最高频次的前五项
        public IActionResult GetTopErrorCode_ByCount(int limit =5)
        {
            using (ECContext db = new ECContext()) {
                try {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom)).GroupBy(e => e.Issue).Select(g => new
                    {
                        item = g.Key,
                        count = g.Count(),
                    }).OrderByDescending(e => e.count).Take(limit).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult GetTopErrorCode_ByCount_station(string errorcode,int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Issue.Equals(errorcode)).GroupBy(e => e.Station  ).Select(g => new
                    {
                        item = g.Key,
                        count = g.Count(),
                    }).OrderByDescending(e => e.count).Take(limit).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult GetTopErrorCode_ByCount_line(string errorcode,string station,int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Issue.Equals(errorcode) &e.Station.Equals(station)).GroupBy(e => e.Line).Select(g => new
                    {
                        item = g.Key,
                        count = g.Count(),
                    }).OrderByDescending(e => e.count).Take(limit).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        #region 获取RootCause ,downtime最高的前五项
        public IActionResult GetTopRootCause_ByDowntime(int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom)).GroupBy(e => e.Rootcause).Select(g => new
                    {
                        item = g.Key,
                        value = g.Sum(e=>e.Downtime)
                    }).OrderByDescending(e => e.value).Take(limit).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult GetTopRootCause_ByDowntime_ErrorCode(string rootcause, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Rootcause.Equals(rootcause)).GroupBy(e => e.Issue).Select(g => new
                    {
                        item = g.Key,
                        value = g.Sum(e=>e.Downtime)
                    }).OrderByDescending(e => e.value).Take(limit).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion
    }
}
