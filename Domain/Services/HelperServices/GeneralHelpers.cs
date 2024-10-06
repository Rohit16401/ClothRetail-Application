using System.Text.RegularExpressions;

namespace Domain.Services.HelperServices
{
    public static class GeneralHelpers
    {
        public static (string Text, string Number) SeparateTextAndNumber(this string input)
        {
            // Define regular expressions for text and numbers
            var textPattern = @"^[^\d]+";
            var numberPattern = @"\d+$";

            // Extract text and number using regex
            string text = Regex.Match(input, textPattern).Value;
            string number = Regex.Match(input, numberPattern).Value;

            return (text, number);
        }

        public static DateTime? ParseDateTimeOrNull(this string dateTimeString)
        {
            // Check if the input string is null or empty
            if (string.IsNullOrWhiteSpace(dateTimeString))
            {
                return null; // Return null if the string is null, empty, or consists only of white-space characters
            }

            // Try parsing the string to a DateTime
            if (DateTime.TryParse(dateTimeString, out DateTime result))
            {
                return result; // Return the parsed DateTime
            }

            return null; // Return null if parsing fails
        }

        public static TimeSpan? ParseTimespanOrNull(this string timeSpanString)
        {
            // Check if the input string is null or empty
            if (string.IsNullOrWhiteSpace(timeSpanString))
            {
                return null; // Return null if the string is null, empty, or consists only of white-space characters
            }

            // Try parsing the string to a TimeSpan
            if (TimeSpan.TryParse(timeSpanString, out TimeSpan result))
            {
                return result; // Return the parsed TimeSpan
            }

            return null; // Return null if parsing fails
        }

        public static TimeSpan? ParseTimespanOrNull1(this string timeSpan)
        {
            // Convert string to TimeSpan? if needed
            TimeSpan? correctedOverTimeFromRequest = string.IsNullOrEmpty(timeSpan)
                ? (TimeSpan?)null
                : TimeSpan.Parse(timeSpan);
            return correctedOverTimeFromRequest;
        }
    }
}
