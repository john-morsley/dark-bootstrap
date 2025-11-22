namespace Morsley.UK.DarkBootstrap.UI.Models;

public class SendMessageViewModel
{
    public string Name { get; set; } = string.Empty;

    public string? EmailAddress { get; set; }

    public string? DetectedCountryCode { get; set; }

    public string? SelectedCountryCode { get; set; }

    public string? MobileNumber { get; set; }

    public string Message { get; set; } = string.Empty;

    public IEnumerable<Problem> Problems { get; set; } = Enumerable.Empty<Problem>();

    public string? SendOutcomeStatus { get; set; }

    public string? SendOutcomeMessage { get; set; }

    public bool IsValidated { get; set; } = false;
}