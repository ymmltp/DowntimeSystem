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
        private string[] contains = { "eCalling", "Sparepart", "FPY", "Downtime System" };
        //查询EC表单
        [HttpGet]
        public IActionResult GetDowntimeList(IncidentDet tmp,string starttime ,string endtime)
        {
            try {
                using (ECContext db = new ECContext())
                {
                    List<IncidentDet> items = db.IncidentDets.Where(e => e.Calcdowntime == true).ToList(); 
                    if (!string.IsNullOrEmpty(tmp.Project)) items = items.Where(e => e.Project.Equals(tmp.Project)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Department)) items = items.Where(e => e.Department.Equals(tmp.Department)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Line)) items = items.Where(e => e.Line.Equals(tmp.Line)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Station)) items = items.Where(e => e.Station.Equals(tmp.Station)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Comefrom)) items = items.Where(e => e.Comefrom.Equals(tmp.Comefrom)).ToList();

                    if (!string.IsNullOrEmpty(starttime)) items = items.Where(e => e.Occurtime >= Convert.ToDateTime(starttime)).ToList();
                    if (!string.IsNullOrEmpty(endtime)) items = items.Where(e => e.Occurtime <= Convert.ToDateTime(endtime)).ToList();

                    if (!string.IsNullOrEmpty(tmp.Incidentstatus.ToString())) items = items.Where(e => e.Incidentstatus.Equals(tmp.Incidentstatus)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Actionstatus.ToString())) items = items.Where(e => e.Actionstatus.Equals(tmp.Actionstatus)).ToList();
                    return Json(items.OrderBy(e => e.Incidentstatus).OrderBy(e=>e.Actionstatus).ToList()) ;
                }
            }
            catch (Exception ex) {
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
                    List<IncidentDet> items = db.IncidentDets.Where(e => e.Calcdowntime == true).ToList();
                    if (!string.IsNullOrEmpty(tmp.Project)) items = items.Where(e => e.Project.Equals(tmp.Project)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Department)) items = items.Where(e => e.Department.Equals(tmp.Department)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Line)) items = items.Where(e => e.Line.Equals(tmp.Line)).ToList();
                    return Json(items.OrderBy(e=>e.Project).Select(e=>e.Project).Distinct().ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetLine(IncidentDet tmp)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    List<IncidentDet> items = db.IncidentDets.Where(e => e.Calcdowntime == true).ToList();
                    if (!string.IsNullOrEmpty(tmp.Project)) items = items.Where(e => e.Project.Equals(tmp.Project)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Department)) items = items.Where(e => e.Department.Equals(tmp.Department)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Line)) items = items.Where(e => e.Line.Equals(tmp.Line)).ToList();
                    return Json(items.OrderBy(e => e.Line).Select(e => e.Line).Distinct().ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetStation(IncidentDet tmp)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    List<IncidentDet> items = db.IncidentDets.Where(e => e.Calcdowntime == true).ToList();
                    if (!string.IsNullOrEmpty(tmp.Project)) items = items.Where(e => e.Project.Equals(tmp.Project)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Department)) items = items.Where(e => e.Department.Equals(tmp.Department)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Line)) items = items.Where(e => e.Line.Equals(tmp.Line)).ToList();
                    return Json(items.OrderBy(e => e.Station).Select(e => e.Station).Distinct().ToList());
                }
            }
            catch (Exception ex)
            {
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
                    List<IncidentDet> items = db.IncidentDets.Where(e => e.Calcdowntime == true).ToList();
                    return Json(items.OrderBy(e => e.Department).Select(e => e.Department).Distinct().ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //编辑EC表单
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
    }
}
