namespace Morsley.UK.DarkBootstrap.UI.Models;

public class SmtpSettings
{
    public string Server { get; set; } = string.Empty;

    public int Port { get; set; }

    public bool UseSsl { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FromName { get; set; } = string.Empty;

    public string FromAddress { get; set; } = string.Empty;

    public string ToName { get; set; } = string.Empty;

    public string ToAddress { get; set; } = string.Empty;

    public bool SkipCertificateValidation { get; set; }
}