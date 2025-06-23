
using backen_dotnet.Data;
using Microsoft.AspNetCore.Mvc;

namespace backen_dotnet.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(MailData mailData);

    }
}
