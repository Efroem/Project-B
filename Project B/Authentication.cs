using System.Net.Mail;
using System.Text.Json;

static class Authentication {
    public static Account? Start() {
        Console.WriteLine("1. Login\n2. Register");
        string userAction = (Console.ReadLine() ?? "").ToLower();
        if (userAction == "1")
            try {
                return Login();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        else if (userAction == "2") {
            return Register();
        } else {
            Console.WriteLine("pain");
        }
        return null;
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

        return foundAccount;
        
    }

    public static Account Register() {
        Console.WriteLine("Email:");
        string email = Console.ReadLine() ?? "";
        Console.WriteLine("Password:");
        string password = Console.ReadLine() ?? "";
        Console.WriteLine("Confirm password:");
        string passwordConfirm = Console.ReadLine() ?? "";
        Console.WriteLine("First name:");
        string firstName = Console.ReadLine() ?? "";
        Console.WriteLine("Last name:");
        string lastName = Console.ReadLine() ?? "";

        Console.WriteLine("birthdate:");
        string birthdate = Console.ReadLine() ?? "";
        
        Console.WriteLine("Phone number:");
        string phoneNumber = Console.ReadLine() ?? "";
        
        Account account = new(email, password, firstName, lastName, birthdate, phoneNumber);
        List<Account> AccountList = GetSavedAccounts();
        AccountList.Add(account);

        return account;
        
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

    private static Account? GetAccountByEmail(string email) {
        List<Account> AccountList = GetSavedAccounts();
        return AccountList?.Find(account => account.email == email);
    }
}