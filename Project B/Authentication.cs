using System.Net.Mail;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

// Authentication class
public static class Authentication
{
    public static Account? User { get; private set; }

    // Starts the authentication process
    public static Account? Start()
    {

        while (true)
        {
            string[] options = new string[] { "1.Inloggen            ", "2.Registreren         ", "3.Terug naar hoofdmenu" };
            int userAction = ProgramFunctions.ShowMenuInline(options);
            if (userAction == 0)
                try
                {
                    Console.Clear();
                    return Login();
                }
                catch (Exception e)
                {
                    AsciiArtPrinter.PrintAsciilogin();
                    Console.WriteLine();
                    ProgramFunctions.PrintTextCentered(e.Message);
                }
            else if (userAction == 1)
            {
                try
                {
                    Console.Clear();
                    return Register();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            else
            {
                Console.Clear();
                return null;
            }

        }
    }

    // Starts the Login process
    public static Account Login()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        AsciiArtPrinter.PrintAsciiInlog();
        Console.ResetColor();
        Console.WriteLine("E-mailadres:");
        string email = (Console.ReadLine() ?? "").ToLower();
        Console.Clear(); // Clear the console after reading email
        Console.ForegroundColor = ConsoleColor.Yellow;
        AsciiArtPrinter.PrintAsciiInlog();
        Console.ResetColor();
        Console.WriteLine("Wachtwoord:");
        string password = ReadPassword();
        Console.Clear();

        // Searches for account that has the correct email
        Account? foundAccount = GetAccountByEmail(email) ?? throw new Exception("Aanmeldgegevens niet gevonden");

        // checks the password hash on the found account
        if (!foundAccount.TestPassword(HashPassword(password)))
        {
            throw new Exception("Aanmeldgegevens niet gevonden");
        }

        // sets User property and returns User
        User = foundAccount;
        return foundAccount;
    }

    // Starts the registration process
    public static Account Register()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        AsciiArtPrinter.PrintAsciiRegister();
        Console.ResetColor();
        // calls email checker
        string email = RegisterEmail();
        Console.Clear(); // Clear the console after reading email

        // hashes returned string of the confirm password process function
        string password = HashPassword(RegisterConfirmPassword());
        Console.Clear(); // Clear the console after confirming password
        Console.ForegroundColor = ConsoleColor.Yellow;
        AsciiArtPrinter.PrintAsciiRegister();
        Console.ResetColor();
        Console.WriteLine("Voornaam:");
        string firstName = Console.ReadLine() ?? "";
        Console.Clear();

        while (!IsValidFirstName(firstName))
        {
            Console.WriteLine("Ongeldige voornaam. Voer een geldige voornaam in die alleen letters bevat:");
            firstName = Console.ReadLine() ?? "";
            Console.Clear();
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        AsciiArtPrinter.PrintAsciiRegister();
        Console.ResetColor();
        Console.WriteLine("Achternaam:");
        string lastName = Console.ReadLine() ?? "";
        Console.Clear();

        string birthdate = RegisterBirthdate();
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Yellow;
        AsciiArtPrinter.PrintAsciiRegister();
        Console.ResetColor();

        bool admin = false;
        if (email.Contains("@admin"))
        {
            string adminCode = "admin123";
            Console.WriteLine("Geef een admin code in indien van toepassing:");

            string userAdminCode = Console.ReadLine() ?? "";
            admin = userAdminCode == adminCode;
        }
        Console.Clear();

        // Creates new object
        Account account = new(email, password, firstName, lastName, birthdate, admin);
        SaveNewAccount(account);

        // save account to property and return account
        User = account;
        return account;
    }

    // View the information of currently logged in account
    public static void ViewProfile()
    {
        while (true)
        {
            if (User != null)
            {
                Console.WriteLine();
                string[] userStrings = User.ToString().Split("\n");
                int longestUserString = userStrings.OrderByDescending(x => x.Length).First().Length;
                int leftPadding = (Console.WindowWidth - longestUserString) / 2;
                foreach (string userString in userStrings)
                {
                    Console.CursorLeft = leftPadding;
                    Console.WriteLine($"{userString}");
                }
            }

            string[] options = new string[] { "1.Uitloggen           ", "2.Profiel aanpassen   ", "3.Terug naar hoofdmenu" };
            int userAction = ProgramFunctions.ShowMenuInline(options);
            switch (userAction)
            {
                case 0:
                    Console.Clear();
                    Logout();
                    return;

                case 1:
                    EditProfile();
                    break;

                case 2:
                    Console.Clear();
                    return;
            }
        }
    }

    // resets User property
    public static void Logout()
    {
        User = null;
    }

    // Reads the password input while not showing the typed characters by visually replacing them with *
    private static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Ignore any key that's not a printable character or Enter
            if (char.IsControl(key.KeyChar) && key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                continue;

            // Handle backspace
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b"); // Erase the character from the screen
            }
            else if (key.Key != ConsoleKey.Enter) // Print asterisks for all other characters
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Move to the next line after user presses Enter
        return password;
    }

