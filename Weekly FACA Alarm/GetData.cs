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

        public List<WeeklyAlarmNameList> GetContact(string department, string project, int level)
        {
            List<WeeklyAlarmNameList> item = db.WeeklyAlarmNameLists.Where(e => e.Department.Equals(department) & e.Project.Equals(project) & e.Level <= level).ToList();
            return item;
        }

        public List<myTableBody> GetInfo(string department=null, string project=null) {
            string currentweek = DateTime.Now.Year.ToString() + (gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Sunday)-1).ToString().PadLeft(2, '0');
//#  if DEBUG
//            currentweek = "202325";
//#endif
            var where = db.IssueSummaries.Where(e => e.Week.Equals(currentweek));  //  & e.Action == null & e.Correctiveaction == null & e.Preventiveaction == null
            where = !string.IsNullOrEmpty(department) ? where.Where(e => e.Department.Equals(department)) : where;
            where = !string.IsNullOrEmpty(project) ? where.Where(e => e.Project.Equals(project)) : where;
            var tmp = where.OrderByDescending(e => e.Qty).Take(3);  //取前三项
            var items = tmp.Where(e => e.Action == null & e.Correctiveaction == null & e.Preventiveaction == null).Select(e => new myTableBody
            {
                Week = e.Week,
                Department = e.Department,
                Project = e.Project,
                Line = e.Line,
                Station = e.Station,
                issue = e.Issue,
                Rootcause = e.Rootcause,
                QTY = e.Qty.Value
            }).ToList();
            return items;
        }
    }

    public class myTableBody{
        public string Week { get; set; }
        public string Department { get; set; }
        public string Project { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public string issue { get; set; }
        public string Rootcause { get; set; }
        public int QTY { get; set; }
    }



}
