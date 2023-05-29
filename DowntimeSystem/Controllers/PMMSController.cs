using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowntimeSystem.Models.PMMS;
using DowntimeSystem.Models;

namespace DowntimeSystem.Controllers
{

    public class PMMSController : Controller
    {
        private PMMSContext db = new PMMSContext();

        [HttpGet]
        public IActionResult getEQID(string[] department, string[] project, string[] line,string[] type)
        {
            var where = db.Eqs.Where(e => true);
            where = department.ArrayIsTrueNull() ? where : where.Where(e => department.Contains(e.Department));
            where = project.ArrayIsTrueNull() ? where : where.Where(e => project.Contains(e.Workcell));
            where = line.ArrayIsTrueNull() ? where : where.Where(e => line.Contains(e.Line));
            where = type.ArrayIsTrueNull() ? where : where.Where(e => type.Contains(e.Category));
            var item = where.Select(e => e.Eqid).Distinct().ToList();
            return Json(item);
        }

        [HttpGet]
        public IActionResult GetDepartment()
        {
            var item = db.Eqs.Select(e => e.Department).Distinct().ToList();
            return Json(item);         
        }

        [HttpGet]
        public IActionResult GetProject()
        {
            var item = db.Eqs.Select(e => e.Workcell).Distinct().ToList();
            return Json(item);        
        }

        [HttpGet]
        public IActionResult GetLine(string[] department,string[] project)
        {
            var where = db.Eqs.Where(e => true);
            where =department.ArrayIsTrueNull()? where : where.Where(e => department.Contains(e.Department));
            where = project.ArrayIsTrueNull() ? where : where.Where(e => project.Contains(e.Workcell));
            var item = where.Select(e => e.Line).Distinct().ToList();
            return Json(item);          
        }

        [HttpGet]
        public IActionResult GetEQType(string[] department, string[] project,string[] line)
        {
            var where = db.Eqs.Where(e => true);
            where = department.ArrayIsTrueNull() ? where : where.Where(e => department.Contains(e.Department));
            where = project.ArrayIsTrueNull() ? where : where.Where(e => project.Contains(e.Workcell));
            where = line.ArrayIsTrueNull() ? where : where.Where(e =>line.Contains(e.Line));
            var item = where.Select(e => e.Category).Distinct().ToList();
            return Json(item);
        }
    }
}
