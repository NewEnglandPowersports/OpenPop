using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Xml;


namespace ConsoleProject
{
    internal class EmailHelper
    {
        public EmailHelper()
        {
            this.sFrom = string.Empty;
            this.sTo = string.Empty;
            this.sSubject = string.Empty;
            this.sBody = string.Empty;
            this.sStore = string.Empty;
        }

        public string sFrom { get; set; }

        public string sTo { get; set; }

        public string sSubject { get; set; }

        public string sBody { get; set; }

        public string sStore { get; set; }

        public void SendEmailMessage(EmailHelper mHelper)
        {
            try
            {
                var myMail = new System.Net.Mail.MailMessage();

                myMail.From = new MailAddress(mHelper.sFrom);
                var mailAddy = new List<string>();
                if (mHelper.sStore == "admin" || mHelper.sStore == "testing")
                {
                    mailAddy.Add("jim.burns@newenglandpowersports.com");
                    mailAddy.Add("mmclean@capecoder.com");
                }
                else
                {
#if DEBUG
                    mailAddy.Add("mmclean@capecoder.com");
                    mailAddy.Add("jim@ne-ps.net");
#else
                    mailAddy = this.BuildEmailTo(mHelper.sStore.ToString());  //todo
#endif
                }

                foreach (string item in mailAddy)
                {
                    myMail.To.Add(new MailAddress(item));
                }
                myMail.Subject = mHelper.sSubject + ' ' + mHelper.sStore;
                myMail.Body = mHelper.sBody;

                myMail.IsBodyHtml = true;
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;
                var sc = new SmtpClient();
#if DEBUG
                sc.Host = "smtp.comcast.net";
                sc.Credentials = new NetworkCredential("capecoderltd@comcast.net", "38ne7y92");
#else
                sc.Host = ConfigurationManager.AppSettings["SMTP_SERVER"].ToString(); 
                sc.Credentials = new NetworkCredential("mailstop@cycles128.com","JIM!345bu");
#endif
                sc.Port = 587;// 465;
                sc.Send(myMail);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw;
            }
        }

        private List<string> BuildEmailTo(string store)
        {
            try
            {
                var mailAddy = new List<string>();
                if (store == null || store == "")
                {
#if DEBUG
                    mailAddy.Add("mmclean@capecoder.com");
#else
                    mailAddy.Add(ConfigurationManager.AppSettings["DefaultToEmailAddress"].ToString());  //todo
#endif
                }
                else
                {

                    var doc = new XmlDocument();
                    doc.Load(ConfigurationManager.AppSettings["XMLEmailFile"].ToString());//todo
                    XmlNodeList emailList = doc.GetElementsByTagName(store.ToLower());
                    foreach (XmlNode item in emailList)
                    {
                        mailAddy.Add(item.InnerText);
                    }
                }
                if (mailAddy.Count == 0)
                {
#if DEBUG
                    mailAddy.Add("mmclean@capecoder.com");
#else
                    mailAddy.Add(ConfigurationManager.AppSettings["DefaultToEmailAddress"].ToString()); //todo
#endif
                }

                return mailAddy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}