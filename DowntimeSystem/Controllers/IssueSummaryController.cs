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
        [HttpGet]
        public IActionResult GetIssueSummary(IssueSummary tmp)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    List<IssueSummary> items = db.IssueSummaries.ToList();
                    if (!string.IsNullOrEmpty(tmp.Project)) items = items.Where(e => e.Project.Equals(tmp.Project)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Department)) items = items.Where(e => e.Department.Equals(tmp.Department)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Line)) items = items.Where(e => e.Line.Equals(tmp.Line)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Station)) items = items.Where(e => e.Station.Equals(tmp.Station)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Issue)) items = items.Where(e => e.Issue.Equals(tmp.Issue)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Rootcause)) items = items.Where(e => e.Rootcause.Equals(tmp.Rootcause)).ToList();
                    return Json(items.OrderByDescending(e=>e.Qty).OrderByDescending(e=>e.Totaldowntime));
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
        public IActionResult AddIssueSummary_QTY_TotalDowntime(IssueSummary issueSummary, IssueSummary newOne)
        {
            if (string.IsNullOrEmpty(Convert.ToString(issueSummary.Id)))
            {
                return BadRequest("Please select one issue to modify ");
            }
            try
            {
                using (ECContext db = new ECContext())
                {
                    IssueSummary item = db.IssueSummaries.Find(issueSummary.Id);
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
                    List<IssueSummary> items = db.IssueSummaries.ToList();
                    if (!string.IsNullOrEmpty(tmp.Project)) items = items.Where(e => e.Project.Equals(tmp.Project)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Department)) items = items.Where(e => e.Department.Equals(tmp.Department)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Line)) items = items.Where(e => e.Line.Equals(tmp.Line)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Station)) items = items.Where(e => e.Station.Equals(tmp.Station)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Issue)) items = items.Where(e => e.Issue.Equals(tmp.Issue)).ToList();
                    if (!string.IsNullOrEmpty(tmp.Rootcause)) items = items.Where(e => e.Rootcause.Equals(tmp.Rootcause)).ToList();
                    if (items.Count > 0)
                    {
                        AddIssueSummary_QTY_TotalDowntime(items[0],tmp);
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
