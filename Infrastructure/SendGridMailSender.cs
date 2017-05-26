using System;
using System.Net;
using SendGrid;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Altidude.Infrastructure
{
    public class SendGridMailSender: IIdentityMessageService
    {
        private const string ApiKey = "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";
        //public void Send(MailMessage message)
        //{
        //    SendGridMessage sgMessage = new SendGridMessage();

        //    foreach (var to in message.To)
        //        sgMessage.AddTo(to.Address);

        //    sgMessage.From = message.From;    //new MailAddress("altidude.net@gmail.com", "Altidude.net");
        //    sgMessage.Subject = message.Subject;
        //    sgMessage.Text = message.Body;

        //    // Create credentials, specifying your user name and password.
        //    var credentials = new NetworkCredential("AltidudeMail", "azure_50caa16cfd38f30917c23017940897d9@azure.com");

        //    // Create an Web transport for sending email.
        //    var transportWeb = new Web(credentials);

        //    // Send the email, which returns an awaitable task.
        //    //transportWeb.DeliverAsync(sgMessage);

        //    // If developing a Console Application, use the following
        //    transportWeb.DeliverAsync(sgMessage).Wait();
        //}
        public void Send(MailMessage message)
        {
            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");

            //var credentials = new NetworkCredential("AltidudeMail", "azure_50caa16cfd38f30917c23017940897d9@azure.com");

            var client = new SendGridClient(ApiKey);
            var from = new EmailAddress(message.From.Address, message.From.DisplayName);
            var subject = message.Subject;
            var to = new EmailAddress(message.To[0].Address, message.To[0].DisplayName);
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            client.SendEmailAsync(msg).Wait();
        }

        public Task SendAsync(IdentityMessage message)
        {
            return Task.Run(() =>
            {
                SendMessage("altidude.net@gmail.com", "Altidude", message.Destination, null, message.Subject,
                    "", message.Body);
            });
        }

        public async Task SendMessageAsync(string fromAddress, string fromDisplayName, string toAddress, string toDisplayName, string subject, string plainTextContent, string htmlContent)
        {
            var client = new SendGridClient(ApiKey);
            var from = new EmailAddress(fromAddress, fromDisplayName);
            var to = new EmailAddress(toAddress, toDisplayName);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await client.SendEmailAsync(msg);
        }

        public void SendMessage(string fromAddress, string fromDisplayName, string toAddress, string toDisplayName, string subject, string plainTextContent, string htmlContent)
        {
            SendMessageAsync(fromAddress, fromDisplayName, toAddress, toDisplayName, subject, plainTextContent,
                htmlContent).Wait();
        }
    }
}