namespace Morsley.UK.DarkBootstrap.UI.Helpers;

public class PhoneHelper
{
    public static bool IsValidPhone(string countryCode, string number)
    {
        if (string.IsNullOrWhiteSpace(number)) return true; // Empty is valid (optional field)

        number = number.Trim();
        countryCode = countryCode?.Trim() ?? string.Empty;

        var fullNumber = countryCode + number;

        if (PhoneNumber.TryParse(fullNumber, out PhoneNumber? phoneNumber))
        {
            return phoneNumber != null;
        }

        return false;
    }
}