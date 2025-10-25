namespace PasswordValidatorLoopsandif;

class Program
{
    static void Main(string[] args)
    {
        while (true) 
        {
            Console.Write("Enter a test password: ");
            string password = Console.ReadLine();

            bool hasUppercase = false;
            bool hasLowercase = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;
            bool isLongEnough = (password.Length >= 8);

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUppercase = true;
                }
                else if (char.IsLower(c))
                {
                    hasLowercase = true;
                }
                else if (char.IsDigit(c))
                {
                    hasDigit = true;
                }
                else if (!char.IsLetterOrDigit(c))
                {
                    hasSpecialChar = true;
                }
            }

            Console.WriteLine("--- Validation Report ---");
            Console.WriteLine($"Is >= 8 Chars:    {isLongEnough}");
            Console.WriteLine($"Has Uppercase:    {hasUppercase}");
            Console.WriteLine($"Has Lowercase:    {hasLowercase}");
            Console.WriteLine($"Has Digit:          {hasDigit}");
            Console.WriteLine($"Has Special Char:   {hasSpecialChar}");

            if (isLongEnough && hasUppercase && hasLowercase && hasDigit && hasSpecialChar)
            {
                Console.WriteLine("Validation SUCCEEDED!");
                break; 
            }
            else
            {
                Console.WriteLine("Validation FAILED!");
            }
        }
    
        Console.WriteLine("Program will now exit.");
    }
}
