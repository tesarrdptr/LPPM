using System;
using System.Net.Mail;
using System.Web.Configuration;

namespace ProposalPolmanAstra.Classes
{
    public class MailFunction
    {
        public void SendMail(string subjek, string to, string body)
        {
            MailMessage mail = new MailMessage();
            mail.Subject = subjek;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.From = new MailAddress(WebConfigurationManager.AppSettings["linkUserMail"]);
            mail.To.Add(new MailAddress(to));

            SmtpClient message = new SmtpClient(WebConfigurationManager.AppSettings["linkSMTPServer"], Convert.ToInt32(WebConfigurationManager.AppSettings["linkPort"]));
            message.UseDefaultCredentials = false;
            message.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["linkUserMail"], WebConfigurationManager.AppSettings["linkPasswordMail"]);
            message.DeliveryMethod = SmtpDeliveryMethod.Network;
            message.EnableSsl = true;

            if (WebConfigurationManager.AppSettings["isCanSend"].ToString().Equals("1"))
                message.Send(mail);
        }
    }
}