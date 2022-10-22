using SendGrid;
using SendGrid.Helpers.Mail;

namespace SengridApi;

public class SendGridTools{

    private string _apiKey; 

    public SendGridTools(string apiKey){
        this._apiKey = apiKey;
    }

    public async Task<string> SendEmail(string fromEmail, string toEmail, string templateUrl, string AccountName)
    {
        var client = new SendGridClient(this._apiKey);
        var data =new {};
        var from = new EmailAddress(fromEmail, "Panamify");
        var to = new EmailAddress(toEmail);
        var subject = $"Hi";
        var plainContext = "123";
        var htmlContent = File.ReadAllText(templateUrl);
        //htmlContent = htmlContent.Replace("Name Account", AccountName);
        //htmlContent = htmlContent.Replace("linkedinlink", AccountName.Replace(" ","").ToLower());
        var ms = MailHelper.CreateSingleEmail(from, to, subject, plainContext, htmlContent);
        var response = await client.SendEmailAsync(ms);

        return response.StatusCode.ToString();
    }

}