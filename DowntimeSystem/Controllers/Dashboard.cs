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
        //最近30天
        private DateTime currentDay =Convert.ToDateTime( DateTime.Now.ToString("yyyy-MM-dd"));
        private DateTime lastDay = Convert.ToDateTime("2022-01-01");//Convert.ToDateTime(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));

        # region 获取Error Code 最高频次的前五项
        public IActionResult GetTopErrorCode_ByCount(int limit =5)
        {
            using (ECContext db = new ECContext()) {
                try {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & lastDay<e.Occurtime& e.Occurtime<currentDay).GroupBy(e => e.Issue).Select(g => new
                    {
                        item = g.Key,
                        value = g.Count(),
                    }).OrderByDescending(e => e.value).Take(limit).ToList();
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
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Issue.Equals(errorcode) & lastDay < e.Occurtime & e.Occurtime < currentDay).GroupBy(e => e.Station  ).Select(g => new
                    {
                        item = g.Key,
                        value = g.Count(),
                    }).OrderByDescending(e => e.value).Take(limit).ToList();
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
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Issue.Equals(errorcode) &e.Station.Equals(station) & lastDay < e.Occurtime & e.Occurtime < currentDay).GroupBy(e => e.Line).Select(g => new
                    {
                        item = g.Key,
                        value = g.Count(),
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

        #region 获取RootCause ,downtime最高的前五项
        public IActionResult GetTopRootCause_ByDowntime(int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & lastDay < e.Occurtime & e.Occurtime < currentDay).GroupBy(e => e.Rootcause).Select(g => new
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
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Rootcause.Equals(rootcause) & lastDay < e.Occurtime & e.Occurtime < currentDay).GroupBy(e => e.Issue).Select(g => new
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
        //all error code top five rootcause
        public IActionResult GetAllErrorCode_ByCount()
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & lastDay < e.Occurtime & e.Occurtime < currentDay).GroupBy(e => e.Issue).Select(g => new
                    {
                        item = g.Key,
                        value = g.Count(),
                    }).OrderByDescending(e => e.value).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult GetTopRootCause_ByErrorCode(string errorcode,int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Issue.Equals(errorcode) & lastDay < e.Occurtime & e.Occurtime < currentDay).GroupBy(e => e.Rootcause).Select(g => new
                    {
                        item = g.Key==null?"UnClose":g.Key,
                        value = g.Count()
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

        # region 获取Station ,downtime最高的前五项
        public IActionResult GetTopDowntime_ByStation(int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & lastDay < e.Occurtime & e.Occurtime < currentDay).GroupBy(e => e.Station).Select(g => new
                    {
                        item = g.Key,
                        value = g.Sum(e => e.Downtime)
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

        #region 每个部门的downtime
        public IActionResult GetTopDowntime_ByDepartment()
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & lastDay < e.Occurtime & e.Occurtime < currentDay).GroupBy(e => e.Department).Select(g => new
                    {
                        item = g.Key,
                        value = g.Sum(e => e.Downtime)
                    }).OrderByDescending(e => e.value).ToList();
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
