using System.Net.Mail;

namespace Morsley.UK.DarkBootstrap.UI.Helpers;

public class EmailHelper
{
    public static bool IsValidEmail(string value)
    {
        value = value.Trim();

        if (string.IsNullOrWhiteSpace(value)) return false;

        // Must contain @ symbol
        if (!value.Contains('@')) return false;

        try
        {
            var mailAddress = new MailAddress(value);
            // Ensure the parsed address matches exactly (no display name)
            // and that the domain has a proper TLD (contains a dot)
            return mailAddress.Address == value &&
                   mailAddress.Host.Contains('.') &&
                   !mailAddress.Host.StartsWith('.') &&
                   !mailAddress.Host.EndsWith('.') &&
                   mailAddress.Host.Split('.').Length >= 2 &&
                   mailAddress.Host.Split('.').All(part => !string.IsNullOrWhiteSpace(part));
        }
        catch
        {
            return false;
        }
    }
}