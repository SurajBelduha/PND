using System.Text.RegularExpressions;

namespace PhoneNumberDetection
{
    public static class PhoneNumberExtractor
    {
        // Define a dictionary to map English words to digits
        private static readonly Dictionary<string, string> EnglishWordToDigit = new Dictionary<string, string>()
        {
            { "zero", "0" }, { "one", "1" }, { "two", "2" }, { "three", "3" }, { "four", "4" },
            { "five", "5" }, { "six", "6" }, { "seven", "7" }, { "eight", "8" }, { "nine", "9" }
        };

        // Define a dictionary to map Hindi words to digits
        private static readonly Dictionary<string, string> HindiWordToDigit = new Dictionary<string, string>()
        {
            { "शून्य", "0" }, { "एक", "1" }, { "दो", "2" }, { "तीन", "3" }, { "चार", "4" },
            { "पांच", "5" }, { "छह", "6" }, { "सात", "7" }, { "आठ", "8" }, { "नौ", "9" }
        };

        // Define a list of tuples, each containing a regex pattern and a description for different phone number formats.
        private static readonly List<Tuple<string, string>> Patterns = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>(@"\+?\d{1,3}[- ]?\(?\d{1,3}\)?[- ]?\d{3}[- ]?\d{4}", "Country Code"),
            new Tuple<string, string>(@"\(?\d{2,3}\)?[- ]?\d{3}[- ]?\d{4}", "With Parentheses"),
            new Tuple<string, string>(@"\d{3}[- ]?\d{3}[- ]?\d{4}", "10-digit"),
            new Tuple<string, string>(@"\(?\d{2,3}\)?\s?\d{5,10}", "Without Country Code"),
            new Tuple<string, string>(BuildWordPattern(EnglishWordToDigit.Keys), "English Words"),
            new Tuple<string, string>(BuildWordPattern(HindiWordToDigit.Keys), "Hindi Words")
        };

        public static List<PhoneNumberModel> ExtractPhoneNumbers(string inputText)
        {
            // Initialize a list to store detected phone numbers
            var phoneNumbers = new List<PhoneNumberModel>();

            // Iterate over each pattern defined in the Patterns list
            foreach (var pattern in Patterns)
            {
                // Check if the pattern is for English or Hindi words
                if (pattern.Item2 == "English Words" || pattern.Item2 == "Hindi Words")
                {
                    // Extract numbers based on the word patterns
                    var detectedNumbers = ExtractWordBasedNumbers(inputText, pattern.Item2 == "English Words" ? EnglishWordToDigit : HindiWordToDigit);
                    // For each detected number, check its length
                    foreach (var number in detectedNumbers)
                    {
                        if (number.Length == 10 || number.Length == 12) 
                            phoneNumbers.Add(new PhoneNumberModel(number, $"{pattern.Item2} as Digits"));
                    }
                }
                else
                {
                    // For other patterns, use regular expressions to find matches in the input text
                    MatchCollection matches = Regex.Matches(inputText, pattern.Item1, RegexOptions.IgnoreCase);

                    // Add each matched phone number to the list
                    foreach (Match match in matches)
                    {
                        phoneNumbers.Add(new PhoneNumberModel(match.Value, pattern.Item2));
                    }
                }
            }
            return phoneNumbers;
        }

        // Join the list of words into a single string
        private static string BuildWordPattern(IEnumerable<string> words)
        {
            return @"\b(?:" + string.Join("|", words.Select(Regex.Escape)) + @")\b";
        }

        private static List<string> ExtractWordBasedNumbers(string text, Dictionary<string, string> wordToDigit)
        {
            var results = new List<string>();
            try
            {
                // Split the input text into words based on common delimiters
                string[] words = text.Split(new[] { ' ', '-', ',', '(', ')', '.' }, StringSplitOptions.RemoveEmptyEntries);
                var digitList = new List<string>();
                foreach (var word in words)
                {
                    // Convert the word to lowercase to handle
                    string lowerWord = word.ToLower();
                    // Check if the current word can be mapped to a digit
                    if (wordToDigit.ContainsKey(lowerWord))
                    {
                        digitList.Add(wordToDigit[lowerWord]);
                    }
                    else if (digitList.Count > 0)
                    {
                        // If the word is not a number word and digit list is not empty, 
                        // concatenate the digits collected so far into a number string
                        results.Add(string.Join("", digitList));

                        // Clear the digit list for the next potential number sequence
                        digitList.Clear();
                    }
                }
                // After processing all words, if there are still digits in the list, add them as a number
                if (digitList.Count > 0)
                    results.Add(string.Join("", digitList));

            }
            catch (Exception)
            {
                Console.WriteLine($"Something went wrong please contact administrator");
            }
            return results;
        }

       
        /// <summary>
        /// This  method takes string parameter
        /// New method to convert mixed language to word sequences
        /// Overall, this method is Return the list of numbers found 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<string> ExtractMixedLanguageWordNumbers(string text)
        {
            var combinedWordToDigit = EnglishWordToDigit.Concat(HindiWordToDigit).ToDictionary(x => x.Key, x => x.Value);
            return ExtractWordBasedNumbers(text, combinedWordToDigit);
        }
    }
}

