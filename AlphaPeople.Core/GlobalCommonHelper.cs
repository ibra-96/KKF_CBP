using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AlphaPeople.Core
{
    abstract public class GlobalCommonHelper
    {
        #region General Methods

        /// <summary>
        /// Take any string and encrypt it using SHA1 then
        /// return the encrypted data
        /// </summary>
        /// <param name="data">input text you will enterd to encrypt it</param>
        /// <returns>return the encrypted text as hexadecimal string</returns>
        public string GetSHA1HashData(string data)
        {
            //create new instance of md5
            SHA1 sha1 = SHA1.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();
        }

        /// <summary>
        /// Creates a slug url from string .
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public string GetSlugURLFromString(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            // invalid chars          
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space  
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens  
            return str;
        }

        public void LogException(string[] lines, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = path + "\\Log_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_').Replace('-', '_') + ".txt";

            System.IO.File.AppendAllLines(filepath, lines);
        }

        /// <summary>
        /// Delete file by specified path.
        /// </summary>
        /// <param name="path">path of file.</param>
        public void DeleteTargetFile(string path)
        {
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }
        }

        public bool SendEmailToTarget(string To, string subject, string body)
        {
            //string From = ConfigurationManager.AppSettings["From"].ToString();
            //string Password = ConfigurationManager.AppSettings["Password"].ToString();
            string From = "";
            string Password = "";
            bool success = true;
            // Replace sender@example.com with your "From" address. 
            // This address must be verified with Amazon SES.
            String FROM = From;
            String FROMNAME = "KKF Admin";

            // Replace recipient@example.com with a "To" address. If your account 
            // is still in the sandbox, this address must be verified.
            //  String TO = "recipient@amazon.com";

            // Replace smtp_username with your Amazon SES SMTP user name.
            String SMTP_USERNAME = From;

            // Replace smtp_password with your Amazon SES SMTP user name.
            String SMTP_PASSWORD = Password;

            // (Optional) the name of a configuration set to use for this message.
            // If you comment out this line, you also need to remove or comment out
            // the "X-SES-CONFIGURATION-SET" header below.
            //  String CONFIGSET = "ConfigSet";

            // If you're using Amazon SES in a region other than US West (Oregon), 
            // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
            // endpoint in the appropriate AWS Region.
            String HOST = "smtp.gmail.com";

            // The port you will connect to on the Amazon SES SMTP endpoint. We
            // are choosing port 587 because we will use STARTTLS to encrypt
            // the connection.
            int PORT = 587;

            // The subject line of the email
            String SUBJECT = subject;
            //"Amazon SES test (SMTP interface accessed using C#)";

            // The body of the email
            String BODY = body;
            //"<h1>Amazon SES Test</h1>" +
            //"<p>This email was sent through the " +
            //"<a href='https://aws.amazon.com/ses'>Amazon SES</a> SMTP interface " +
            //"using the .NET System.Net.Mail library.</p>";

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add(new MailAddress(To));
            message.Subject = SUBJECT;
            message.Body = BODY;
            // Comment or delete the next line if you are not using a configuration set
            //  message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                // Enable SSL encryption
                client.EnableSsl = true;

                // Try to send the message. Show status in console.
                try
                {

                    client.Send(message);
                    return success;
                }
                catch (Exception ex)
                {
                    string Logpath = ConfigurationManager.AppSettings["Log"].ToString();
                    string[] lines = { "Message : " + ex.Message, "InnerException : " + ((ex.InnerException == null) ? "" : ex.InnerException.ToString()), "Date : " + DateTime.Now.ToString(), "________________________________________________________/t /n" };
                    LogException(lines, Logpath);
                    return false;

                }
            }
        }

        public bool SendEmailWithAttachment(string Subject, string TO, string Body)
        {
            bool success = true;
            try
            {
                MailMessage mail = new MailMessage();

                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("your_email_address@gmail.com");

                mail.To.Add(TO);

                mail.Subject = Subject;

                mail.Body = Body;

                System.Net.Mail.Attachment attachment;

                attachment = new System.Net.Mail.Attachment("your attachment file");

                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;

                SmtpServer.Credentials = new System.Net.NetworkCredential("username", "password");

                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return success;
            }

            catch (Exception ex)
            {
                string Logpath = ConfigurationManager.AppSettings["Log"].ToString();
                string[] lines = { "Message : " + ex.Message, "InnerException : " + ((ex.InnerException == null) ? "" : ex.InnerException.ToString()), "Date : " + DateTime.Now.ToString(), "________________________________________________________/t /n" };
                LogException(lines, Logpath);
                return false;
            }
        }

        /// <summary>
        /// Returns Allowed HTML only.
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Allowed HTML</returns>
        public string EnsureOnlyAllowedHtml(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            const string allowedTags = "br,hr,b,i,u,a,div,ol,ul,li,blockquote,img,span,p,em," +
                                        "strong,font,pre,h1,h2,h3,h4,h5,h6,address,cite";

            var m = Regex.Matches(text, "<.*?>", RegexOptions.IgnoreCase);
            for (int i = m.Count - 1; i >= 0; i--)
            {
                string tag = text.Substring(m[i].Index + 1, m[i].Length - 1).Trim().ToLower();

                if (!IsValidTag(tag, allowedTags))
                {
                    text = text.Remove(m[i].Index, m[i].Length);
                }
            }

            return text;
        }

        #endregion

        #region Internal Processing Private Methods

        private string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        private bool IsValidTag(string tag, string tags)
        {
            string[] allowedTags = tags.Split(',');
            if (tag.IndexOf("javascript") >= 0) return false;
            if (tag.IndexOf("vbscript") >= 0) return false;
            if (tag.IndexOf("onclick") >= 0) return false;

            var endchars = new char[] { ' ', '>', '/', '\t' };

            int pos = tag.IndexOfAny(endchars, 1);
            if (pos > 0) tag = tag.Substring(0, pos);
            if (tag[0] == '/') tag = tag.Substring(1);

            foreach (string aTag in allowedTags)
            {
                if (tag == aTag) return true;
            }

            return false;
        }

        #endregion

    }
}
