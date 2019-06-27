using SendGrid;
using SendGrid.Helpers.Mail;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureMail
{
    class MailComponent
    {
        public static void SendMail(string content)
        {
            var client = new SendGridClient("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");
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("sandip.khandelwal@microsoft1.com", "MOM"));
            var recipients = new List<EmailAddress>
                {
                    new EmailAddress("sandip.khandelwal@gmail.com"),
                    new EmailAddress("neeta9657@gmail.com"),
                    new EmailAddress("umapuja@gmail.com")
                };
            msg.AddTos(recipients);
            msg.SetSubject("Mail from Azure and SendGrid");
            msg.AddContent(MimeType.Text, content);
            client.SendEmailAsync(msg);
        }
    }
}
