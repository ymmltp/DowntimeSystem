using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowntimeSystem.Models;

namespace DowntimeSystem.Controllers
{
    public class EscalateNameListController : Controller
    {
        #region 查询 
        [HttpGet]
        public IActionResult Get(EscalationNameList tmp)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    var where = db.EscalationNameLists.Where(e => true);
                    if (!string.IsNullOrEmpty(tmp.Project)) where = where.Where(e => e.Project.Equals(tmp.Project));
                    if (!string.IsNullOrEmpty(tmp.Department)) where = where.Where(e => e.Department.Equals(tmp.Department));
                    var items = where.ToList();
                    return Json(items);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region 编辑 
        [HttpPost]
        public IActionResult Add([FromBody]EscalationNameList item)
        {
            if (string.IsNullOrEmpty(item.Email))
            {
                return BadRequest("请输入邮箱再提交...");
            }
            try
            {
                using (ECContext db = new ECContext())
                {
                    db.EscalationNameLists.Add(item);
                    db.SaveChanges();
                    return Json(item);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);     //有冲突
            }
        }

        [HttpPost]
        public IActionResult Update([FromBody]EscalationNameList item)
        {
            if (string.IsNullOrEmpty(item.Email))
            {
                 return BadRequest("请输入邮箱再提交...");
            }
            try
            {
                using (ECContext db = new ECContext())
                {
                    db.EscalationNameLists.Update(item);
                    db.SaveChanges();
                    return Json(item);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region 删除
        [HttpPost]
        public IActionResult Delete([FromBody]EscalationNameList item)
        {
            try
            {
                if (string.IsNullOrEmpty(item.Email))
                { 
                    return BadRequest("请输入邮箱再提交...");
                }
                using (ECContext db = new ECContext())
                {
                    db.EscalationNameLists.Remove(item);
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
