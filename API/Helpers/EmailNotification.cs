using System.Net.Mail;

namespace API.Helpers
{
    public class EmailNotification
    {
          public void Sendmail(string emailBody,string emailTo)
        {
            MailMessage mailMessage = new MailMessage("varuntestrider@gmail.com",emailTo);
            mailMessage.Subject = "ZipFileNotifications";
            mailMessage.Body = emailBody;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "varuntestrider@gmail.com",
                Password = "horserider28"
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }
    }
}