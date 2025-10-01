namespace BusinessLogic.Interfaces
{
    public interface IMailerSendService
    {
        Task<object?> SendEmailAsync(Repository.Models.MailerSendEmail email);
    }
}

