using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowntimeSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Data.Common;
using DowntimeSystem.Models.HR;

namespace DowntimeSystem.Controllers
{
    public class Dashboard : Controller
    {
        private iccojabilContext _epdb = new iccojabilContext();
        private string[] contains = { "eCalling" }; //, "FPY", "Downtime System"
        private GregorianCalendar gc = new GregorianCalendar();
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

         
        #region MTTR & MTTA & MTBF 部分的数据查询 ,只统计已经 close 且维修人不是 Auto 的 DT 

        #region MTTR & MTTA
        /// <summary>
        /// 返回 MTTR 和 MTTA 具体数值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="departmentlist"></param>
        /// <param name="projectlist"></param>
        /// <param name="stationlist"></param>
        /// <param name="currentDay"></param>
        /// <param name="lastDay"></param>
        /// <returns></returns>
        public IActionResult MTTR_MTTA_Overview(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    try
                    {
                        var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                        if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                        if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                        if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                        if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                        var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                        var calcu = tmp.Select(e => new
                        {
                            mttr = (Convert.ToDateTime(e.Finishtime) - Convert.ToDateTime(e.Repairtime)).TotalMinutes, //mins
                            mtta = (Convert.ToDateTime(e.Repairtime) - Convert.ToDateTime(e.Occurtime)).TotalMinutes, //mins
                            Incidentstatus = e.Incidentstatus,
                        }).ToList();
                        var items = calcu.GroupBy(e => e.Incidentstatus).Select(g => new
                        {
                            TotalMTTR = Math.Round(g.Sum(e => e.mttr) / g.Count(), 2),
                            TotalMTTA = Math.Round(g.Sum(e => e.mtta) / g.Count(), 2),
                        }).ToList();
                        return Json(items);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IActionResult MTTR_MTTA_Detail_byTime(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay, string filterType)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    try
                    {
                        var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                        if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                        if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                        if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                        if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                        var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                        var calcu = tmp.Select(e => new
                        {
                            mttr = (Convert.ToDateTime(e.Finishtime) - Convert.ToDateTime(e.Repairtime)).TotalMinutes, //mins
                            mtta = (Convert.ToDateTime(e.Repairtime) - Convert.ToDateTime(e.Occurtime)).TotalMinutes, //mins
                            filterType = filterType == "Daily" ? e.Occurtime.ToString("yyyy-MM-dd") : filterType == "Weekly" ? "WK"+ gc.GetWeekOfYear(e.Occurtime, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString() : e.Occurtime.ToString("yyyy-MM"),
                        }).ToList();
                        var items = calcu.GroupBy(e => e.filterType).Select(g => new
                        {
                            filterType = g.Key,
                            target = 10,
                            mttr = Math.Round(g.Sum(e => e.mttr) / g.Count(), 2),
                            mtta = Math.Round(g.Sum(e => e.mtta) / g.Count(), 2),
                        }).OrderBy(e=>e.filterType).ToList();
                        return Json(items);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        ///按部门，项目，天/周/月，计算 MTTR, MTTA
        /// </summary>
        /// <param name="item"></param>
        /// <param name="departmentlist"></param>
        /// <param name="projectlist"></param>
        /// <param name="stationlist"></param>
        /// <param name="currentDay"></param>
        /// <param name="lastDay"></param>
        /// <returns></returns>
        public IActionResult MTTR_MTTA_Detail_ByDepartment_Time(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay,string filterType)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay))
                        .Select(e => new {
                            Department = e.Department,
                            Project = e.Project,
                            Finishtime = e.Finishtime,
                            Repairtime = e.Repairtime,
                            Downtime = e.Downtime,
                            Occurtime = e.Occurtime,
                            filterType = filterType == "Daily"? e.Occurtime.ToString("yyyy-MM-dd") : filterType == "Weekly" ? "WK"+gc.GetWeekOfYear(e.Occurtime, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString() : e.Occurtime.ToString("yyyy-MM"),
                        }).ToList();
                    var items = tmp.GroupBy(e => new { e.filterType, e.Department }).Select(g => new
                    {
                        MTTR =Math.Round(g.Sum(e => (Convert.ToDateTime(e.Finishtime) - Convert.ToDateTime(e.Repairtime)).TotalMinutes)/g.Count(),2), //mins
                        MTTA = Math.Round(g.Sum(e => (Convert.ToDateTime(e.Repairtime) - Convert.ToDateTime(e.Occurtime)).TotalMinutes)/g.Count(),2), //mins
                        target = 10,  //获取该项目的目标值
                        filterType = g.Key.filterType,
                        Department = g.Key.Department,
                    }).OrderBy(e => e.filterType).ToList();
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
        public IActionResult TotalMTTR(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson!="Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
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
        public IActionResult MTTRByDepartment(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay) {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
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
        public IActionResult MTTRByWorkcell(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay) {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => e.Project).Select(g => new
                    {
                        value = g.Sum(e => (Convert.ToDateTime(e.Finishtime) - Convert.ToDateTime(e.Repairtime)).TotalMinutes) , //mins
                        target = 10,  //获取该项目的目标值
                        count = g.Count(),
                        Project = g.Key,
                    }).Select(e => new
                    {
                        value = Math.Round( e.value / e.count,2),
                        item = e.Project,
                        target = e.target,  //获取该项目的目标值
                    }).OrderByDescending(e => e.value).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult MTTRByMonth(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay))
                        .Select(e=> new{
                            Month = e.Occurtime.ToString("yyyy-MM"),
                            Finishtime = e.Finishtime,
                            Repairtime = e.Repairtime,
                        }).ToList();
                    var items = tmp.GroupBy(e => e.Month).Select(g => new
                    {
                        value = g.Sum(e => (Convert.ToDateTime(e.Finishtime) - Convert.ToDateTime(e.Repairtime)).TotalMinutes), //mins
                        count = g.Count(),
                        target = 10,  //获取该项目的目标值
                        Month = g.Key,
                    }).Select(e => new
                    {
                        value = Math.Round(e.value / e.count, 2),
                        item = e.Month,
                        target = e.target,
                    }).OrderBy(e => e.item).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult MTTRByArea(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay))
                        .Select(e => new {
                            para = e.Project=="BE"?"BE" : e.Project == "SMT" ? "SMT" : e.Project == "CR" ? "CR" :"FATP",
                            Finishtime = e.Finishtime,
                            Repairtime = e.Repairtime,
                        }).ToList();
                    var items = tmp.GroupBy(e => e.para).Select(g => new
                    {
                        value = g.Sum(e => (Convert.ToDateTime(e.Finishtime) - Convert.ToDateTime(e.Repairtime)).TotalMinutes), //mins
                        count = g.Count(),
                        target = 10,  //获取该项目的目标值
                        para = g.Key,
                    }).Select(e => new
                    {
                        value = Math.Round(e.value / e.count, 2),
                        item = e.para,
                        target = e.target,
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

        #region MTTA
        public IActionResult TotalMTTA(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var calcu = tmp.Select(e => new
                    {
                        value = (Convert.ToDateTime(e.Repairtime) - Convert.ToDateTime(e.Occurtime)).TotalMinutes, //mins
                        Incidentstatus = e.Incidentstatus,
                    }).ToList();
                    var items = calcu.GroupBy(e => e.Incidentstatus).Select(g => new
                    {
                        TotalMTTA = Math.Round(g.Sum(e => e.value) / g.Count(), 2),
                    }).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult MTTAByDepartment(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => e.Department).Select(g => new
                    {
                        value = g.Sum(e => (Convert.ToDateTime(e.Repairtime) - Convert.ToDateTime(e.Occurtime)).TotalMinutes), //mins
                        count = g.Count(),
                        Department = g.Key,
                    }).Select(e => new
                    {
                        value = Math.Round(e.value / e.count, 2),
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
        public IActionResult MTTAByWorkcell(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => e.Project).Select(g => new
                    {
                        value = g.Sum(e => (Convert.ToDateTime(e.Repairtime) - Convert.ToDateTime(e.Occurtime)).TotalMinutes), //mins
                        count = g.Count(),
                        Project = g.Key,
                    }).Select(e => new
                    {
                        value = Math.Round(e.value / e.count, 2),
                        item = e.Project,
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

        #region MTBF ,mean time between failure, 按机台统计（所有的工作时间）如果时间长度大于一周 -1 ,大于一个月-5（4天休息，1天保养）
        public IActionResult TotalMTBF(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay) 
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    #region
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();

                    //查询出所有的work记录
                    var records = tmp.Select(e => new
                    {
                        Department = e.Department,
                        Project = e.Project,
                        Station = e.Station,
                        Machine = e.Machine,
                        startworkTime = e.Finishtime,
                        downtimeOccurtTime = Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) == DateTime.MinValue?
                        Convert.ToDateTime(currentDay) : Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) //当前设备下次发生downtime的事件
                    }).OrderBy(e=> e.Station).OrderBy (e=>e.Machine).ToList();

                    //计算正常运行时间
                    var items = records.Select(e => new
                    {
                        Department = e.Department,
                        Project = e.Project,
                        Station = e.Station,
                        Machine = e.Machine,
                        Workhours = (Convert.ToDateTime(e.downtimeOccurtTime) - Convert.ToDateTime(e.startworkTime)).TotalHours
                    }).Where(e => e.Workhours > 0).GroupBy(e => true).Select(g => new
                    {
                        count = g.Count(),
                        value = g.Sum(e => e.Workhours > 720 ? e.Workhours - 170 * (e.Workhours / 720) :
                         e.Workhours > 168 ? e.Workhours - 36 * (e.Workhours / 168) :
                         e.Workhours > 24 ? e.Workhours - 2 * (e.Workhours / 24) : e.Workhours)
                    }).Select(e => new { totalMTBF = Math.Round(e.value / e.count, 2) }).ToList();

                    #endregion
                    return  Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult MTBFByDepartment(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay) 
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    #region
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();

                    //查询出所有的work记录
                    var records = tmp.Select(e => new
                    {
                        Department = e.Department,
                        Project = e.Project,
                        Station = e.Station,
                        Machine = e.Machine,
                        startworkTime = e.Finishtime,
                        downtimeOccurtTime = Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) == DateTime.MinValue ?
                        Convert.ToDateTime(currentDay) : Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) //当前设备下次发生downtime的事件
                    }).OrderBy(e => e.Station).OrderBy(e => e.Machine).ToList();

                    //计算正常运行时间
                    var items = records.Select(e => new
                    {
                        Department = e.Department,
                        Project = e.Project,
                        Station = e.Station,
                        Machine = e.Machine,
                        Workhours = (Convert.ToDateTime(e.downtimeOccurtTime) - Convert.ToDateTime(e.startworkTime)).TotalHours
                    }).Where(e => e.Workhours > 0).GroupBy(e => e.Department).Select(g => new
                    {
                        Department = g.Key,
                        count = g.Count(),
                        value = g.Sum(e => e.Workhours > 720 ? e.Workhours - 170 * (e.Workhours / 720) :
                         e.Workhours > 168 ? e.Workhours - 36 * (e.Workhours / 168) :
                         e.Workhours > 24 ? e.Workhours - 2 * (e.Workhours / 24) : e.Workhours)
                    }).Select(e => new
                    { 
                        value = Math.Round(e.value / e.count, 2),
                        item = e.Department,
                    }).OrderBy(e=>e.value).ToList();

                    #endregion
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult MTBFByWorkcell(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay) 
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    #region
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();

                    //查询出所有的work记录
                    var records = tmp.Select(e => new
                    {
                        Department = e.Department,
                        Project = e.Project,
                        Station = e.Station,
                        Machine = e.Machine,
                        startworkTime = e.Finishtime,
                        downtimeOccurtTime = Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) == DateTime.MinValue ?
                        Convert.ToDateTime(currentDay) : Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) //当前设备下次发生downtime的事件
                    }).OrderBy(e => e.Station).OrderBy(e => e.Machine).ToList();

                    //计算正常运行时间
                    var items = records.Select(e => new
                    {
                        Department = e.Department,
                        Project = e.Project,
                        Station = e.Station,
                        Machine = e.Machine,
                        Workhours = (Convert.ToDateTime(e.downtimeOccurtTime) - Convert.ToDateTime(e.startworkTime)).TotalHours
                    }).Where(e => e.Workhours > 0).GroupBy(e => e.Project).Select(g => new
                    {
                        Project = g.Key,
                        count = g.Count(),
                        target = 10, // 获取该项目的目标值
                        value = g.Sum(e => e.Workhours > 720 ? e.Workhours - 170 * (e.Workhours / 720) :
                         e.Workhours > 168 ? e.Workhours - 36 * (e.Workhours / 168) :
                         e.Workhours > 24 ? e.Workhours - 2 * (e.Workhours / 24) : e.Workhours)
                    }).Select(e => new
                    {
                        value = Math.Round(e.value / e.count, 2),
                        item = e.Project,
                        target =e.target, // 获取该项目的目标值
                    }).OrderByDescending(e => e.value).ToList();

                    #endregion
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult MTBFByMonth(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    #region
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();

                    //查询出所有的work记录
                    var records = tmp.Select(e => new
                    {
                        Department = e.Department,
                        Project = e.Project,
                        Station = e.Station,
                        Machine = e.Machine,
                        startworkTime = e.Finishtime,
                        downtimeOccurtTime = Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) == DateTime.MinValue ?
                        Convert.ToDateTime(currentDay) : Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) //当前设备下次发生downtime的事件
                    }).OrderBy(e => e.Station).OrderBy(e => e.Machine).ToList();

                    //计算正常运行时间
                    var items = records.Select(e => new
                    {
                        Department = e.Department,
                        Project = e.Project,
                        Station = e.Station,
                        Machine = e.Machine,
                        Month = e.downtimeOccurtTime.ToString("yyyy-MM"),
                        Workhours = (Convert.ToDateTime(e.downtimeOccurtTime) - Convert.ToDateTime(e.startworkTime)).TotalHours
                    }).Where(e => e.Workhours > 0).GroupBy(e => e.Month).Select(g => new
                    {
                        Month = g.Key,
                        count = g.Count(),
                        target = 10, // 获取各项目的每月目标值总和
                        value = g.Sum(e => e.Workhours > 720 ? e.Workhours - 170 * (e.Workhours / 720) :
                         e.Workhours > 168 ? e.Workhours - 36 * (e.Workhours / 168) :
                         e.Workhours > 24 ? e.Workhours - 2 * (e.Workhours / 24) : e.Workhours)
                    }).Select(e => new
                    {
                        value = Math.Round(e.value / e.count, 2),
                        item = e.Month,
                        target = e.target,
                    }).OrderBy(e => e.item).ToList();

                    #endregion
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        //按部门，项目，天，计算MTBF ,和total Downtime
        public IActionResult MTBF_Detail_ByDepartment_Time(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay, string filterType)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    #region
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();

                    //查询出所有的work记录  非常慢
                    var records = tmp.Select(e => new
                    {
                        Department = e.Department,
                        Project = e.Project,
                        Station = e.Station,
                        Machine = e.Machine,
                        startworkTime = e.Finishtime,
                        downtimeOccurtTime = Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) == DateTime.MinValue ?
                        Convert.ToDateTime(currentDay) : Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) //当前设备下次发生downtime的事件
                    }).OrderBy(e => e.Station).OrderBy(e => e.Machine).ToList();

                    //计算正常运行时间
                    var items = records.Select(e => new
                    {
                        Department = e.Department,
                        Project = e.Project,
                        Station = e.Station,
                        Machine = e.Machine,
                        filterType = filterType == "Daily" ? e.downtimeOccurtTime.ToString("yyyy-MM-dd") : filterType == "Weekly" ?"WK"+ gc.GetWeekOfYear(e.downtimeOccurtTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString() : e.downtimeOccurtTime.ToString("yyyy-MM"),
                        Workhours = (Convert.ToDateTime(e.downtimeOccurtTime) - Convert.ToDateTime(e.startworkTime)).TotalHours
                    }).Where(e => e.Workhours > 0).GroupBy(e =>new { e.filterType,e.Department }).Select(g => new
                    {
                        filterType = g.Key.filterType,
                        Department = g.Key.Department,
                        count = g.Count(),
                        target = 10, // 获取各项目的每月目标值总和
                        value = g.Sum(e => e.Workhours > 720 ? e.Workhours - 170 * (e.Workhours / 720) :
                         e.Workhours > 168 ? e.Workhours - 36 * (e.Workhours / 168) :
                         e.Workhours > 24 ? e.Workhours - 2 * (e.Workhours / 24) : e.Workhours)
                    }).Select(e => new
                    {
                        value = Math.Round(e.value / e.count, 2),
                        filterType = e.filterType,
                        Department = e.Department,
                        target = e.target,
                    }).OrderBy(e => e.filterType).ToList();

                    #endregion
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult MTBF_Detail_ByTime(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay, string filterType)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    #region
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();

                    //查询出所有的work记录  非常慢
                    var records = tmp.Select(e => new
                    {
                        Station = e.Station,
                        Machine = e.Machine,
                        startworkTime = e.Finishtime,
                        downtimeOccurtTime = Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) == DateTime.MinValue ?
                        Convert.ToDateTime(currentDay) : Convert.ToDateTime(tmp.Where(j => j.Machine.Equals(e.Machine) & j.Id > e.Id).OrderBy(j => j.Id).Select(j => j.Occurtime).FirstOrDefault()) //当前设备下次发生downtime的事件
                    }).OrderBy(e => e.Station).OrderBy(e => e.Machine).ToList();

                    //计算正常运行时间
                    var items = records.Select(e => new
                    {
                        Station = e.Station,
                        Machine = e.Machine,
                        filterType = filterType == "Daily" ? e.downtimeOccurtTime.ToString("yyyy-MM-dd") : filterType == "Weekly" ? "WK"+gc.GetWeekOfYear(e.downtimeOccurtTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString() : e.downtimeOccurtTime.ToString("yyyy-MM"),
                        Workhours = (Convert.ToDateTime(e.downtimeOccurtTime) - Convert.ToDateTime(e.startworkTime)).TotalHours
                    }).Where(e => e.Workhours > 0).GroupBy(e => e.filterType).Select(g => new
                    {
                        filterType = g.Key,
                        count = g.Count(),
                        target = 10, // 获取各项目的每月目标值总和
                        value = g.Sum(e => e.Workhours > 720 ? e.Workhours - 170 * (e.Workhours / 720) :
                         e.Workhours > 168 ? e.Workhours - 36 * (e.Workhours / 168) :
                         e.Workhours > 24 ? e.Workhours - 2 * (e.Workhours / 24) : e.Workhours)
                    }).Select(e => new
                    {
                        value = Math.Round(e.value / e.count, 2),
                        filterType = e.filterType,
                        target = e.target,
                    }).OrderBy(e => e.filterType).ToList();

                    #endregion
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        #endregion

        #region 员工的平均repair时间 和 次数 和 总计维修时间，,只统计已经 close 且维修人不是 Auto 的 DT
        public IActionResult EmployeeWorkEffiency(IncidentDet item, string[] departmentlist, string[] projectlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) & e.Incidentstatus == 2 & e.Respperson != "Auto");
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => e.Respperson).Select(g => new
                    {
                        value = Math.Round(g.Sum(e => (Convert.ToDateTime(e.Finishtime) - Convert.ToDateTime(e.Repairtime)).TotalMinutes) / g.Count(), 2),
                        totalValue = Math.Round(g.Sum(e => (Convert.ToDateTime(e.Finishtime) - Convert.ToDateTime(e.Repairtime)).TotalMinutes), 2),
                        count = g.Count(),
                        item = g.Key,
                    }).OrderByDescending(e => e.count).ToList();
                    var result = items.Join(_epdb.EmpViewForTes, dt => dt.item, ep => ep.Empid, (dt, ep) => new
                    {
                        value = dt.value,
                        totalValue = dt.totalValue,
                        count = dt.count,
                        item = ep.ChineseName,
                    }).ToList();
                    return Json(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        #region Downtime (频率) Distribution by Status
        public IActionResult GetCountGroupByDowntimeStatus(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom));
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => e.Actionstatus).Select(g => new
                    {
                        name = g.Key == 0 ? "New Open" : g.Key == 1 ? "Issue Fix" : g.Key == 2 ? "Closed" : "RC&CA",
                        value = g.Count()
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

        #region Downtime (时间、频率) Distribution by Area
        /// <summary>
        /// Area as('FATP','BE','SMT','CR')
        /// </summary>
        /// <param name="item"></param>
        /// <param name="departmentlist"></param>
        /// <param name="projectlist"></param>
        /// <param name="currentDay"></param>
        /// <param name="lastDay"></param>
        /// <returns></returns>
        public IActionResult GetDTDistribution_ByArea(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) ); //&e.Incidentstatus == 2 & e.Respperson != "Auto"
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));

                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay))
                        .Select(e => new {
                            Project = e.Project == "SMT" ? "SMT" : e.Project == "BE" ? "BE" : e.Project == "CR" ? "CR" : "FATP",
                            Downtime = e.Incidentstatus == 2? e.Downtime : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds),
                        }).ToList();
                    var items = tmp.GroupBy(e => e.Project).Select(g => new
                    {
                        item = g.Key,
                        count = g.Count(),
                        dt = Math.Round(Convert.ToDouble(g.Sum(e => e.Downtime / 3600) ), 2),
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

        #region Downtime (时间、频率) Distribution by Department & Status
        /// <summary>
        /// Area as('FATP','BE','SMT','CR')
        /// </summary>
        /// <param name="item"></param>
        /// <param name="departmentlist"></param>
        /// <param name="projectlist"></param>
        /// <param name="currentDay"></param>
        /// <param name="lastDay"></param>
        /// <returns></returns>
        public IActionResult GetDTDistribution_ByDepartment_Status(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom)); //&e.Incidentstatus == 2 & e.Respperson != "Auto"
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));

                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay))
                        .Select(e => new {
                            Department = e.Department,
                            Actionstatus = e.Actionstatus,
                            Downtime = e.Incidentstatus == 2 ? e.Downtime : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds),
                        }).ToList();
                    var items = tmp.GroupBy(e =>new { e.Department,e.Actionstatus }).Select(g => new
                    {
                        count = g.Count(),
                        Department = g.Key.Department,
                        status = g.Key.Actionstatus == 0?"New Open" : g.Key.Actionstatus == 1?"Issue Fix" : g.Key.Actionstatus == 1 ? "DT Close" :"RC&CA",
                        dt = Math.Round(Convert.ToDouble(g.Sum(e => e.Downtime / 3600) ), 2),
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

        #region Downtime (时间、频率) Distribution by Project & Department
        public IActionResult GetDTDistribution_ByDepartment_Project(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) ); //& e.Incidentstatus == 2 & e.Respperson != "Auto"
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => new { e.Department, e.Project }).Select(g => new
                    {
                        dt = Math.Round(Convert.ToDecimal(g.Sum(e =>e.Incidentstatus == 2? e.Downtime / 3600 : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds) / 3600)), 2),
                        Department = g.Key.Department,
                        Project = g.Key.Project,
                        count = g.Count()
                    }).OrderBy(e => e.Department).OrderBy(e => e.Project).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        #region Downtime (时间、频率) Distribution by Month
        public IActionResult GetDTDistribution_ByMonth(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom)); //& e.Incidentstatus == 2 & e.Respperson != "Auto"
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay))
                        .Select(e=>new { 
                            month = e.Occurtime.ToString("yyyy-MM"),
                            Downtime =  e.Incidentstatus == 2 ? e.Downtime : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds)
                        }).ToList();
                    var items = tmp.GroupBy(e => e.month).Select(g => new
                    {
                        dt = Math.Round(Convert.ToDouble(g.Sum(e => e.Downtime/3600)), 2),
                        month = g.Key,
                        count = g.Count()
                    }).OrderBy(e => e.month).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        #region Total Downtime (时间)
        public IActionResult GetTotalDT(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom));
                    if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                    if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                    if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                    if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                    var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var items = tmp.GroupBy(e => true).Select(g => new
                    {
                        value = Math.Round(Convert.ToDecimal(g.Sum(e =>e.Incidentstatus==2? e.Downtime / 3600 : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds) / 3600)) , 0),
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



        #region Downtime (时间，频率) Distribution by Station & Time
        public IActionResult GetDTDistribution_byStation_Time(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay, string filterType)
        {
                using (ECContext db = new ECContext())
                {
                    try
                    {
                        var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom) ); //& e.Incidentstatus == 2 & e.Respperson != "Auto"
                        if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                        if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                        if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                        if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                        var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                        var calcu = tmp.Select(e => new
                        {
                            Station = e.Station,
                            Downtime = e.Incidentstatus == 2 ? e.Downtime : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds),
                            filterType = filterType == "Daily" ? e.Occurtime.ToString("yyyy-MM-dd") : filterType == "Weekly" ? "WK"+gc.GetWeekOfYear(e.Occurtime, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString() : e.Occurtime.ToString("yyyy-MM"),
                        }).ToList();
                        var items = calcu.GroupBy(e =>new { e.filterType ,e.Station}).Select(g => new
                        {
                            filterType = g.Key.filterType,
                            station = g.Key.Station,
                            dt =Math.Round( Convert.ToDouble(g.Sum(e=>e.Downtime/3600)),2),
                            count = g.Count()
                        }).OrderBy(e => e.filterType).ToList();   //.OrderByDescending(e=>e.dt)
                    return Json(items);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
        }
        #endregion

        #region Downtime (时间，频率) Distribution by Defect Code & Time
        public IActionResult GetDTDistribution_byDefectCode_Time(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay, string filterType)
        {
                using (ECContext db = new ECContext())
                {
                    try
                    {
                        var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom)); //& e.Incidentstatus == 2 & e.Respperson != "Auto"
                        if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                        if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                        if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                        if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                        var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                        var calcu = tmp.Select(e => new
                        {
                            issue = e.Issue,
                            Downtime = e.Incidentstatus == 2 ? e.Downtime : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds),
                            filterType = filterType == "Daily" ? e.Occurtime.ToString("yyyy-MM-dd") : filterType == "Weekly" ?"WK"+ gc.GetWeekOfYear(e.Occurtime, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString() : e.Occurtime.ToString("yyyy-MM"),
                        }).ToList();
                        var items = calcu.GroupBy(e => new { e.filterType, e.issue }).Select(g => new
                        {
                            filterType = g.Key.filterType,
                            issue = g.Key.issue,
                            dt = Math.Round(Convert.ToDouble(g.Sum(e => e.Downtime / 3600)), 2),
                            count = g.Count()
                        }).OrderBy(e => e.filterType).ToList(); //OrderByDescending(e => e.dt).
                    return Json(items);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
        }
        #endregion

        #region Downtime (时间，频率) Distribution by Line & Time
        public IActionResult GetDTDistribution_byLine_Time(IncidentDet item, string[] departmentlist, string[] projectlist, string[] stationlist, string currentDay, string lastDay, string filterType)
        {
                using (ECContext db = new ECContext())
                {
                    try
                    {
                        var where = db.IncidentDets.Where(e => contains.Contains(e.Comefrom)); //& e.Incidentstatus == 2 & e.Respperson != "Auto"
                        if (projectlist.Length > 0) where = where.Where(e => projectlist.Contains(e.Project));
                        if (departmentlist.Length > 0) where = where.Where(e => departmentlist.Contains(e.Department));
                        if (stationlist.Length > 0) where = where.Where(e => stationlist.Contains(e.Station));
                        if (!string.IsNullOrEmpty(item.Comefrom)) where = where.Where(e => e.Comefrom.Equals(item.Comefrom));
                        var tmp = where.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    var calcu = tmp.Select(e => new
                        {
                            Line = e.Line,
                            Downtime = e.Incidentstatus == 2 ? e.Downtime : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds),
                            filterType = filterType == "Daily" ? e.Occurtime.ToString("yyyy-MM-dd") : filterType == "Weekly" ? "WK" + gc.GetWeekOfYear(e.Occurtime, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString() : e.Occurtime.ToString("yyyy-MM"),
                        }).ToList();
                        var items = calcu.GroupBy(e => new { e.filterType, e.Line }).Select(g => new
                        {
                            filterType = g.Key.filterType,
                            line = g.Key.Line,
                            dt = Math.Round(Convert.ToDouble(g.Sum(e => e.Downtime / 3600)), 2),
                            count = g.Count()
                        }).OrderBy(e => e.filterType).ToList(); //OrderByDescending(e => e.dt).
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
