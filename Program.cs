


namespace PhoneNumberDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputText = GetInputText(args);
                List<PhoneNumberModel> phoneNumbers = PhoneNumberExtractor.ExtractPhoneNumbers(inputText);
                // Add detection for mixed language word sequences
                phoneNumbers.AddRange(PhoneNumberExtractor.ExtractMixedLanguageWordNumbers(inputText).Select(number => new PhoneNumberModel(number, "Mixed Language")));
                DisplayPhoneNumbers(phoneNumbers);
            }
            catch (Exception)
            {
                Console.WriteLine($"Something went wrong please contact administrator");
            }

        }

        static string GetInputText(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    if (File.Exists(args[0]))
                        return File.ReadAllText(args[0]);
                    else
                    {
                        Console.WriteLine("File not found. Please provide valid file path.");
                        Environment.Exit(1);
                    }
                }
                Console.WriteLine("Enter text to extract phone numbers:");
            }
            catch (Exception)
            {
                Console.WriteLine($"Something went wrong please contact administrator");
            }
            return Console.ReadLine();
        }

        /// <summary>     
        ///  This  method takes one parameter: PhoneNumberModel 
        ///   Overall, this method is used Display all detected phone numbers
        /// </summary>
        /// <param name="phoneNumbers"></param>

        static void DisplayPhoneNumbers(List<PhoneNumberModel> phoneNumbers)
        {
            try
            {
                if (phoneNumbers.Any())
                {
                    Console.WriteLine("Detected Phone Numbers:");
                    Console.WriteLine("");
                    foreach (var phoneNumber in phoneNumbers)
                        Console.WriteLine($"Number: {phoneNumber.Number}, Format: {phoneNumber.Format}");
                }
                else
                    Console.WriteLine("No phone numbers detected.");
            }
            catch (Exception)
            {
                Console.WriteLine($"Something went wrong please contact administrator");
            }
        }
    }
}



