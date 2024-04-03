class Account {
    public string Email {get; set;}
    private string _password;
    public string Password {get => _password; set => _password = _password == null ? value : _password;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string BirthDate {get; set;}
    public string PhoneNumber {get; set;}

    public Account(string email, string password, string firstName, string lastName, string birthDate, string phoneNumber) {
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        PhoneNumber = phoneNumber;
    }
    
    public bool TestPassword(string password) => _password == password;

    public override string ToString() => $"Email: {Email}" + "\n" + $"First name: {FirstName}" + "\n" + $"Last name: {LastName}" + 
        "\n" + $"Date of birth: {BirthDate}" + "\n" + $"Phone number: {PhoneNumber}";
}