using System.Net.Mail;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

static class Authentication {
    public static Account? User {get; private set;}

    // Starts the authentication process
    public static Account? Start() {
        while (true) {
            Console.WriteLine("1. Login\n2. Register");
            string userAction = (Console.ReadLine() ?? "").ToLower();
            if (userAction == "1" || userAction == "log in" || userAction == "login")
                try {
                    return Login();
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            else if (userAction == "2" || userAction == "register") {
                try {
                    return Register();
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
                
            } else {
                Console.WriteLine("pain");
            }
            return null;
        }
    }

    // Starts the Login process
    public static Account Login() {
        Console.WriteLine("E-mail:");
        string email = Console.ReadLine() ?? "";
        Console.WriteLine("Password:");
        string password = ReadPassword();

        // Searches for account that has the correct email
        Account? foundAccount = GetAccountByEmail(email) ?? throw new Exception("Invalid credentials");

        // checks the password hash on the found account
        if (!foundAccount.TestPassword(HashPassword(password)))
            throw new Exception("Invalid credentials");

        // sets User property and returns User
        User = foundAccount;
        return foundAccount;
        
    }

    // Starts the registration process
    public static Account Register() {
        // calls email checker
        string email = RegisterEmail();

        // hashes returned string of the confirm password process function
        string password = HashPassword(RegisterConfirmPassword());
        
        Console.WriteLine("First name:");
        string firstName = Console.ReadLine() ?? "";
        Console.WriteLine("Last name:");
        string lastName = Console.ReadLine() ?? "";

        string birthdate = RegisterBirthdate();
        
        
        Console.WriteLine("Phone number:");
        string phoneNumber = RegisterPhoneNumber();
        
        // Creates new object
        Account account = new(email, password, firstName, lastName, birthdate, phoneNumber);
        SaveNewAccount(account);

        // save account to property and return account
        User = account;
        return account;
    }

    // View the information of currently logged in account
    public static void ViewProfile() {
        while (true) {
            if (User != null)
                Console.WriteLine($"{User.ToString()}");

            Console.WriteLine("1. Logout\n2. Go Back");
            string userAction = Console.ReadLine() ?? "";
            if (userAction == "1" || userAction.ToLower() == "logout") {
                Logout();
                break;
            } else if (userAction == "2" || userAction.ToLower() == "go back" || userAction.ToLower() == "back")
                break;
        }
    }

    // resets User property
    public static void Logout() {
        User = null;
    }

    // Reads the password input while not showing the typed characters by visually replacing them with *
    private static string ReadPassword() {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Ignore any key that's not a printable character or Enter
            if (char.IsControl(key.KeyChar) && key.Key != ConsoleKey.Enter)
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
    private static string RegisterEmail() {
        while (true) {
            try {
                Console.WriteLine("Email:");
                string email = Console.ReadLine() ?? "";
                if (IsValidEmail(email) && GetAccountByEmail(email) == null)
                    return email;
                else
                    throw new Exception("Email invalid or already in use");
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

        }
    }

    // asks for password twice and checks if they match
    private static string RegisterConfirmPassword() {
        while (true) {
            try {
                Console.WriteLine("Password:");
                string password = ReadPassword();
                Console.WriteLine("Confirm password:");
                string passwordConfirm = ReadPassword();
                if (password != passwordConfirm)
                    throw new Exception("Passwords don't match");
                else
                    return password;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }

    // returns hash of string
    private static string HashPassword(string password)
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
    private static string RegisterBirthdate() {
        while (true) {
            try {
                Console.WriteLine("birthdate (day-month-year):");
                string birthdate = Console.ReadLine() ?? "";
                DateTime birthdatetime;
                if (!DateTime.TryParseExact(birthdate, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out birthdatetime) && !DateTime.TryParseExact(birthdate, "d-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out birthdatetime) && !DateTime.TryParseExact(birthdate, "d-M-yyyy", null, System.Globalization.DateTimeStyles.None, out birthdatetime) && !DateTime.TryParseExact(birthdate, "dd-M-yyyy", null, System.Globalization.DateTimeStyles.None, out birthdatetime))
                    throw new Exception("Invalid birthday");
                else
                    return birthdate;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
    // Validates phone number and returns if it's true
    private static string RegisterPhoneNumber() {
        string pattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
        while (true) {
            try {
                Console.WriteLine("Phone number:");
                string phoneNumber = Console.ReadLine() ?? "";
                if (!Regex.IsMatch(phoneNumber, pattern))
                    throw new Exception("Invalid phone number");
                else
                    return phoneNumber;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }

    // checks if email is a valid adress
    private static bool IsValidEmail(string email) {
        try {
            MailAddress mailadress = new(email);
            return true;
        }catch (FormatException) {
            return false;
        }
    }
    
    // retrieves all accounts from accounts.json
    private static List<Account> GetSavedAccounts() {
        // Read the JSON file as a string
        string jsonString = File.ReadAllText("accounts.json");

        // Deserialize the JSON string into an object
        return JsonSerializer.Deserialize<List<Account>>(jsonString) ?? new();
    }

    // Saves new account in JSON
    private static void SaveNewAccount(Account account) {
        // Retrieves existing accounts
        List<Account> AccountList = GetSavedAccounts();
        // Adds the new account
        AccountList.Add(account);
        //saves accounts
        SaveAccounts(AccountList);
    }

    // Saves Accounts
    private static void SaveAccounts(List<Account> AccountList) {
        JsonSerializerOptions options = new() {WriteIndented = true};
        string jsonString = JsonSerializer.Serialize(AccountList, options);
        File.WriteAllText("accounts.json", jsonString);
    }

    // Search for an account based on email
    private static Account? GetAccountByEmail(string email) {
        List<Account> AccountList = GetSavedAccounts();
        return AccountList?.Find(account => account.email == email);
    }
}