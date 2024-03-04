using RestSharp;
using RestSharp.Authenticators;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<RestResponse> SendEmailAsync(string to, string subject, string text = "")
    {
        var options = new RestClientOptions("https://api.mailgun.net/v3");
        options.Authenticator = new HttpBasicAuthenticator("api", _configuration["EmailSettings:MailGunApiKey"]!);

        _logger.LogInformation($"Sending email to: {to}, Subject: {subject}");

        var domain = _configuration["EmailSettings:MailGunDomain"];
        var client = new RestClient(options);
        var request = new RestRequest();

        request.AddParameter("domain", domain!, ParameterType.UrlSegment);
        request.Resource = "{domain}/messages";
        request.AddParameter("from", $"GDP <mailgun@{domain}>");
        request.AddParameter("to", $"{to}");
        request.AddParameter("subject", $"{subject}");
        request.AddParameter("text", $"GDP");
        request.AddParameter("html", $"{text}");
        request.Method = Method.Post;

        try
        {
            var response = await client.ExecuteAsync(request);
            _logger.LogInformation($"Email sent. Status code: {response.StatusCode}");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email: {ex.Message}");
            throw;
        }
    }
}
