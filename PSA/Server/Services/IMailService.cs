using PSA.Shared;

namespace PSA.Server.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string subject, string name, string email, string message, string type = "plain");

        Task SendInvoice(Shared.Client? client, Contract? contract, Manager? manager, Worker? worker);
    }
}