using System;
using System.Net;
using System.Text;
using System.Net.Mail;

namespace AlphaPeople.Core
{
    public class MailHelper
    {
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string Host { get; set; }
        public static int Port { get; set; }
        public static bool SSL { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; } = true;

        static MailHelper()
        {
            Host = "smtp.office365.com";
            Port = 587;
            SSL = true;
            Username = "cbpportal@kkf.org.sa";
            Password = "P@ssw0rd#56789";
        }

        public void Send(string portalLinke)
        {
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = Host;
                smtp.Port = Port;
                smtp.EnableSsl = SSL;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(Username, Password);

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(Username, "مؤسسة الملك خالد", Encoding.UTF8);
                    message.To.Add(ToEmail);
                    message.Subject = Subject;
                    message.BodyEncoding = Encoding.UTF8;
                    message.Body = $"<div style='direction:rtl; font-size:14px;'> {Body} <br/><br/>"
                                 + $"<a style='font-size:16px; text-decoration:none; color:#C0a979;' target='_blank' href='https://cbp.kkf.org.sa/{portalLinke}'>للذهاب إلى البوابة يرجى الضغط هنا</a><br/><br/>";
                    message.IsBodyHtml = IsHtml;
                    try
                    {
                        smtp.Send(message);
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}