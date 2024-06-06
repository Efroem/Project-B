class Account
{
    public string Email { get; set; }
    private string? _password;
    public string? Password { get => _password; set => _password = _password == null ? value : _password; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BirthDate { get; set; }
    // public UserRole Role { get; set; }
    public bool IsAdmin { get; set; } = false;

    public Account(string email, string password, string firstName, string lastName, string birthDate, bool isAdmin = false)
    {
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        IsAdmin = isAdmin;
    }

    public bool TestPassword(string password) => _password == password;

    public override string ToString() => $"Email: {Email}" + "\n" + $"Voornaam: {FirstName}" + "\n" + $"Achternaam: {LastName}" +
        "\n" + $"Geboortedatum: {BirthDate}" + "\n";
}