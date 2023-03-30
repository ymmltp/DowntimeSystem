using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DowntimeSystem.Models;
using System.Globalization;

namespace Weekly_FACA_Alarm
{
    public class GetData
    {
        private static ECContext db = new ECContext();
        private GregorianCalendar gc = new GregorianCalendar();
        public GetData()
        {

        }

        public List<WeeklyAlarmNameList> GetContact(string department, string project)
        {
            List<WeeklyAlarmNameList> item = db.WeeklyAlarmNameLists.Where(e => e.Department.Equals(department) & e.Project.Equals(project)).ToList();
            return item;
        }

        public List<myTableBody> GetInfo(string department=null, string project=null) {
            string currentweek = DateTime.Now.Year.ToString() + gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Sunday).ToString().PadLeft(2, '0');

            var where = db.IssueSummaries.Where(e => e.Week == currentweek & e.Action == null & e.Correctiveaction == null & e.Preventiveaction == null);  // 
            where = !string.IsNullOrEmpty(department) ? where.Where(e => e.Department.Equals(department)) : where;
            where = !string.IsNullOrEmpty(project) ? where.Where(e => e.Project.Equals(project)) : where;
            var items = where.GroupBy(e => new { e.Department, e.Project }).Select(g => new myTableBody
            {
                department = g.Key.Department,
                project = g.Key.Project,
                count = g.Count()
            }).ToList();
            return items;
        }
    }

    public class myTableBody{
        public string department { get; set; }
        public string project { get; set; }
        public int count { get; set; }
    }



}
