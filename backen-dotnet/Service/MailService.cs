using backen_dotnet.Data;
using backen_dotnet.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace backen_dotnet.Service
{
    public class MailService : IMailService
    {
        public MailService()
        {
        }

        public async Task<bool> SendMailAsync(MailData mailData)
        {
            try
            {
                var message = new MailMessage()
                {
                    From = new MailAddress(mailData.FromEmail),
                    Subject = mailData.Subject,
                    IsBodyHtml = true,
                    Body = mailData.Body
                };

                foreach (var toEmail in mailData.ToEmails.Split(";"))
                {
                    message.To.Add(new MailAddress(toEmail));
                }

                using var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(mailData.FromEmail, "ywvjnttknbuddlyb"),
                    EnableSsl = true,
                };

                // Utilisation de SendMailAsync pour un envoi asynchrone
                await smtp.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                // Gestion des exceptions (log ou autre traitement)
                Console.WriteLine($"Erreur lors de l'envoi de l'email : {ex.Message}");
                return false;
            }
        }
    }
}
