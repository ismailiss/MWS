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
                message.From = new ("ismailelaissaoui1@gmail.com");
                foreach(string email in emails)
                {
                    message.To.Add("ismailelaissaoui1@gmail.com");
                }
                message.Subject = "MWSAPP FILES";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = body;
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
