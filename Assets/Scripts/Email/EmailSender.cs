using System.Net;
using System.Net.Mail;

public class EmailSender {
    public static void SendEmail(string messageBody, string emailSubject) {
        // Set the email properties
        string fromEmail = "astroballhelp@outlook.com";
        string fromEmailPassword = "rzYVyjrHvNFE8yN";
        string toEmail = "astroballhelp@outlook.com";

        // Set up the SMTP client
        SmtpClient client = new SmtpClient("smtp.office365.com", 587);
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(fromEmail, fromEmailPassword);
        client.EnableSsl = true;


        // Set up the email message
        MailMessage message = new MailMessage(fromEmail, toEmail, emailSubject, messageBody);
        message.IsBodyHtml = false;

        // Send the email
        client.Send(message);
    }
}