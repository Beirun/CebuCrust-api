namespace CebuCrust_api.Interfaces
{
    public interface IEmailService
    {
        Task SendResetEmailAsync(string toEmail, string resetCode);
    }
}
