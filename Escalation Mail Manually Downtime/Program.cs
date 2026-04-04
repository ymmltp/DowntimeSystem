using System;
using System.Data;
using System.Collections.Generic;
using DowntimeSystem.Models;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace Escalation_Mail_Manually_Downtime
{
    class Program
    {
        //手动上传的Downtime发送预警邮件，逐条发送
        //level1-level2 gap 内的的发送给技术员 （<=level1）
        //level2-level3 gap的发送给工程师+技术员（<=level2）
        //level3 gap以外的的发送给工程师+技术员+manager  (<=level3)
        //发送给当前项目所有部门人员

        static void Main(string[] args)
        {
            GetData gd = new GetData();
            List<myTableBody> dt = gd.GetInfo();
            //遍历各项目，部门的信息
            foreach (var i in dt)
            {
                List<EscalationRule> contact = gd.GetRule(i.Department, i.Project);
                var tmp = contact.OrderBy(e => e.Timespan).ToList();
                //获取未关闭的 Downtime 数据
                //获取不同等级邮件的发送规则                
                for (int index = tmp.Count - 1; index >= 0; index--)
                {
                    if (i.Occurtime.AddMinutes(tmp[index].Timespan) > DateTimeOffset.Now) continue;
                    else
                    {
                        //获取该等级下的联系人
                        List<EscalationNameList> econtact = gd.GetContact(i.Project, tmp[index].Level);
                        if (econtact == null) continue;
                        //生成联系人信息
                        List<string> to = new List<string>();
                        List<string> cc = new List<string>();
                        //debug
                        to.Add("Adele_Lu@jabil.com");
                        foreach (var mail in econtact)
                        {
                            if (mail.Contacttype.ToUpper() == "TO")
                                to.Add(mail.Email);
                            if (mail.Contacttype.ToUpper() == "CC")
                                cc.Add(mail.Email);
                        }
                        //根据内容生成邮件body
                        string content = CreateForm(i);
                        SendMail.MailSend(content, to, cc, tmp[index].Timespan, 1, tmp[index].Level);
                        break;
                    }

                }
            }
        }

        //生成邮件body
        private static string CreateForm(myTableBody row)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<table border='1' style='border-collapse:collapse; width:60%; font-family:Arial;'>");
            html.Append("<caption style='font-weight:bold; font-size:18px; padding:5px;'>Down time</caption>");
            html.AppendFormat("<tr><td><strong>Workcell</strong></td><td style='text-align:center;'>{0}</td></tr>", row.Project);
            html.AppendFormat("<tr><td><strong>Line</strong></td><td style='text-align:center;'>{0}</td></tr>", row.Line);
            html.AppendFormat("<tr><td><strong>Date</strong></td><td style='text-align:center;'>{0}</td></tr>", Convert.ToDateTime(row.Occurtime).ToString("yyyy-MM-dd"));
            html.AppendFormat("<tr><td><strong>Station</strong></td><td style='text-align:center;'>{0}</td></tr>", row.Station);
            html.AppendFormat("<tr><td><strong>Start time</strong></td><td style='text-align:center;'>{0}</td></tr>",  Convert.ToDateTime(row.Occurtime).ToString("HH:mm"));
            html.AppendFormat("<tr><td><strong>End time</strong></td><td style='text-align:center;'>{0}</td></tr>","Now");
            html.AppendFormat("<tr><td style='background-color:yellow;'><strong>Issue description:</strong></td><td style='text-align:center;'>{0}</td></tr>",row.Issueremark);
            html.AppendFormat("<tr><td><strong>Downtime Dep.</strong></td><td style='text-align:center;'>{0}</td></tr>",row.Department);
            html.AppendFormat("<tr><td><strong>Downtime PIC</strong></td><td style='text-align:center;'>{0}</td></tr>",row.Department);
            html.AppendFormat("<tr style='background-color:red; color:white;'><td><strong>Status:</strong></td><td style='text-align:center;'>{0}</td></tr>", "Open");
            html.Append("</table>");
            return html.ToString();
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
