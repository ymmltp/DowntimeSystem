using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowntimeSystem.Models;

namespace DowntimeSystem.Controllers
{
    public class EC : Controller
    {
        #region 查询EC表单
        [HttpGet]
        public IActionResult GetDowntimeList(IncidentDet tmp,string starttime ,string endtime)
        {
            try {
                using (ECContext db = new ECContext())
                {
                    var where = db.IncidentDets.Where(e => e.Calcdowntime == true); 
                    if (!string.IsNullOrEmpty(tmp.Project)) where = where.Where(e => e.Project.Equals(tmp.Project));
                    if (!string.IsNullOrEmpty(tmp.Department)) where = where.Where(e => e.Department.Equals(tmp.Department));
                    if (!string.IsNullOrEmpty(tmp.Line)) where = where.Where(e => e.Line.Equals(tmp.Line));
                    if (!string.IsNullOrEmpty(tmp.Station)) where = where.Where(e => e.Station.Equals(tmp.Station));
                    if (!string.IsNullOrEmpty(tmp.Comefrom)) where = where.Where(e => e.Comefrom.Equals(tmp.Comefrom));
                    if (tmp.Id!=0) where = where.Where(e => e.Id.Equals(tmp.Id));
                    if (!string.IsNullOrEmpty(tmp.Respperson)) where = where.Where(e => e.Respperson.Equals(tmp.Respperson));
                    if (!string.IsNullOrEmpty(starttime)) where = where.Where(e => e.Occurtime >= Convert.ToDateTime(starttime));
                    if (!string.IsNullOrEmpty(endtime)) where = where.Where(e => e.Occurtime <= Convert.ToDateTime(endtime));
                    if (!string.IsNullOrEmpty(tmp.Incidentstatus.ToString())) where = where.Where(e => e.Incidentstatus.Equals(tmp.Incidentstatus));
                    if (!string.IsNullOrEmpty(tmp.Actionstatus.ToString())) where = where.Where(e => e.Actionstatus.Equals(tmp.Actionstatus));
                    var items = where.ToList();
                    return Json(items.OrderBy(e => e.Incidentstatus).OrderBy(e=>e.Actionstatus).ToList()) ;
                }
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetDepartment(IncidentDet tmp)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                   var items = db.IncidentDets.Where(e => e.Calcdowntime == true);
                    return Json(items.Select(e => e.Department).Distinct().ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetProject(IncidentDet tmp)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    var items = db.IncidentDets.Where(e => e.Calcdowntime == true);
                    if (!string.IsNullOrEmpty(tmp.Project)) items = items.Where(e => e.Project.Equals(tmp.Project));
                    if (!string.IsNullOrEmpty(tmp.Department)) items = items.Where(e => e.Department.Equals(tmp.Department));
                    if (!string.IsNullOrEmpty(tmp.Line)) items = items.Where(e => e.Line.Equals(tmp.Line));
                    return Json(items.Select(e=>e.Project).Distinct().OrderBy(e => e).ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }      
        [HttpGet]
        public IActionResult GetLine(IncidentDet tmp, string[] departmentList, string[] projectList)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    var where = db.IncidentDets.Where(e => e.Calcdowntime == true);
                    if (!string.IsNullOrEmpty(tmp.Project)) where = where.Where(e => e.Project.Equals(tmp.Project));
                    if (!string.IsNullOrEmpty(tmp.Department)) where = where.Where(e => e.Department.Equals(tmp.Department));
                    if (!string.IsNullOrEmpty(tmp.Line)) where = where.Where(e => e.Line.Equals(tmp.Line));
                    if (projectList.Length > 0) where = where.Where(e => projectList.Contains(e.Project));
                    if (departmentList.Length > 0) where = where.Where(e => departmentList.Contains(e.Department));
                    return Json(where.Select(e => e.Line).Distinct().OrderBy(e => e).ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetStation(IncidentDet tmp, string[] departmentList,string[] projectList,string[] lineList)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    var where = db.IncidentDets.Where(e => e.Calcdowntime == true);
                    if (!string.IsNullOrEmpty(tmp.Project)) where = where.Where(e => e.Project.Equals(tmp.Project));
                    if (!string.IsNullOrEmpty(tmp.Department)) where = where.Where(e => e.Department.Equals(tmp.Department));
                    if (!string.IsNullOrEmpty(tmp.Line)) where = where.Where(e => e.Line.Equals(tmp.Line));
                    if (projectList.Length>0) where = where.Where(e => projectList.Contains(e.Project));
                    if (departmentList.Length > 0) where = where.Where(e => departmentList.Contains(e.Department));
                    if (lineList.Length > 0) where = where.Where(e => lineList.Contains(e.Line));
                    return Json(where.Select(e => e.Station).Distinct().OrderBy(e => e).ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetFixture(IncidentDet tmp, string[] departmentList, string[] projectList, string[] lineList, string[] stationList)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    var where = db.IncidentDets.Where(e => e.Calcdowntime == true);
                    if (!string.IsNullOrEmpty(tmp.Project)) where = where.Where(e => e.Project.Equals(tmp.Project));
                    if (!string.IsNullOrEmpty(tmp.Department)) where = where.Where(e => e.Department.Equals(tmp.Department));
                    if (!string.IsNullOrEmpty(tmp.Line)) where = where.Where(e => e.Line.Equals(tmp.Line));
                    if (projectList.Length > 0) where = where.Where(e => projectList.Contains(e.Project));
                    if (departmentList.Length > 0) where = where.Where(e => departmentList.Contains(e.Department));
                    if (lineList.Length > 0) where = where.Where(e => lineList.Contains(e.Line));
                    if (stationList.Length > 0) where = where.Where(e => stationList.Contains(e.Station));
                    return Json(where.Select(e => e.Machine).Distinct().OrderBy(e => e).ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region 编辑EC表单
        [HttpPost]
        public IActionResult CreateDowmtimeIncident(IncidentDet item)
        {
            if (string.IsNullOrEmpty(item.Issue))
            {
                return BadRequest("Please input you issue...");
            }
            else {
                try
                {
                    using (ECContext db = new ECContext())
                    {
                        db.IncidentDets.Add(item);
                        db.SaveChanges();
                        return Json(true);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpPost]
        public IActionResult StartRepaire(int id, string name)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    IncidentDet tmp =  db.IncidentDets.Find(id);
                    tmp.Incidentstatus = 1;
                    tmp.Respperson = name;
                    tmp.Actionstatus = 1;
                    tmp.Repairtime = DateTime.Now;
                    db.SaveChanges();
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }
        [HttpPost]
        public IActionResult CloseRepaire(int id)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    DateTime Current = DateTime.Now;
                    IncidentDet tmp = db.IncidentDets.Find(id);
                    tmp.Incidentstatus = 2;
                    tmp.Actionstatus = 2;
                    tmp.Finishtime = Current;
                    tmp.Downtime = Convert.ToInt32( (Current - tmp.Occurtime).TotalSeconds);
                    db.SaveChanges();
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult RCCA(IncidentDet item)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    IncidentDet tmp = db.IncidentDets.Find(item.Id);
                    tmp.Station = item.Station;
                    tmp.Respperson = item.Respperson;
                    tmp.Machine = item.Machine;
                    tmp.Issue = item.Issue;
                    tmp.Issueremark = item.Issueremark;
                    tmp.Calcdowntime = item.Calcdowntime;
                    tmp.Labor = item.Labor;
                    tmp.Actionstatus = 3;
                    tmp.Rootcause = item.Rootcause;
                    tmp.Rootcauseremark = item.Rootcauseremark;
                    tmp.Action = item.Action;
                    tmp.Actionremark = item.Actionremark;
                    db.SaveChanges();
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
