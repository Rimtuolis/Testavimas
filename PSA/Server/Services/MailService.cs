using Microsoft.Extensions.Options;
using PSA.Shared;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace PSA.Server.Services
{
    public class MailService : IMailService
    {
        private readonly MailOptions _mailOptions;

        public MailService(IOptions<MailOptions> mailOptions)
        {
            _mailOptions = mailOptions.Value;
        }

        public async Task SendEmailAsync(string subject, string name, string email, string message, string type = "plain")
        {
            if (!_mailOptions.UseSmtp4Dev)
            {
                throw new NotImplementedException(
                    "Other SMTP hosts are not supported yet. Install and open Smtp4Dev (https://github.com/rnwood/smtp4dev)" +
                    "or implement usage with another external host, such as Google.");
            }

            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(_mailOptions.SenderName, _mailOptions.SenderEmail));
            mimeMessage.To.Add(new MailboxAddress(name, email));
            mimeMessage.Subject = subject;

            mimeMessage.Body = new TextPart(type)
            {
                Text = message
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(_mailOptions.Host, _mailOptions.Port, SecureSocketOptions.None);

            client.Capabilities &= ~SmtpCapabilities.Pipelining;

            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }

        public async Task SendInvoice(Shared.Client? client, Contract? contract, Manager? manager, Worker? worker)
        {
            if (string.IsNullOrWhiteSpace(manager.vardas) ||
                string.IsNullOrWhiteSpace(manager.pavarde) ||
                string.IsNullOrWhiteSpace(worker.vardas) ||
                string.IsNullOrWhiteSpace(worker.pavarde) ||
                string.IsNullOrWhiteSpace(client.vardas) ||
                string.IsNullOrWhiteSpace(client.el_pastas))
            {
                return;
            }

            string html = $"<html> <h1> Invoice #{contract.id_Sutartis} </h1> <p> Thank you for ordering! </p> <h3> Signed by manager {manager.vardas} {manager.pavarde} </h3> <h3> Shipped by worker {worker.vardas} {worker.pavarde} </h3> </html>";

            await SendEmailAsync($"Invoice #{contract.id_Sutartis}", client.vardas, client.el_pastas, html, "html");
        }
    }
}