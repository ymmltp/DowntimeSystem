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
        private string[] contains = { "eCalling", "FPY", "Downtime System" };
        # region 获取system
        [HttpGet]
        public IActionResult GetSystem()
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    var items = db.IncidentDets.Where(e => contains.Contains(e.Comefrom)).Select(e => e.Comefrom).Distinct().ToList();
                    return Json(items);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region 获取Error Code 最高频次的前五项
        public IActionResult GetTopErrorCode_ByCount(IncidentDet item, string[]departmentlist,string[] projectlist, string currentDay, string lastDay , int limit =5)
        {
            using (ECContext db = new ECContext()) {
                try {
                    var tmp = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length>0)  tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (departmentlist.Length > 0) tmp = tmp.Where(e => departmentlist.Contains(e.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = tmp.GroupBy(e => e.Issue).Select(g => new
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
        public IActionResult GetLine_ByErrorCode(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay, string errorcode, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (departmentlist.Length > 0) tmp = tmp.Where(e => departmentlist.Contains(e.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = tmp.Where(e =>  e.Issue.Equals(errorcode) ).GroupBy(e => e.Line).Select(g => new
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
        public IActionResult GetStation_ByLine(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay, string errorcode, string line, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) &  Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)) .ToList();
                    if (projectlist.Length > 0) tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (departmentlist.Length > 0) tmp = tmp.Where(e => departmentlist.Contains(e.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = tmp.Where(e => e.Issue.Equals(errorcode)&e.Line.Equals(line)).GroupBy(e => e.Station  )
                        .Select(g => new
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
        public IActionResult GetRootCause_ByStation(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay, string errorcode, string line, string station, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)  tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (departmentlist.Length > 0) tmp = tmp.Where(e => departmentlist.Contains(e.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = tmp.Where(e =>  e.Issue.Equals(errorcode) & e.Line.Equals(line) & e.Station.Equals(station) )
                        .GroupBy(e => e.Rootcause)
                        .Select(g => new
                        {
                            item = g.Key==null?"None Root Cause":g.Key,
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

        #region open & close count
        public IActionResult OpenClose_ByCount(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)  tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (departmentlist.Length > 0) tmp = tmp.Where(e => departmentlist.Contains(e.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    //var items = tmp.GroupBy(e => new { e.Incidentstatus ,e.Department})
                    //    .Select(g => new
                    //    {
                    //        item = g.Key.Incidentstatus == 2 ? "Close" : g.Key.Incidentstatus == 1? "OnGoing" : "Open",
                    //        department = g.Key.Department,
                    //        value = g.Key.Incidentstatus == 2 ?g.Sum(e=>e.Downtime) / 3600 :g.Sum(e=>Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds)) / 3600
                    //    }).OrderByDescending(e => e.department ).ThenBy(e=>e.item).Take(limit).ToList();
                    var items = tmp.GroupBy(e => e.Incidentstatus)
                    .Select(g => new
                    {
                        item = g.Key == 2 ? "Closed" : g.Key == 1 ? "On-going" : "Open",
                        value = g.Key == 2 ? g.Sum(e => e.Downtime) / 3600 : g.Sum(e => Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds)) / 3600
                    }).OrderByDescending(e  => e.item).Take(limit).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult OpenClose_Items(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay,string status )
        { 
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)  tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (departmentlist.Length > 0) tmp = tmp.Where(e => departmentlist.Contains(e.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    int incidentstatus = status == "Closed" ? 2 : status == "Open" ? 0 : 1;
                    tmp = tmp.Where(e => e.Incidentstatus == incidentstatus).ToList();
                    var items = tmp.OrderBy(e => e.Occurtime).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult OpenCloseDowntime_StationPieChart(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay, string status)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0) tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (departmentlist.Length > 0) tmp = tmp.Where(e => departmentlist.Contains(e.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    int incidentstatus = status == "Closed" ? 2 : status == "Open" ? 0 : 1;
                    var items = tmp.Where(e => e.Incidentstatus == incidentstatus)
                        .GroupBy(e => e.Station)
                        .Select(g => new {
                            key = g.Key,
                            value = g.Sum(e => e.Incidentstatus == 2 ? e.Downtime : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds)) / 3600
                        }).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult OpenCloseDowntime_DefectCodePieChart(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay, string status)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0) tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (departmentlist.Length > 0) tmp = tmp.Where(e => departmentlist.Contains(e.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    int incidentstatus = status == "Closed" ? 2 : status == "Open" ? 0 : 1;
                    var items = tmp.Where(e => e.Incidentstatus == incidentstatus)
                        .GroupBy(e => e.Issue)
                        .Select(g => new {
                            key = g.Key,
                            value = g.Sum(e => e.Incidentstatus == 2 ? e.Downtime : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds)) / 3600
                        }).ToList();
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
        public IActionResult GetTopDowntime_ByStation(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay,int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e=> contains.Contains(e.Comefrom) ).ToList();
                    if (projectlist.Length > 0) tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (departmentlist.Length > 0) tmp = tmp.Where(e => departmentlist.Contains(e.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = tmp.Where(e => Convert.ToDateTime(lastDay )< e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).GroupBy(e => e.Station).Select(g => new
                    {
                        item = g.Key,
                        value = g.Sum(e=>e.Incidentstatus==2?e.Downtime: Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds))  / 3600,
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
        public IActionResult GetTopDowntime_ByDepartment(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e=> contains.Contains(e.Comefrom) ).ToList();
                    if (projectlist.Length > 0) tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (departmentlist.Length > 0) tmp = tmp.Where(e => departmentlist.Contains(e.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = tmp.Where(e => Convert.ToDateTime(lastDay)< e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay))
                        .GroupBy(e => new { e.Incidentstatus, e.Department })
                        .Select(g => new
                    {
                        item = g.Key.Department,
                        status = g.Key.Incidentstatus,
                        value = g.Sum(e => e.Incidentstatus == 2 ? e.Downtime : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds)) / 3600,
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
