namespace Morsley.UK.DarkBootstrap.UI.Helpers;

public class PhoneHelper
{
    public static bool IsValidPhone(string countryCode, string number)
    {
        if (string.IsNullOrWhiteSpace(number)) return true; // Empty is valid (optional field)

        number = number.Trim();
        countryCode = countryCode?.Trim() ?? string.Empty;

        // Combine country code and number
        var fullNumber = countryCode + number;

        // Try to parse the phone number - if it parses successfully, it's valid
        if (PhoneNumber.TryParse(fullNumber, out PhoneNumber? phoneNumber))
        {
            return phoneNumber != null;
        }

        return false;
    }
}