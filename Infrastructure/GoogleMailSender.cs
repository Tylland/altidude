using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;


namespace Altidude.Infrastructure
{
    public class GoogleMailSender
    {
        public void Send(MailMessage message)
        {
            //SmtpClient client = new SmtpClient();
            //client.Port = 587;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.Host = "smtp.gmail.com";

            //client.Send(mail);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("altidude.net@gmail.com", "Altidude99"),
                Timeout = 20000
            };


            smtp.Send(message);
        }
    }
}