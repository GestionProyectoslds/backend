

using RestSharp;

public interface IEmailService
{
    Task<RestResponse> SendEmailAsync(string to, string subject, string html);
}
