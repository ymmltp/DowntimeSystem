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
        #region 获取Error Code 最高频次的前五项
        public IActionResult GetTopErrorCode_ByCount(IncidentDet item, string[] projectlist, string currentDay, string lastDay , int limit =5)
        {
            using (ECContext db = new ECContext()) {
                try {
                    var tmp = db.IncidentDets.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length>0)  tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
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
        public IActionResult GetLine_ByErrorCode(IncidentDet item, string[] projectlist, string currentDay, string lastDay, string errorcode, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e =>  Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
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
        public IActionResult GetStation_ByLine(IncidentDet item, string[] projectlist, string currentDay, string lastDay, string errorcode, string line, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e =>Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)) .ToList();
                    if (projectlist.Length > 0) tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
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
        public IActionResult GetRootCause_ByStation(IncidentDet item, string[] projectlist, string currentDay, string lastDay, string errorcode, string line, string station, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)
                        tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
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
        public IActionResult OpenClose_ByCount(IncidentDet item,string[] projectlist, string currentDay, string lastDay, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)
                        tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = tmp.GroupBy(e => e.Incidentstatus)
                        .Select(g => new
                        {
                            item = g.Key == 2 ? "Close" : g.Key == 1? "OnGoing" : "Open",
                            value = g.Key == 2 ?g.Sum(e=>e.Downtime) / 3600 :g.Sum(e=>Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds)) / 3600,
                            //value = g.Key == 2 ?Math.Round(Convert.ToDecimal(g.Sum(e=>e.Downtime) / 3600),2) : Math.Round(Convert.ToDecimal(g.Sum(e=>Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds)) / 3600),2),
                        }).OrderByDescending(e => e.value).Take(limit).ToList();
                    return Json(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        public IActionResult OpenClose_Items(IncidentDet item, string[] projectlist, string currentDay, string lastDay,string status )
        { 
            using (ECContext db = new ECContext())
            {
                try
                {

                    var tmp = db.IncidentDets.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)
                        tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    int incidentstatus = status == "Close" ? 2 : status == "Open" ? 0 : 1;
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
        #endregion

        #region  (不使用)获取RootCause ,downtime最高的前五项
        public IActionResult GetTopRootCause_ByDowntime(IncidentDet item, string[] projectlist, string currentDay, string lastDay, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)
                        tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = db.IncidentDets.GroupBy(e => e.Rootcause).Select(g => new
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
        public IActionResult GetTopRootCause_ByDowntime_ErrorCode(IncidentDet item, string[] projectlist, string rootcause, string currentDay, string lastDay, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)
                        tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = db.IncidentDets.Where(e => e.Rootcause.Equals(rootcause) ).GroupBy(e => e.Issue).Select(g => new
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
        public IActionResult GetAllErrorCode_ByCount(IncidentDet item, string[] projectlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)
                        tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = db.IncidentDets.GroupBy(e => e.Issue).Select(g => new
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
        public IActionResult GetTopRootCause_ByErrorCode(IncidentDet item, string[] projectlist,string errorcode, string currentDay, string lastDay, int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.Where(e => Convert.ToDateTime(lastDay) < e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).ToList();
                    if (projectlist.Length > 0)
                        tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = db.IncidentDets.Where(e=>e.Issue.Equals(errorcode)).GroupBy(e => e.Rootcause).Select(g => new
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
        public IActionResult GetTopDowntime_ByStation(IncidentDet item, string[] projectlist, string currentDay, string lastDay,int limit = 5)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.ToList();
                    if (projectlist.Length > 0)
                        tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
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
        public IActionResult GetTopDowntime_ByDepartment(IncidentDet item, string[] projectlist, string currentDay, string lastDay)
        {
            using (ECContext db = new ECContext())
            {
                try
                {
                    var tmp = db.IncidentDets.ToList();
                    if (projectlist.Length > 0) tmp = tmp.Where(e => projectlist.Contains(e.Project)).ToList();
                    if (!string.IsNullOrEmpty(item.Department)) tmp = tmp.Where(e => e.Department.Equals(item.Department)).ToList();
                    if (!string.IsNullOrEmpty(item.Comefrom)) tmp = tmp.Where(e => e.Comefrom.Equals(item.Comefrom)).ToList();
                    var items = tmp.Where(e => Convert.ToDateTime(lastDay)< e.Occurtime & e.Occurtime < Convert.ToDateTime(currentDay)).GroupBy(e => e.Department).Select(g => new
                    {
                        item = g.Key,
                        value = g.Sum(e => e.Incidentstatus == 2 ? e.Downtime : Convert.ToInt32((DateTime.Now - e.Occurtime).TotalSeconds)) / 3600,
                        //value = g.Sum(e => e.Downtime) / 3600,
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
