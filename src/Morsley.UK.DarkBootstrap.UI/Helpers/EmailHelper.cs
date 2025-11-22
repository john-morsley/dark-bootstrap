namespace Morsley.UK.DarkBootstrap.UI.Helpers;

public class EmailHelper
{
    public static bool IsValidEmail(string value)
    {
        value = value.Trim();

        if (string.IsNullOrWhiteSpace(value)) return false;

        return EmailValidator.Validate(value, allowTopLevelDomains: true, allowInternational: true);
    }
}