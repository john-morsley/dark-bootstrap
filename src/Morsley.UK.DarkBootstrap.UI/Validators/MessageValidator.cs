namespace Morsley.UK.DarkBootstrap.UI.Validators;

public class MessageValidator
{
    private const int MaximumNameLength = 100;
    private const int MaximumMessageLength = 5000;
    private const int MinimumMessageLength = 10;

    public IEnumerable<Problem> Validate(SendMessageViewModel message)
    {
        var problems = new List<Problem>();

        ValidateName(message.Name, problems);        
        ValidateEmailAddress(message.EmailAddress, problems);
        ValidateMobileNumber(message.SelectedCountryCode, message.MobileNumber, problems);
        ValidateMessage(message.Message, problems);

        message.IsValidated = true;

        return problems;
    }

    private void ValidateName(string name, List<Problem> problems)
    {
        var issues = new List<string>();

        if (string.IsNullOrWhiteSpace(name))
        {
            issues.Add("Name is mandatory.");
        }
        else if (name.Length > MaximumNameLength)
        {
            issues.Add($"Name must not exceed {MaximumNameLength} characters.");
        }

        if (issues.Any())
        {
            problems.Add(new Problem
            {
                Item = "Name",
                Issues = issues
            });
        }
    }

    private void ValidateMessage(string message, List<Problem> problems)
    {
        var issues = new List<string>();

        if (string.IsNullOrWhiteSpace(message))
        {
            issues.Add("Message is mandatory.");
        }
        else
        {
            if (message.Length < MinimumMessageLength)
            {
                issues.Add($"Message must be at least {MinimumMessageLength} characters.");
            }

            if (message.Length > MaximumMessageLength)
            {
                issues.Add($"Message must not exceed {MaximumMessageLength} characters.");
            }
        }

        if (issues.Any())
        {
            problems.Add(new Problem
            {
                Item = "Message",
                Issues = issues
            });
        }
    }

    private void ValidateEmailAddress(string? emailAddress, List<Problem> problems)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            return; // Email is optional
        }

        var issues = new List<string>();

        if (!EmailHelper.IsValidEmail(emailAddress))
        {
            issues.Add("Email Address is not valid.");
        }

        if (issues.Any())
        {
            problems.Add(new Problem
            {
                Item = "Email Address",
                Issues = issues
            });
        }
    }

    private void ValidateMobileNumber(string? countryCode, string? mobileNumber, List<Problem> problems)
    {
        if (string.IsNullOrWhiteSpace(mobileNumber))
        {
            return; // Mobile is optional
        }

        var issues = new List<string>();

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            issues.Add("Country Code is required when providing a mobile number.");
        }
        else if (!PhoneHelper.IsValidPhone(countryCode, mobileNumber))
        {
            issues.Add("Mobile Number is not valid for the selected country code.");
        }

        if (issues.Any())
        {
            problems.Add(new Problem
            {
                Item = "Mobile Number",
                Issues = issues
            });
        }
    }
}
