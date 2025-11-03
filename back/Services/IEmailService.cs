namespace back.Services;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string to, string token);
}

