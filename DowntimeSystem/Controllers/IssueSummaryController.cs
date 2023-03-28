using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowntimeSystem.Models;

namespace DowntimeSystem.Controllers
{
    public class IssueSummaryController : Controller
    {
        #region 查询 Issue Summary 
        //All
        [HttpGet]
        public IActionResult GetIssueSummary(IssueSummary tmp)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    var where = db.IssueSummaryAlls.Where(e=>true);
                    if (!string.IsNullOrEmpty(tmp.Project)) where = where.Where(e => e.Project.Equals(tmp.Project));
                    if (!string.IsNullOrEmpty(tmp.Department)) where = where.Where(e => e.Department.Equals(tmp.Department));
                    if (!string.IsNullOrEmpty(tmp.Line)) where = where.Where(e => e.Line.Equals(tmp.Line));
                    if (!string.IsNullOrEmpty(tmp.Station)) where = where.Where(e => e.Station.Equals(tmp.Station));
                    if (!string.IsNullOrEmpty(tmp.Issue)) where = where.Where(e => e.Issue.Equals(tmp.Issue));
                    if (!string.IsNullOrEmpty(tmp.Rootcause)) where = where.Where(e => e.Rootcause.Equals(tmp.Rootcause));
                    var items = where.ToList();
                    return Json(items.OrderByDescending(e=>e.Qty));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Weekly
        [HttpGet]
        public IActionResult GetIssueWeekly(IssueSummary tmp)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    var where = db.IssueSummaries.Where(e => true);
                    if (!string.IsNullOrEmpty(tmp.Project)) where = where.Where(e => e.Project.Equals(tmp.Project));
                    if (!string.IsNullOrEmpty(tmp.Department)) where = where.Where(e => e.Department.Equals(tmp.Department));
                    if (!string.IsNullOrEmpty(tmp.Line)) where = where.Where(e => e.Line.Equals(tmp.Line));
                    if (!string.IsNullOrEmpty(tmp.Station)) where = where.Where(e => e.Station.Equals(tmp.Station));
                    if (!string.IsNullOrEmpty(tmp.Issue)) where = where.Where(e => e.Issue.Equals(tmp.Issue));
                    if (!string.IsNullOrEmpty(tmp.Rootcause)) where = where.Where(e => e.Rootcause.Equals(tmp.Rootcause));
                    if (!string.IsNullOrEmpty(tmp.Week)) where = where.Where(e => e.Week.Equals(tmp.Week));
                    var items = where.ToList();
                    return Json(items.OrderByDescending(e => e.Qty));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region 编辑 Issue Summary 
        [HttpPost]
        public IActionResult CreateIssueSummary(IssueSummary item)
        {
            if (string.IsNullOrEmpty(item.Issue)) {
                return BadRequest("Please input you issue...");
            }
            try
            {
                using (ECContext db = new ECContext())
                {
                    if (string.IsNullOrEmpty(item.Qty.ToString()))  item.Qty = 1;             
                    db.IssueSummaries.Add(item);
                    db.SaveChanges();
                    return Json(item.Id);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult EditIssueSummary_Action(IssueSummary tmp)
        {
            if (string.IsNullOrEmpty(Convert.ToString(tmp.Id)))
            {
                return BadRequest("Please select one issue to modify ");
            }
            try
            {
                using (ECContext db = new ECContext())
                {
                    IssueSummary item = db.IssueSummaries.Find(tmp.Id);
                    if (!string.IsNullOrEmpty(tmp.Action)) item.Action = tmp.Action;
                    if (!string.IsNullOrEmpty(tmp.Correctiveaction)) item.Correctiveaction = tmp.Correctiveaction;
                    if (!string.IsNullOrEmpty(tmp.Preventiveaction)) item.Preventiveaction = tmp.Preventiveaction;
                    if (!string.IsNullOrEmpty(tmp.Editor)) item.Editor = tmp.Editor;
                    item.Lastupdatedate = DateTime.Now;
                    db.SaveChanges();
                    return Json(item.Id);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddIssueSummary_QTY_TotalDowntime(int Id, IssueSummary newOne)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                return BadRequest("Please select one issue to modify ");
            }
            try
            {
                using (ECContext db = new ECContext())
                {
                    IssueSummary item = db.IssueSummaries.Find(Id);
                    item.Qty += newOne.Qty;
                    item.Totaldowntime += newOne.Totaldowntime;
                    db.SaveChanges();
                    return Json(item.Id);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CloseDowntime_ModifyIssueSummary(IssueSummary tmp)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    var where = db.IssueSummaries.Where(e=>true);
                    if (!string.IsNullOrEmpty(tmp.Project)) where = where.Where(e => e.Project.Equals(tmp.Project));
                    if (!string.IsNullOrEmpty(tmp.Department)) where = where.Where(e => e.Department.Equals(tmp.Department));
                    if (!string.IsNullOrEmpty(tmp.Line)) where = where.Where(e => e.Line.Equals(tmp.Line));
                    if (!string.IsNullOrEmpty(tmp.Station)) where = where.Where(e => e.Station.Equals(tmp.Station));
                    if (!string.IsNullOrEmpty(tmp.Issue)) where = where.Where(e => e.Issue.Equals(tmp.Issue));
                    if (!string.IsNullOrEmpty(tmp.Rootcause)) where = where.Where(e => e.Rootcause.Equals(tmp.Rootcause));
                    if (!string.IsNullOrEmpty(tmp.Week)) where = where.Where(e => e.Week.Equals(tmp.Week));
                    var items = where.ToList();
                    if (items.Count > 0)
                    {
                        AddIssueSummary_QTY_TotalDowntime(items[0].Id,tmp);
                    }
                    else {
                        CreateIssueSummary(tmp);
                    }
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
