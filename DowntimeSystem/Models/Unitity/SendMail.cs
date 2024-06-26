﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;

namespace DowntimeSystem.Models.Unitity
{
    public class SendMail
    {
        public static bool MailSend(string subject, string cont, List<string> mailto, List<string> mailcc)
        {
            /********************jabil******************/
            string smtpServer = "CORIMC04"; //SMTP服务器
            string mailFrom = "Downtime_Alarm@jabil.com"; //登陆用户名
            string userPassword = "";//登陆密码
            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码
            smtpClient.EnableSsl = false;//import
            smtpClient.Port = 25;

            // 发送邮件设置        
            MailMessage mailMessage = new MailMessage(); // 发送人
            mailMessage.From = new MailAddress(mailFrom);

            for (int i = 0; i < mailto.Count; i++)
            {
                    mailMessage.To.Add(mailto[i].Trim());//收件人
            }
            for (int i = 0; i < mailcc.Count; i++)
            {
                mailMessage.CC.Add(mailcc[i].Trim());//收件人
            }
            string mailSubject = subject;

            mailMessage.Subject = mailSubject;//主题      


            mailMessage.Body += "<B>Hi all，</B><br/>";
            mailMessage.Body += "This is an automated message from Downtime System. Please do not reply to this message.<br/>";
            mailMessage.Body += cont;
            mailMessage.Body += "<br/><br/>";

            mailMessage.Attachments.Add(new Attachment(AppDomain.CurrentDomain.BaseDirectory + $"Jabil.png"));
            mailMessage.Attachments[0].ContentType.Name = "image/png";
            mailMessage.Attachments[0].ContentId = "pic";
            mailMessage.Attachments[0].ContentDisposition.Inline = true;
            mailMessage.Attachments[0].TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            mailMessage.Body += "<img src=\"cid:pic\"/><br/>";
            mailMessage.Body += "<span style='font-size:12px'>https://www.jabil.com</span><br/>";
            mailMessage.Body += "<span style='font-size:12px ;color:#888'>Address: Lot J9, J10 Export Processing Zone, Wuxi City, Jiangsu Province, PRC, Post Code: 214028 </span><br/>";

            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码       
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Low;//优先级
            try
            {
                smtpClient.Send(mailMessage); // 发送邮件
                mailMessage.Dispose();
                smtpClient.Dispose();
                return true;
            }
            catch (SmtpException ex)
            {
                return false;
            }
        }

        public static string CreateForm(IEnumerable<object> obj)
        {
            if (obj == null || !obj.Any())
            {
                return "";
            }
            var paras = obj.First().GetType().GetProperties();
            string table = @"<table  style='border-collapse:collapse;font-size:13px;' border=1 bordercolor=DCDCDC><tr  align=center  bgcolor=#F5F5F5><B>";
            foreach (var i in paras)
            {
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
    }
}
