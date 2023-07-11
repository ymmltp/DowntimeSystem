using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using DowntimeSystem.Models;
using Newtonsoft.Json;
using System.IO;


namespace Weekly_FACA_Alarm
{
    class Program
    {
        private static string jsonfile = "./Setting.json";
        private static CSetting setting = new CSetting();

        static void Main(string[] args)
        {
            if (!ReadJson()) throw (new Exception("Setting initialization failed 初始化失败"));

            if (setting.Block.Count <= 0) return;
            else
            {
                foreach (var mailblock in setting.Block)
                {
                    GetData gd = new GetData();
                    List<myTableBody> dt = gd.GetInfo(mailblock.Department, mailblock.Project);
                    if (dt.Count == 0) continue;
                    List<WeeklyAlarmNameList> econtact = gd.GetContact(mailblock.Department, mailblock.Project, mailblock.Level);
                    if (econtact.Count == 0) continue;
                    string content = CreateForm(dt);
                    //生成联系人信息
                    List<string> to = new List<string>();
                    List<string> cc = new List<string>();
                    cc.Add("Adele_Lu@jabil.com");
                    foreach (var mail in econtact)
                    {
                        to.Add(mail.Email);
                    }
                    //SendMail.MailSend(content, new List<string>{ "Adele_Lu@jabil.com"}, cc);
                    SendMail.MailSend(content, to, cc);
                }

            }
        }


        //生成邮件body
        private static string CreateForm(List<myTableBody> obj)
        {
            var paras = obj[0].GetType().GetProperties();
            string table = @"<table  style='border-collapse:collapse;font-size:13px;  '  border=1 bordercolor=DCDCDC><tr  align=center  bgcolor=#F5F5F5><B>";
            foreach (var i in paras)
            {
                if (i.Name.ToUpper() == "DOWNTIME")
                    table += $@"<td>Downtime(mins)</td>";
                else
                    table += $@"<td>{i.Name}</td>";
            }
            table += @" </B></tr>";
            foreach (var item in obj)
            {
                table += "<tr  align=center >";
                var paramaters = item.GetType().GetProperties();
                foreach (var property in paramaters)
                {
                    table += $@"<td>{property.GetValue(item, null)}</td>";
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
            public int Level { get; set; }
        }
    }

}

