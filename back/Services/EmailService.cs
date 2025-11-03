using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace back.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendPasswordResetEmailAsync(string to, string token)
    {
        try
        {
            var host = _configuration["Email:Host"] ?? "smtp.gmail.com";
            var port = int.Parse(_configuration["Email:Port"] ?? "587");
            var username = _configuration["Email:Username"] ?? string.Empty;
            var password = _configuration["Email:Password"] ?? string.Empty;
            var fromEmail = _configuration["Email:FromEmail"] ?? username;
            var resetUrl = _configuration["ResetPasswordUrl"] + token;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sistema", fromEmail));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = "Solicitud de Restablecimiento de Contraseña";

            var bodyBuilder = new BodyBuilder
            {
                TextBody = $@"Hola,

Has solicitado restablecer tu contraseña. Haz clic en el siguiente enlace para continuar:
{resetUrl}

Si no solicitaste esto, por favor ignora este correo.

Gracias."
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar el correo de restablecimiento");
            throw;
        }
    }
}

