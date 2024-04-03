class Account {
    public string email {get; set;}
    private string _password;
    public string password {get => _password; set => _password = _password == null ? value : _password;}
    public string firstName {get; set;}
    public string lastName {get; set;}
    public string birthDate {get; set;}
    public string phoneNumber {get; set;}

    public Account(string email, string password, string firstName, string lastName, string birthDate, string phoneNumber) {
        this.email = email;
        this.password = password;
        this.firstName = firstName;
        this.lastName = lastName;
        this.birthDate = birthDate;
        this.phoneNumber = phoneNumber;
    }
    
    public bool TestPassword(string password) => _password == password;

    public override string ToString() => $"Email: {email}" + "\n" + $"First name: {firstName}" + "\n" + $"Last name: {lastName}" + 
        "\n" + $"Date of birth: {birthDate}" + "\n" + $"Phone number: {phoneNumber}";
}