using SendGrid;
using SendGrid.Helpers.Mail;

namespace SengridApi;

public class SendGridTools{

    private string _apiKey; 

    public SendGridTools(string apiKey){
        this._apiKey = apiKey;
    }

    public async Task<string> SendEmail(string fromEmail, string toEmail, string templateUrl, string toName)
    {
        var client = new SendGridClient(this._apiKey);
        var data =new {};
        var from = new EmailAddress(fromEmail, "Panamify");
        var to = new EmailAddress(toEmail);
        var subject = $"Hi {toName}";
        var plainContext = "123";
        var htmlContent = File.ReadAllText(templateUrl);
        htmlContent = htmlContent.Replace("---Nombre---", toName);
        var ms = MailHelper.CreateSingleEmail(from, to, subject, plainContext, htmlContent);
        var response = await client.SendEmailAsync(ms);

        return response.StatusCode.ToString();
    }

}