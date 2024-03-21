class Account {
    public string email {get; set;}
    public string password {get; set;}
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
    
    public bool TestPassword(string password) => this.password == password;
}