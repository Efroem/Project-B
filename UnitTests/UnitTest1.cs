namespace UnitTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void accountPasswordTest()
    {
        Account account = new("test@test.tester", "password", "test", "test", "5-5-1995");
        // test Unit case to check if unit cases are set up properly
        Assert.IsTrue(account.TestPassword("password"), "account password test not working like expected");
    }

    [TestMethod]
    public void authenticationEmail() {
        Assert.IsTrue(Authentication.IsValidEmail("test@mail.com"), "Email validation doesn't work");
        Assert.IsFalse(Authentication.IsValidEmail("noEmail"), "Email validation doesn't reject random string");
    }

    [TestMethod]
    public void authenticationHash() {
        Assert.AreNotEqual(Authentication.HashPassword("password"), "password", "Password isn't being hashed");
        Assert.AreEqual(Authentication.HashPassword("password"), Authentication.HashPassword("password"), "Password hashing isn't equal");
    }

    [TestMethod]
    public void authenticationFirstName() {
        Assert.IsTrue(Authentication.IsValidFirstName("Name"), "Name is valid");
        Assert.IsFalse(Authentication.IsValidFirstName("4"), "Invalid name isn't registered as invalid");
    }

    [TestMethod]
    public void payment() {
        try {
            Payment.AddSeatPrice(5.99);
        } catch (Exception _) {
            Assert.IsTrue(false);
        }
    }
}