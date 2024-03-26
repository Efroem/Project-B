using System.Net.Mail;
using System.Text.Json;

static class Authentication {
    public static Account? User {get; private set;}

    public static Account? Start() {
        while (true) {
            Console.WriteLine("1. Login\n2. Register");
            string userAction = (Console.ReadLine() ?? "").ToLower();
            if (userAction == "1")
                try {
                    return Login();
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            else if (userAction == "2") {
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

        Account? foundAccount = GetAccountByEmail(email);
        if (foundAccount == null)
            throw new Exception("Invalid credentials");
        
        if (!foundAccount.TestPassword(password))
            throw new Exception("Invalid credentials");

        User = foundAccount;
        return foundAccount;
        
    }

    public static Account Register() {
        Console.WriteLine("Email:");
        string email = Console.ReadLine() ?? "";

        string password = RegisterConfirmPassword();
        
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

    private static string RegisterBirthdate() {
        while (true) {
            try {
                Console.WriteLine("birthdate (day-month-year):");
                string birthdate = Console.ReadLine() ?? "";
                DateTime birthdatetime;
                if (!DateTime.TryParseExact(birthdate, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out birthdatetime))
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
        string jsonString = JsonSerializer.Serialize(AccountList);
        File.WriteAllText("accounts.json", jsonString);
    }

    private static Account? GetAccountByEmail(string email) {
        List<Account> AccountList = GetSavedAccounts();
        return AccountList?.Find(account => account.email == email);
    }
}