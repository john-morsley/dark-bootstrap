namespace DarkBootstrap.Controllers;

public class MessageController : Controller
{
    private readonly ILogger<MessageController> _logger;

    private readonly IOptions<SmtpSettings> _smtpSettings;

    public MessageController(ILogger<MessageController> logger, IOptions<SmtpSettings> smtpSettings)
    {
        _logger = logger;
        _smtpSettings = smtpSettings;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send(string name, string message)
    {
        _logger.LogInformation("Message requested from {Name}: {Message}", name, message);
        
        try
        {
            var payload = new
            {
                Name = name,
                Message = message
            };

            //if (response.IsSuccessStatusCode)
            //{
            //    ViewData["MessageSent"] = true;
            //}
            //else
            //{
            //    _logger.LogWarning("Message API returned status code {StatusCode}", response.StatusCode);
            //    ModelState.AddModelError(string.Empty, "There was a problem sending your message. Please try again later.");
            //}
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error trying to send the message");
            ModelState.AddModelError(string.Empty, "There was a problem sending your message. Please try again later.");
        }

        return View();
    }
}