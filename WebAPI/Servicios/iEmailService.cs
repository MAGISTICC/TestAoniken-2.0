using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Threading.Tasks;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Your App Name", "marconatoll2005@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", toEmail));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("plain") { Text = message };

        using (var client = new SmtpClient())
        {
            client.Connect(_configuration["Email:SmtpServer"], int.Parse(_configuration["Email:SmtpPort"]), true);
            client.Authenticate(_configuration["Email:SmtpUser"], _configuration["Email:SmtpPass"]);
            await client.SendAsync(emailMessage);
            client.Disconnect(true);
        }
    }
}
