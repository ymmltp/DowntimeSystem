using System;
using System.Data;
using System.Collections.Generic;
using DowntimeSystem.Models;
using System.Linq;
using System.IO;
using Newtonsoft.Json;



namespace Escalation_Mail
{
    class Program
    {
        private static string jsonfile = "./Setting.json";
        private static CSetting setting = new CSetting();

        //level1-level2 gap 内的的发送给技术员 （<=level1）
        //level2-level3 gap的发送给工程师+技术员（<=level2）
        //level3 gap以外的的发送给工程师+技术员+manager  (<=level3)

        static void Main(string[] args)
        { 
            if (!ReadJson()) throw (new Exception("Setting initialization failed 初始化失败"));

            if (setting.Block.Count <= 0) return;
            else
            {
                foreach (var mailblock in setting.Block) {
                    GetData gd = new GetData();
                    List<EscalationRule> contact = gd.GetRule(mailblock.Department, mailblock.Project);
                    var tmp = contact.OrderBy(e=>e.Timespan).GroupBy(e => new { e.Department, e.Project }).ToList();
                    //遍历各项目，部门的信息
                    foreach (var i in tmp)
                    {
                        //获取未关闭的 Downtime 数据
                        List<myTableBody> dt = gd.GetInfo(i.Key.Department, i.Key.Project);
                        //获取不同等级邮件的发送规则
                        var tmp1 = i.ToList(); 
                        for(int index = 0;index<tmp1.Count;index++)
                        {
                            //获取该等级下的 Downtime 事件
                            var where = dt.Where(e => e.Occurtime.AddMinutes(tmp1[index].Timespan) <= DateTime.Now );
                            if (index != (tmp1.Count - 1)) { 
                                where = where.Where(e => e.Occurtime.AddMinutes(tmp1[index + 1].Timespan) > DateTime.Now);
                            }
                            var downtimeInfo = where.ToList();
                            if (downtimeInfo == null || downtimeInfo.Count <= 0) continue;
                            //获取该等级下的联系人
                            List<EscalationNameList> econtact = gd.GetContact(i.Key.Department, i.Key.Project, tmp1[index].Level);
                            if (econtact == null) continue;
                            //生成联系人信息
                            List<string> to = new List<string>();
                            List<string> cc = new List<string>();

                            //debug
                            //to.Add("Adele_Lu@jabil.com");
                            //to.Add("Justin_Zhu@jabil.com");
                            //to.Add("Neil_Gao@jabil.com");


                            foreach (var mail in econtact)
                            {
                                if (mail.Contacttype.ToUpper() == "TO")
                                    to.Add(mail.Email);
                                if (mail.Contacttype.ToUpper() == "CC")
                                    cc.Add(mail.Email);
                            }

                            //根据内容生成邮件body
                            string content = CreateForm(downtimeInfo);
                            SendMail.MailSend(content, to, cc, tmp1[index].Timespan, downtimeInfo.Count, tmp1[index].Level);
                        }
                    }
                }
            }
        }

        //生成邮件body
        private static string CreateForm(List<myTableBody> obj) {
            var paras = obj[0].GetType().GetProperties();
            string table = @"<table  style='border-collapse:collapse;font-size:13px;  '  border=1 bordercolor=DCDCDC><tr  align=center  bgcolor=#F5F5F5><B>";
            foreach (var i in paras) {
                if (i.Name.ToUpper() == "DOWNTIME") 
                    table += $@"<td>Downtime(mins)</td>"; 
                else
                    table += $@"<td>{i.Name}</td>";
            }
            table += @" </B></tr>";
            foreach (var item in obj) {
                table += "<tr  align=center >";
                var paramaters = item.GetType().GetProperties();
                foreach (var property in paramaters)
                {
                    if (property.Name.ToUpper() == "ID")
                    {
                        table += $@"<td><a href='http://cnwuxg0te01:8050/Home/Query/?ticket={property.GetValue(item, null)}'>{property.GetValue(item, null)}</a></td>";
                    }
                    else {
                        table += $@"<td>{property.GetValue(item, null)}</td>";
                    }
                }
                table += "</tr>";
            }
            table += @"</table>";
            return table;
        }

        // Read JSON File 
        private static bool ReadJson()
        {
            try
            {
                var bizConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, jsonfile);
                string json = File.ReadAllText(bizConfigPath);
                setting = JsonConvert.DeserializeObject<CSetting>(json);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

    public class CSetting
    {
        public List<MailBlock> Block { get; set; }
        public class MailBlock
        {
            public string Project { get; set; }
            public string Department { get; set; }
        }
    }

}
