class Account {
    public string email;
    private string password;
    public string firstName;
    public string lastName;
    public string birthdate;
    public string phoneNumber;

    public Account(string email, string password, string firstName, string lastName, string birthdate, string phoneNumber) {
        this.email = email;
        this.password = password;
        this.firstName = firstName;
        this.lastName = lastName;
        this.birthdate = birthdate;
        this.phoneNumber = phoneNumber;
    }
    
    public bool TestPassword(string password) => this.password == password;
}