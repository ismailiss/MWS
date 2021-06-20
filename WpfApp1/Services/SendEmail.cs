using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MWSAPP.Services
{
    public static class SendEmail
    {
        public static void Email(string htmlString)
        {
            try
            {
                MailMessage message = new ();
                SmtpClient smtp = new ();
                message.From = new ("ismailelaissa.com");
                message.To.Add(new ("ismailela.com"));
                message.Subject = "Test";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("", "");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex) {
            
            }
        }
    }
}
