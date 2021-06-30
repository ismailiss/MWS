using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MWSAPP.Services
{
    public static class EmailSender
    {
        public static void SendEmail(List<string> emails,string body,string filePricing, string fileInventory)
        {
            try
            {
                MailMessage message = new ();
                SmtpClient smtp = new ();
                message.From = new ("Postmaster@ecomera.it");
                foreach(string email in emails)
                {
                    message.To.Add(email);
                }
                message.Subject = "Test";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = body;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                message.Attachments.Add(new Attachment(filePricing));
                message.Attachments.Add(new Attachment(fileInventory));
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
