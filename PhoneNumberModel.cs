namespace PhoneNumberDetection
{
    public class PhoneNumberModel
    {
        public string Number { get; set; }
        public string Format { get; set; }

        public PhoneNumberModel(string number, string format)
        {
            Number = number;
            Format = format;
        }
    }
}
