using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DowntimeSystem.Models;

namespace Escalation_Mail
{
    public class GetData
    {
        private static ECContext db = new ECContext();

        public GetData()
        {

        }

        public List<EscalationNameList> GetContact(string department, string project, int level)
        {
            List<EscalationNameList> item = db.EscalationNameLists.Where(e => e.Department.Equals(department) & e.Project.Equals(project) & e.Level <= level).ToList();
            return item;
        }

        public List<myTableBody> GetInfo(string department,string project) {
            var where  = db.IncidentDets.Where(e => e.Calcdowntime == true & e.Incidentstatus==0 & e.Occurtime>= Convert.ToDateTime("2023-03-27") );
            where = !string.IsNullOrEmpty(department) ? where.Where(e => e.Department.Equals(department)):where;
            where = !string.IsNullOrEmpty(project) ? where.Where(e => e.Project.Equals(project)):where;
            List<myTableBody> items = where.Select(e => new myTableBody
            {
                Id = e.Id,
                Comefrom = e.Comefrom,
                Department = e.Department,
                Project = e.Project,
                Line = e.Line,
                Station = e.Station,
                Occurtime = e.Occurtime,
                Downtime =(int)(DateTime.Now-e.Occurtime).TotalMinutes,
                Issue = e.Issue,
                Issueremark = e.Issueremark,
                Creator = e.Creator,
                Machine = e.Machine,
            }).ToList();
            return items;
        }

        public List<EscalationRule> GetRule(string department=null,string project=null) {
            var where = db.EscalationRules.Where(e=>true);
            where = !string.IsNullOrEmpty(department) ? where.Where(e => e.Department.Equals(department)):where;
            where = !string.IsNullOrEmpty(project) ? where.Where(e => e.Project.Equals(project)):where;
            List<EscalationRule> items = where.ToList();
            return items;
        }


    }

    public class myTableBody
    {
        public int Id { get; set; }
        public string Comefrom { get; set; }
        public string Department { get; set; }
        public string Project { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public DateTime Occurtime { get; set; }
        public int? Downtime { get; set; }
        public string Issue { get; set; }
        public string Issueremark { get; set; }
        public string Creator { get; set; }
        public string Machine { get; set; }

    }


}
