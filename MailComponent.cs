using Chilkat;
using System;

namespace MailComponent
{
    public class SmtpHost
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool IsSecured { get; set; }

        public SmtpHost(string hostName, bool isSecured, int port, string userName, string password)
        {
            HostName = hostName;
            IsSecured = isSecured;
            Port = port;
            UserName = userName;
            Password = password;
        }

    }
    public class MailManager
    {
        private readonly MailMan mailManager;
        private const string unlockCodeForMail = "Any String for 30-Day Trial";

        // static MailManager()
        // {
        //     mailManager = new MailMan();
        //     mailManager.UnlockComponent(unlockCodeForMail);
        //     mailManager.ConnectTimeout = 120;
        // }

        public MailManager(SmtpHost smtpHost)
        {
            mailManager = new MailMan();
            mailManager.UnlockComponent(unlockCodeForMail);
            mailManager.ConnectTimeout = 120;
            mailManager.SmtpHost = smtpHost.HostName;
            mailManager.SmtpPort = smtpHost.Port;
            mailManager.SmtpSsl = smtpHost.IsSecured;
            mailManager.SmtpUsername = smtpHost.UserName;
            mailManager.SmtpPassword = smtpHost.Password;
        }

        // public static MailMan Manager
        // {
        //     get
        //     {
        //         return mailManager;
        //     }
        // }

        public bool SendMail(string sender, string recepient, string subject, string body, out string serverMessage)
        {
            Email email = new Email {From = sender};

            email.AddMultipleTo(recepient);
            email.Subject = subject;
            email.Body = body;

            bool success = mailManager.SendEmail(email);

            serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
            return success;
        }

        // public bool SendMail(string sender, string recepient, string cc, string subject, string body, out string serverMessage)
        // {
        //     Email email = new Email {From = sender};

        //     email.AddMultipleTo(recepient);
        //     email.AddMultipleCC(cc);
        //     email.Subject = subject;
        //     email.Body = body;

        //     bool success = mailManager.SendEmail(email);

        //     serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
        //     return success;
        // }

        // public bool SendMail(string sender, string recepient, string cc, string bcc, string subject, string body, out string serverMessage)
        // {
        //     Email email = new Email {From = sender};

        //     email.AddMultipleTo(recepient);
        //     email.AddMultipleCC(cc);
        //     email.AddMultipleBcc(bcc);
        //     email.Subject = subject;
        //     email.Body = body;

        //     bool success = mailManager.SendEmail(email);

        //     serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
        //     return success;
        // }

        // public bool SendMail(string sender, string recepient, string subject, string body, StringCollection attachments, bool zipAttachments, out string serverMessage)
        // {
        //     Email email = new Email {From = sender};

        //     email.AddMultipleTo(recepient);
        //     email.Subject = subject;
        //     email.Body = body;

        //     foreach (string attachment in attachments)
        //     {
        //         email.AddFileAttachment(attachment);
        //     }

        //     if (attachments.Count > 0 && zipAttachments)
        //         email.ZipAttachments("attachments.zip");

        //     bool success = mailManager.SendEmail(email);

        //     serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
        //     return success;
        // }

        // public bool SendMail(string sender, string recepient, string cc, string subject, string body, StringCollection attachments, bool zipAttachments, out string serverMessage)
        // {
        //     Email email = new Email {From = sender};

        //     email.AddMultipleTo(recepient);
        //     email.AddMultipleCC(cc);
        //     email.Subject = subject;
        //     email.Body = body;

        //     foreach (string attachment in attachments)
        //     {
        //         email.AddFileAttachment(attachment);
        //     }

        //     if (attachments.Count > 0 && zipAttachments)
        //         email.ZipAttachments("attachments.zip");

        //     bool success = mailManager.SendEmail(email);

        //     serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
        //     return success;
        // }

        // public bool SendMail(string sender, string recepient, string cc, string bcc, string subject, string body, StringCollection attachments, bool zipAttachments, out string serverMessage)
        // {
        //     Email email = new Email {From = sender};

        //     email.AddMultipleTo(recepient);
        //     email.AddMultipleCC(cc);
        //     email.AddMultipleBcc(bcc);
        //     email.Subject = subject;
        //     email.Body = body;

        //     foreach (string attachment in attachments)
        //     {
        //         email.AddFileAttachment(attachment);
        //     }

        //     if (attachments.Count > 0 && zipAttachments)
        //         email.ZipAttachments("attachments.zip");

        //     bool success = mailManager.SendEmail(email);

        //     serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
        //     return success;
        // }

        // public bool SendHtmlMail(string sender, string recepient, string subject, string htmlBody, out string serverMessage)
        // {
        //     Email email = new Email {From = sender};

        //     email.AddMultipleTo(recepient);
        //     email.Subject = subject;
        //     email.AddHtmlAlternativeBody(htmlBody);
        //     email.Body = StringUtils.HtmlToText(htmlBody);

        //     bool success = mailManager.SendEmail(email);

        //     serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
        //     return success;
        // }

        // public bool SendHtmlMail(string sender, string recepient, string cc, string subject, string htmlBody, out string serverMessage)
        // {
        //     Email email = new Email {From = sender};

        //     email.AddMultipleTo(recepient);
        //     email.AddMultipleCC(cc);
        //     email.Subject = subject;
        //     email.AddHtmlAlternativeBody(htmlBody);
        //     email.Body = StringUtils.HtmlToText(htmlBody);

        //     bool success = mailManager.SendEmail(email);

        //     serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
        //     return success;
        // }

        // public bool SendHtmlMail(string sender, string recepient, string cc, string bcc, string subject, string htmlBody, out string serverMessage)
        // {
        //     Email email = new Email {From = sender};

        //     email.AddMultipleTo(recepient);
        //     email.AddMultipleCC(cc);
        //     email.AddMultipleBcc(bcc);
        //     email.Subject = subject;
        //     email.AddHtmlAlternativeBody(htmlBody);
        //     email.Body = StringUtils.HtmlToText(htmlBody);

        //     bool success = mailManager.SendEmail(email);

        //     serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
        //     return success;
        // }

        // public bool SendHtmlMail(string sender, string recepient, string subject, string htmlBody, StringCollection attachments, bool zipAttachments, out string serverMessage)
        // {
        //     Email email = new Email {From = sender};

        //     email.AddMultipleTo(recepient);
        //     email.Subject = subject;
        //     email.AddHtmlAlternativeBody(htmlBody);
        //     email.Body = StringUtils.HtmlToText(htmlBody);

        //     foreach (string attachment in attachments)
        //     {
        //         email.AddFileAttachment(attachment);
        //     }

        //     if (attachments.Count > 0 && zipAttachments)
        //         email.ZipAttachments("attachments.zip");

        //     bool success = mailManager.SendEmail(email);

        //     serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
        //     return success;
        // }

        // public bool SendHtmlMail(string sender, string recepient, string cc, string subject, string htmlBody, StringCollection attachments, bool zipAttachments, out string serverMessage)
        // {
        //     Email email = new Email {From = sender};

        //     email.AddMultipleTo(recepient);
        //     email.AddMultipleCC(cc);
        //     email.Subject = subject;
        //     email.AddHtmlAlternativeBody(htmlBody);
        //     email.Body = StringUtils.HtmlToText(htmlBody);

        //     foreach (string attachment in attachments)
        //     {
        //         email.AddFileAttachment(attachment);
        //     }

        //     if (attachments.Count > 0 && zipAttachments)
        //         email.ZipAttachments("attachments.zip");

        //     bool success = mailManager.SendEmail(email);

        //     serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
        //     return success;
        // }

        // public bool SendHtmlMail(string sender, string recepient, string cc, string bcc, string subject, string htmlBody, StringCollection attachments, bool zipAttachments, out string serverMessage)
        // {
        //     Email email = new Email {From = sender};

        //     email.AddMultipleTo(recepient);
        //     email.AddMultipleCC(cc);
        //     email.AddMultipleBcc(bcc);
        //     email.Subject = subject;
        //     email.AddHtmlAlternativeBody(htmlBody);
        //     email.Body = StringUtils.HtmlToText(htmlBody);

        //     foreach (string attachment in attachments)
        //     {
        //         email.AddFileAttachment(attachment);
        //     }

        //     if (attachments.Count > 0 && zipAttachments)
        //         email.ZipAttachments("attachments.zip");

        //     bool success = mailManager.SendEmail(email);

        //     serverMessage = (success) ? "Success" : "ERROR : \n\n" + mailManager.LastErrorText;
        //     return success;
        // }
    }
}