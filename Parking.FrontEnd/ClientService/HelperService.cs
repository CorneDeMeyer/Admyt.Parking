using System.Text.RegularExpressions;

namespace Parking.FrontEnd.ClientService
{
    public static class HelperService
    {
        public static bool ValidLicensePlate(this string plateText)
        {
            var customLastCharacterLicensePlateRegex = "[A-Z]{1,2}$"; // Custom Licence Plate Ensure the last 2 character are letter to a province
            var customFirstCharacterLicensePlateRegex = "[A-Z]{2,}"; // Custom Licence Plate Ensure the first 2 character are letter to a province

            return CheckValidLicensePlate(plateText, customFirstCharacterLicensePlateRegex)
                 || CheckValidLicensePlate(plateText, customLastCharacterLicensePlateRegex);
        }

        private static bool CheckValidLicensePlate(string plateText, string regex)
        {
            var rg = new Regex(regex);

            var match = rg.Match(plateText.ToUpper()
                                          .Replace(" ", string.Empty)
                                          .Replace("-", string.Empty));

            return match.Success;
        }
    }
}
