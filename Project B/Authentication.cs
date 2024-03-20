using System.Net.Mail;

static class Authentication {
    private static List<Account> testAccountList = new() {new("e@mail.nl", "test", "John", "Doe", "1-1-2000", "5"),};
    public static Account Login() {
        try {
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
        } catch (Exception e) {
            Console.WriteLine(e.Message);
            return null;
        }
        
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
        
        
        

        return new Account(email, password, firstName, lastName, birthdate, phoneNumber);
        
    }

    private static bool IsValidEmail(string email) {
        try {
            MailAddress mailadress = new(email);
            return true;
        }catch (FormatException) {
            return false;
        }
    }

    private static Account? GetAccountByEmail(string email) => testAccountList.Find(account => account.email == email);
}