    // Checks if the email is valid
    private static string RegisterEmail()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("E-mailadres:");
                string email = (Console.ReadLine() ?? "").ToLower();
                Console.Clear();

                if (email == "")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    AsciiArtPrinter.PrintAsciiRegister();
                    Console.ResetColor();
                    throw new Exception("Voer een e-mailadres in");
                }

                if (IsValidEmail(email))
                {
                    if (GetAccountByEmail(email) == null)
                    {
                        return email;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        AsciiArtPrinter.PrintAsciiRegister();
                        Console.ResetColor();
                        throw new Exception("E-mailadres is al in gebruik");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    AsciiArtPrinter.PrintAsciiRegister();
                    Console.ResetColor();
                    throw new Exception("Ongeldig e-mailadres of domein.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }


    // asks for password twice and checks if they match
    private static string RegisterConfirmPassword(bool showArt = true)
    {
        while (true)
        {
            try
            {
                bool allowed = false;
                string errorMsg = "";
                string password;
                do
                {
                    if (showArt)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    AsciiArtPrinter.PrintAsciiRegister();
                    Console.ResetColor();
                    if (errorMsg != "")
                    {
                        Console.Write(errorMsg);
                        errorMsg = "";
                    }

                    Console.WriteLine("Wachtwoord:");
                    password = ReadPassword();
                    Console.Clear();

                    if (password.Length < 8)
                        errorMsg += "Wachtwoord moet 8 of langer karakters zijn.\n";
                    if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                        errorMsg += "Wachtwoord heeft geen symbolen.\n";
                    if (!password.Any(char.IsUpper))
                        errorMsg += "Wachtwoord heeft geen hoofdletters.\n";
                    if (!password.Any(char.IsNumber))
                        errorMsg += "Wachtwoord heeft geen cijfers.\n";

                    if (password.Length >= 8 && password.Any(ch => !char.IsLetterOrDigit(ch)) && password.Any(ch => !char.IsUpper(ch)) && password.Any(ch => !char.IsNumber(ch)))
                        allowed = true;
                } while (allowed == false);
                Console.ForegroundColor = ConsoleColor.Yellow;
                AsciiArtPrinter.PrintAsciiRegister();
                Console.ResetColor();
                Console.WriteLine("Bevestig wachtwoord:");
                string passwordConfirm = ReadPassword();
                if (password != passwordConfirm)
                {
                    Console.Clear();
                    throw new Exception("Wachtwoorden komen niet overeen");
                }

                else
                    return password;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Clear();
            }
        }
    }

    // returns hash of string
    public static string HashPassword(string password)
    {
        byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        // Convert the byte array to a hexadecimal string
        StringBuilder builder = new();
        for (int i = 0; i < hashedBytes.Length; i++)
        {
            builder.Append(hashedBytes[i].ToString("x2"));
        }
        return builder.ToString();
    }

    // Checks if birthday is valid date
    private static string RegisterBirthdate()
    {
        while (true)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                AsciiArtPrinter.PrintAsciiRegister();
                Console.ResetColor();
                Console.WriteLine("Geboortedatum: Voorbeeld:(20-10-1998):");
                string birthdate = Console.ReadLine() ?? "";
                Console.Clear();
                DateTime birthdatetime;
                if (!DateTime.TryParseExact(birthdate, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out birthdatetime) && !DateTime.TryParseExact(birthdate, "d-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out birthdatetime) && !DateTime.TryParseExact(birthdate, "d-M-yyyy", null, System.Globalization.DateTimeStyles.None, out birthdatetime) && !DateTime.TryParseExact(birthdate, "dd-M-yyyy", null, System.Globalization.DateTimeStyles.None, out birthdatetime))
                    throw new Exception("Ongeldige geboortedatum");
                else
                    return birthdate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
    // Validates phone number and returns if it's true


    // checks if email is a valid adress
    public static bool IsValidEmail(string email)
    {
        try
        {
            MailAddress mailadress = new(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    // retrieves all accounts from accounts.json
    private static List<Account> GetSavedAccounts()
    {
        // Read the JSON file as a string
        string jsonString = File.ReadAllText("accounts.json");

        // Deserialize the JSON string into an object
        return JsonSerializer.Deserialize<List<Account>>(jsonString) ?? new();
    }

    // Saves new account in JSON
    private static void SaveNewAccount(Account account)
    {
        // Retrieves existing accounts
        List<Account> AccountList = GetSavedAccounts();
        // Adds the new account
        AccountList.Add(account);
        //saves accounts
        SaveAccounts(AccountList);
    }

    // Saves Accounts
    private static void SaveAccounts(List<Account> AccountList)
    {
        JsonSerializerOptions options = new() { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(AccountList, options);
        File.WriteAllText("accounts.json", jsonString);
    }

    // Search for an account based on email
    private static Account? GetAccountByEmail(string email)
    {
        List<Account> AccountList = GetSavedAccounts();
        return AccountList?.Find(account => account.Email.ToLower() == email.ToLower());
    }

    public static bool IsValidFirstName(string firstName)
    {
        return firstName.All(char.IsLetter);
    }
    private static void EditProfile()
    {
        Console.Clear();
        if (User == null)
            return;

        string password;
        List<Account> savedAccounts = GetSavedAccounts();
        int accountIndex = savedAccounts.FindIndex(x => x.Email == User.Email);

        string[] userStrings = User.ToString().Split("\n");
        int longestUserString = userStrings.OrderByDescending(x => x.Length).First().Length;
        int leftPadding = (Console.WindowWidth - longestUserString) / 2;
        foreach (string userString in userStrings)
        {
            Console.CursorLeft = leftPadding;
            Console.WriteLine($"{userString}");
        }

        string[] options = new string[] {
            "1.Verander email        ",
            "2.Verander voornaam     ",
            "3.Verander achternaam   ",
            "4.Verander geboortedatum",
            "5.Verander wachtwoord   ",
            "6.Terug                 "
        };

        int userAction = ProgramFunctions.ShowMenuInline(options);
        switch (userAction)
        {
            case 0:
                Console.Clear();
                Console.WriteLine("Voer uw Wachtwoord opnieuw in:");
                password = HashPassword(ReadPassword());
                Console.Clear();
                if (User.TestPassword(password))
                {
                    User.Email = RegisterEmail();
                }
                break;

            case 1:
                Console.Clear();
                Console.WriteLine("Voer uw Wachtwoord opnieuw in:");
                password = HashPassword(ReadPassword());
                Console.Clear();
                if (User.TestPassword(password))
                {
                    Console.WriteLine("Voornaam:");
                    User.FirstName = Console.ReadLine() ?? "";
                }
                break;

            case 2:
                Console.Clear();
                Console.WriteLine("Voer uw Wachtwoord opnieuw in:");
                password = HashPassword(ReadPassword());
                Console.Clear();
                if (User.TestPassword(password))
                {
                    Console.WriteLine("Achternaam:");
                    User.LastName = Console.ReadLine() ?? "";
                }
                break;

            case 3:
                Console.Clear();
                Console.WriteLine("Voer uw Wachtwoord opnieuw in:");
                password = HashPassword(ReadPassword());
                Console.Clear();
                if (User.TestPassword(password))
                {
                    User.BirthDate = RegisterBirthdate();
                }
                break;

            case 4:
                Console.Clear();
                Console.WriteLine("Voer uw Wachtwoord opnieuw in:");
                password = HashPassword(ReadPassword());
                Console.Clear();
                if (User.TestPassword(password))
                {
                    Console.WriteLine("Voer uw nieuwe wachtwoord in:");
                    User.Password = HashPassword(RegisterConfirmPassword(false));
                }
                break;

            case 5:
                Console.Clear();
                return;
        }
        savedAccounts[accountIndex] = User;
        SaveAccounts(savedAccounts);
        Console.Clear();
    }
}
