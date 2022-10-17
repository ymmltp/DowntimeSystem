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
                    if(!string.IsNullOrEmpty(line)) tmp = tmp.Where(e => e.Line.Equals(line)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = tmp.Where(e => e.Issue.Equals(errorcode)).GroupBy(e => e.Station  )
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
                    if (!string.IsNullOrEmpty(line)) tmp = tmp.Where(e => e.Line.Equals(line)).ToList();
                    if (!string.IsNullOrEmpty(station)) tmp = tmp.Where(e => e.Station.Equals(station)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = tmp.Where(e =>  e.Issue.Equals(errorcode))
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
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay) & e.Calcdowntime == true);
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (!string.IsNullOrEmpty(item.Department)) where = where.Where(e => e.Department.Equals(item.Department));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var items = where.ToList().GroupBy(e => e.Incidentstatus)
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
                    var where  = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay) & e.Calcdowntime == true);
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (!string.IsNullOrEmpty(item.Department)) where = where.Where(e => e.Department.Equals(item.Department));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    int incidentstatus = status == "Closed" ? 2 : status == "Open" ? 0 : 1;
                    where = where.Where(e => e.Incidentstatus == incidentstatus);
                    var items = where.ToList().OrderBy(e => e.Occurtime).ToList();
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
                    var tmp = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay) & e.Calcdowntime == true).ToList();
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
                    var tmp = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay) & e.Calcdowntime == true).ToList();
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
                    var where = db.IncidentDets.Where(e=> contains.Contains(e.Comefrom) );
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (!string.IsNullOrEmpty(item.Department)) where = where.Where(e => e.Department.Equals(item.Department));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.ToList();
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

        #region MTTR
        public IActionResult TotalMTTR(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2);
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var calcu = tmp.Select(e => new
                    {
                        value = (Convert.ToDateTime(e.Finishtime)-Convert.ToDateTime(e.Repairtime)).TotalMinutes , //mins
                        Incidentstatus = e.Incidentstatus,
                    }).ToList();
                    var items = calcu.GroupBy(e => e.Incidentstatus).Select(g => new
                    {
                        TotalMTTR = Math.Round(g.Sum(e => e.value)/g.Count(),2),
                    }).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult MTTRByDepartment(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay) {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2);
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => e.Department).Select(g => new
                    {
                        value = g.Sum(e => (Convert.ToDateTime(e.Finishtime) - Convert.ToDateTime(e.Repairtime)).TotalMinutes), //mins
                        count = g.Count(),
                        Department = g.Key,
                    }).Select(e => new
                    {
                        value = Math.Round( e.value / e.count,2),
                        item = e.Department,
                    }).OrderBy(e=>e.value).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult MTTRByWorkcell(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay) {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2);
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => e.Project).Select(g => new
                    {
                        value = g.Sum(e => (Convert.ToDateTime(e.Finishtime) - Convert.ToDateTime(e.Repairtime)).TotalMinutes) , //mins
                        count = g.Count(),
                        Project = g.Key,
                    }).Select(e => new
                    {
                        value = Math.Round( e.value / e.count,2),
                        item = e.Project,
                    }).OrderBy(e => e.value).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        #region MTBF
        public IActionResult TotalMTBF(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay) 
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2);
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => e.Incidentstatus).Select(g => new
                    {
                        endtime = g.Max(e => e.Finishtime),
                        starttime = g.Min(e => e.Occurtime),
                        count = g.Count(),
                    }).Select(e => new
                    {
                        TotalMTBF = Math.Round((Convert.ToDateTime(e.endtime)-Convert.ToDateTime(e.starttime)).TotalHours / e.count,2),
                    }).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult MTBFByDepartment(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay) 
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2);
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => e.Department).Select(g => new
                    {
                        endtime = g.Max(e => e.Finishtime),
                        starttime = g.Min(e => e.Occurtime),
                        count = g.Count(),
                        Department = g.Key,
                    }).Select(e => new
                    {
                        value = Math.Round( (Convert.ToDateTime(e.endtime) - Convert.ToDateTime(e.starttime)).TotalHours / e.count,2),
                        item = e.Department,
                    }).OrderBy(e => e.value).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult MTBFByWorkcell(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay) 
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2);
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => e.Project).Select(g => new
                    {
                        endtime = g.Max(e => e.Finishtime),
                        starttime = g.Min(e => e.Occurtime),
                        count = g.Count(),
                        Project = g.Key,
                    }).Select(e => new
                    {
                        value =Math.Round( (Convert.ToDateTime(e.endtime) - Convert.ToDateTime(e.starttime)).TotalHours / e.count,2),
                        item = e.Project,
                    }).OrderBy(e => e.value).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        #region 员工的平均repaire时间和次数
        public IActionResult EmployeeWorkEffiency(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2);
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => e.Respperson).Select(g => new
                    {
                        value = Math.Round(g.Sum(e => (Convert.ToDateTime(e.Finishtime) - Convert.ToDateTime(e.Repairtime)).TotalMinutes)/g.Count(),2),
                        count = g.Count(),
                        item = g.Key,
                    }).OrderByDescending(e=>e.count).ToList();
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
