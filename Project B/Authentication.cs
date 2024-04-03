using System.Net.Mail;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

static class Authentication {
    public static Account? User {get; private set;}

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
    public static Account Login() {
        Console.WriteLine("E-mail:");
        string email = Console.ReadLine() ?? "";
        Console.WriteLine("Password:");
        string password = Console.ReadLine() ?? "";

        Account? foundAccount = GetAccountByEmail(email) ?? throw new Exception("Invalid credentials");
        if (!foundAccount.TestPassword(HashPassword(password)))
            throw new Exception("Invalid credentials");

        User = foundAccount;
        return foundAccount;
        
    }

    public static Account Register() {
        string email = RegisterEmail();

        string password = HashPassword(RegisterConfirmPassword());
        
        Console.WriteLine("First name:");
        string firstName = Console.ReadLine() ?? "";
        Console.WriteLine("Last name:");
        string lastName = Console.ReadLine() ?? "";

        string birthdate = RegisterBirthdate();
        
        
        Console.WriteLine("Phone number:");
        string phoneNumber = Console.ReadLine() ?? "";
        
        Account account = new(email, password, firstName, lastName, birthdate, phoneNumber);
        List<Account> AccountList = GetSavedAccounts();
        AccountList.Add(account);
        SaveAccounts(AccountList);
        User = account;
        return account;
    }

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

    public static void Logout() {
        User = null;
    }

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

    private static string RegisterConfirmPassword() {
        while (true) {
            try {
                Console.WriteLine("Password:");
                string password = Console.ReadLine() ?? "";
                Console.WriteLine("Confirm password:");
                string passwordConfirm = Console.ReadLine() ?? "";
                if (password != passwordConfirm)
                    throw new Exception("Passwords don't match");
                else
                    return password;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }

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

    private static bool IsValidEmail(string email) {
        try {
            MailAddress mailadress = new(email);
            return true;
        }catch (FormatException) {
            return false;
        }
    }
    
    private static List<Account> GetSavedAccounts() {
        // Read the JSON file as a string
        string jsonString = File.ReadAllText("accounts.json");

        // Deserialize the JSON string into an object
        return JsonSerializer.Deserialize<List<Account>>(jsonString) ?? new();
    }

    private static void SaveAccounts(List<Account> AccountList) {
        JsonSerializerOptions options = new() {WriteIndented = true};
        string jsonString = JsonSerializer.Serialize(AccountList, options);
        File.WriteAllText("accounts.json", jsonString);
    }

    private static Account? GetAccountByEmail(string email) {
        List<Account> AccountList = GetSavedAccounts();
        return AccountList?.Find(account => account.email == email);
    }
}