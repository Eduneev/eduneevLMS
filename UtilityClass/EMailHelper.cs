using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static UtilityClass.DAL;

namespace UtilityClass
{
    public class EMailHelper
    {
        public static string SendEmail(string emailFrom, string emailTo, string emailText, string emailCC, string subject)
        {
            try
            {
                MailAddress macTo = new MailAddress(emailTo);
                MailAddress cc;
                MailMessage mm = new MailMessage();

                mm.From = new MailAddress(emailFrom);
                mm.To.Add(macTo);

                if (emailCC != null)
                {
                    cc = new MailAddress(emailCC);
                    mm.CC.Add(cc);
                }

                mm.Body = emailText;
                mm.Subject = subject;
                mm.IsBodyHtml = true;

                System.Net.Mail.SmtpClient smtpserv = new System.Net.Mail.SmtpClient(EnvSettings.SMTPSERVER);
                smtpserv.Send(mm);

                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string SendEmailWithAttachment(string emailFrom, string emailTo, string emailText, string emailCC, string subject, string filePath)
        {
            try
            {
                MailAddress macTo = new MailAddress(emailTo);
                MailAddress cc;

                MailMessage mm = new MailMessage();

                mm.From = new MailAddress(emailFrom);
                mm.To.Add(macTo);

                if (emailCC != null)
                {
                    cc = new MailAddress(emailCC);
                    mm.CC.Add(cc);
                }

                mm.Body = emailText;
                mm.Subject = subject;
                mm.IsBodyHtml = true;
                mm.Attachments.Add(new Attachment(filePath));
                System.Net.Mail.SmtpClient smtpserv = new System.Net.Mail.SmtpClient(EnvSettings.SMTPSERVER);
                //uncomment at production time by prashant 
                smtpserv.Send(mm);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
