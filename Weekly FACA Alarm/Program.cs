using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using DowntimeSystem.Models;

namespace Weekly_FACA_Alarm
{
    class Program
    {
        static void Main(string[] args)
        {
            GetData gd = new GetData();
            List<myTableBody> dt = gd.GetInfo();
            foreach (var i in dt) {
                if (i.count > 0) {
                    List<WeeklyAlarmNameList> econtact = gd.GetContact(i.department, i.project);
                    if (econtact.Count == 0) continue;
                    //生成联系人信息
                    List<string> to = new List<string>();
                    List<string> cc = new List<string>();
                    foreach (var mail in econtact)
                    {
                        to.Add(mail.Email);
                    }
                    SendMail.MailSend("", to, cc);
                }
            }
        }
    }
}
