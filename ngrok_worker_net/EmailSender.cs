using System.Net;
using System.Net.Mail;

namespace ngrok_worker_net;

public class EmailSender
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    public EmailSender(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    
    public void SendMail(string? body)
    {
        MailMessage message = new MailMessage();  
        SmtpClient smtp = new SmtpClient(); 
        message.From = new MailAddress(_configuration["Email:Sender"]);  
        message.To.Add(new MailAddress(_configuration["Email:Recipient"]));  
        message.Subject = "New ngrok url";  
        message.IsBodyHtml = true; //to make message body as html  
        message.Body = body;  
        smtp.Port = Convert.ToInt32(_configuration["Email:Port"]);  
        smtp.Host = _configuration["Email:Host"]; //for gmail host  
        smtp.EnableSsl = true;  
        smtp.UseDefaultCredentials = false;  
        smtp.Credentials = new NetworkCredential(_configuration["Email:Credentials:User"], _configuration["Email:Credentials:Password"]);  
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;  
        smtp.Send(message);
        Console.WriteLine("Sending email");
    }
}