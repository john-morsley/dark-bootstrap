namespace Morsley.UK.DarkBootstrap.UI.Controllers;

public class MessageController : Controller
{
    private readonly ILogger<MessageController> _logger;

    private readonly IOptions<SmtpSettings> _smtpSettings;

    public MessageController(ILogger<MessageController> logger, IOptions<SmtpSettings> smtpSettings)
    {
        _logger = logger;
        _smtpSettings = smtpSettings;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var detectedCountryCode = DetectCountryCodeFromRequest();
        var sendMessage = new SendMessageViewModel
        {
            DetectedCountryCode = detectedCountryCode
        };
        return View(sendMessage);
    }

    private string? DetectCountryCodeFromRequest()
    {
        var countryToDialCode = new Dictionary<string, string>
        {
            { "GB", "+44" }, { "UK", "+44" },
            { "US", "+1" }, { "CA", "+1" },
            { "AU", "+61" },
            { "NZ", "+64" },
            { "IE", "+353" },
            { "FR", "+33" },
            { "DE", "+49" },
            { "ES", "+34" },
            { "IT", "+39" },
            { "NL", "+31" },
            { "BE", "+32" },
            { "CH", "+41" },
            { "SE", "+46" },
            { "NO", "+47" },
            { "DK", "+45" },
            { "FI", "+358" },
            { "PT", "+351" },
            { "GR", "+30" },
            { "PL", "+48" },
            { "IN", "+91" },
            { "CN", "+86" },
            { "JP", "+81" },
            { "KR", "+82" },
            { "SG", "+65" },
            { "HK", "+852" },
            { "ZA", "+27" },
            { "BR", "+55" },
            { "MX", "+52" },
            { "AR", "+54" }
        };

        // Get Accept-Language header
        var acceptLanguage = Request.Headers["Accept-Language"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(acceptLanguage))
        {
            return null;
        }

        // Parse the Accept-Language header (format: "en-GB,en;q=0.9,en-US;q=0.8")
        var languages = acceptLanguage.Split(',')
            .Select(lang => lang.Split(';')[0].Trim())
            .ToList();

        foreach (var language in languages)
        {
            if (language.Contains('-'))
            {
                var parts = language.Split('-');
                if (parts.Length == 2)
                {
                    var countryCode = parts[1].ToUpperInvariant();
                    if (countryToDialCode.TryGetValue(countryCode, out var dialCode))
                    {
                        return dialCode;
                    }
                }
            }
        }

        return null;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send(SendMessageViewModel sendMessage)
    {
        _logger.LogInformation("Message requested from {Name}: {Message}", sendMessage.Name, sendMessage.Message);

        if (sendMessage.DetectedCountryCode != sendMessage.SelectedCountryCode)
        {
            sendMessage.DetectedCountryCode = sendMessage.SelectedCountryCode;
        }

        if (!IsSendMessageIsValid(sendMessage))
        {            
            return View("Index", sendMessage);
        }

        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_smtpSettings.Value.FromName, _smtpSettings.Value.FromAddress));
            emailMessage.To.Add(new MailboxAddress(_smtpSettings.Value.ToName, _smtpSettings.Value.ToAddress));
            emailMessage.Subject = $"Message from {sendMessage.Name}";

            var bodyText = $"From: {sendMessage.Name}";

            if (!string.IsNullOrWhiteSpace(sendMessage.EmailAddress))
            {
                bodyText += $"\nEmail: {sendMessage.EmailAddress}";
            }

            if (!string.IsNullOrWhiteSpace(sendMessage.SelectedCountryCode) && !string.IsNullOrWhiteSpace(sendMessage.MobileNumber))
            {
                bodyText += $"\nMobile: {sendMessage.SelectedCountryCode} {sendMessage.MobileNumber}";
            }

            bodyText += $"\n\nMessage:\n{sendMessage.Message}";

            var bodyBuilder = new BodyBuilder
            {
                TextBody = bodyText
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();

            if (_smtpSettings.Value.SkipCertificateValidation)
            {
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
            }

            await smtpClient.ConnectAsync(
                _smtpSettings.Value.Server, 
                _smtpSettings.Value.Port, 
                _smtpSettings.Value.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);

            await smtpClient.AuthenticateAsync(
                _smtpSettings.Value.Username, 
                _smtpSettings.Value.Password);

            await smtpClient.SendAsync(emailMessage);
            await smtpClient.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully to {ToAddress}", _smtpSettings.Value.ToAddress);

            sendMessage.SendOutcomeStatus = "Success";
            sendMessage.SendOutcomeMessage = "Message sent successfully.";
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error trying to send the message");
            sendMessage.SendOutcomeStatus = "Failure";
            sendMessage.SendOutcomeMessage = "There was a problem sending your message. Please try again later.";
        }

        return View("Index", sendMessage);
    }

    private bool IsSendMessageIsValid(SendMessageViewModel sendMessage)
    {
        var validator = new MessageValidator();
        var problems = validator.Validate(sendMessage);

        sendMessage.Problems = problems;

        return !problems.Any();        
    }
}