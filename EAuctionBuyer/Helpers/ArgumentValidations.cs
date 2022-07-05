using System.Text.RegularExpressions;


namespace EAuctionBuyer.Helpers
{
    public static class ArgumentValidations
    {
        public const string motif = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        public static bool ValidateProductName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            else if (name.Length > 30 || name.Length < 5)
                return false;
            else
                return true;

        }

        public static bool ValidateFirstName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            else if (name.Length > 30 || name.Length < 5)
                return false;
            else
                return true;

        }
        public static bool ValidateLastName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            else if (name.Length > 30 || name.Length < 3)
                return false;
            else
                return true;

        }
        public static bool IsValidEmail(string emailaddress)
        {

            if (emailaddress == null || emailaddress == "")
                return false;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emailaddress);
            if (match.Success)
                return true;
            else
                return false;
        }
        
        public static bool ValidatePhoneNumber(string number)
        {
            if (number != null)
                return Regex.IsMatch(number, motif);
            else if (number.Length != 10)
                return false;
            else
                return false;
        }
    }
}
