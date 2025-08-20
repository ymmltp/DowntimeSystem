using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using lib.SqlServerHelper;
using lib.PostgreSqlHelper;
using lib.CommonTools;
using lib.Logger;

namespace IsTrueDowntimeCheck
{
    //每天早上7：15点检查前一天 7：15- 今天7：15的Downtime，
    //只要设备做过点检Downtime 就算有效
    //如何没有设备点检记录，或者是无生产不点检，则Downtime无效，添加备注信息，（无点检，不统计Downtime)
    class Program
    {
        private static string pmmsConnection_String = "data source=cnwuxm0lsql01;initial catalog=PMMS;persist security info=True;user id=pmms_readonly;password=Jabil123;MultipleActiveResultSets=True;App=EntityFramework;TrustServerCertificate=true";
        private static string epmConnection_String = "data source=cnwuxg0te01;initial catalog=epmsheet;persist security info=True;user id=sa;password=Jabil12345;MultipleActiveResultSets=True;App=EntityFramework;TrustServerCertificate=true";
        private static string downtimeConnection_String = "Host=cnwuxm1medb01;Database=EC;Username=ECUser;Password=Jabil123";
        private static string startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 7:15:00";
        private static string endTime = DateTime.Now.ToString("yyyy-MM-dd") + " 7:15:00";

        static void Main(string[] args)
        {
            string[] downtime_EQIDs = GetDowntimeEQs();
            string[] inspect_EQIDs = GetInspectEQs();
            string[] unNormal_EQIDs = downtime_EQIDs.Except(inspect_EQIDs).ToArray();
            if (unNormal_EQIDs.Length > 0)
            {
                string sqlWhere = string.Join("','", unNormal_EQIDs);
                using (PostgreSqlHelper downtimedb = new PostgreSqlHelper(downtimeConnection_String))
                {
                    string updatesql = $"update incident_det set calcdowntime = false , action = 'Abnormal downtime, without inspect history' where ctime >='{startTime}' and ctime<='{endTime}' and calcdowntime =true and machine in ('{sqlWhere}')";
                    int count = downtimedb.Execute(updatesql);
                    Logger.Instance.WriteLog($"Update {count} rows ");
                }
            }
            else {
                Logger.Instance.WriteLog($"Update 0 rows ");
            }
        }

        public static string[] GetDowntimeEQs() {
            string[] downtime_eqids;
            string[] pmms_eqids ;
            using (PostgreSqlHelper downtimedb = new PostgreSqlHelper(downtimeConnection_String)) {
                string selectEQIDs = $"select distinct(machine) as EQID  from incident_det id  where id.ctime >='{startTime}' and id.ctime<='{endTime}' and calcdowntime =true";
                downtime_eqids =CommonTools.ConvertDataTableToList<EQs>(downtimedb.GetData(selectEQIDs)).Select(e=>e.EQID).ToArray();
            }
            using (SqlServerHelper pmmsdb = new SqlServerHelper(pmmsConnection_String)) {
                string selectEQIDs = "Select distinct(EQID) from eq where workcell like '%FATP%'";
                pmms_eqids = CommonTools.ConvertDataTableToList<EQs>(pmmsdb.GetData(selectEQIDs)).Select(e => e.EQID).ToArray();
            }
           return downtime_eqids.Intersect(pmms_eqids).ToArray();
        }

        public static string[] GetInspectEQs() {
            List<EQs> inspect_eqids = new List<EQs>();
            using (SqlServerHelper epmdb = new SqlServerHelper(epmConnection_String)) {
                string selectEQIDs = $"select distinct (i2.EQID) from inspecthistory i left join inspectitems i2  on i.inspectid=i2.inspectid  where i.CheckDate >='{startTime}' and  i.CheckDate <='{endTime}' and (description is null or description !='无需点检')";
                inspect_eqids = CommonTools.ConvertDataTableToList<EQs>(epmdb.GetData(selectEQIDs));
            }
            return inspect_eqids.Select(e => e.EQID).ToArray();
        }
    }

    public class EQs { 
        public string EQID { get; set; }
    }
}
