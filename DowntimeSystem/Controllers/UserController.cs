using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowntimeSystem.Models;

namespace DowntimeSystem.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        // GET: User/GetUserEntity
        public ActionResult GetUserEntity(string ntid)
        {
            ADHelper ad = new ADHelper();
            string domain = "corp.jabil.org";
            ad.Domain = domain;
            UserInfo userinfo = ad.GetADUserEntity(ntid);
            return Json(userinfo);
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    var items =  db.Users.ToList();
                    return Json(items);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Add([FromBody]Users tmp)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    db.Users.Add(tmp);
                    db.SaveChanges();        
                    return Json("Success");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 
        [HttpPost]
        public IActionResult Update([FromBody]Users tmp)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    db.Users.Update(tmp);
                    db.SaveChanges();        
                    return Json("Success");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                using (ECContext db = new ECContext())
                {
                    Users tmp = db.Users.Where(e => e.Id == id).FirstOrDefault();
                    db.Users.Remove(tmp);
                    db.SaveChanges();        
                    return Json("Success");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}