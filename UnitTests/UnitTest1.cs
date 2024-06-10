namespace UnitTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void account()
    {
        Account account = new("test@test.tester", "password", "test", "test", "5-5-1995");
        // test Unit case to check if unit cases are set up properly
        Assert.IsTrue(account.TestPassword("password"), "account password test not working like expected");
        Assert.IsFalse(account.IsAdmin, "Account is admin when it shoudn't be");
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
    public void paymentSeatPrice() {
        try {
            Payment.AddSeatPrice(5.99);
        } catch (Exception) {
            Assert.IsTrue(false);
        }
    }

    [TestMethod]
    public void cinemaHall() {
        var halls = AdminFunctions.ReadFromCinemaHall();
        Assert.IsInstanceOfType(halls, typeof(List<AdminFunctions>), "cinemaHall read doesn't work");
        Assert.IsTrue(halls.Count > 0, "hall count is zero");
    }

    [TestMethod]
    public void scheduleCreation() {
        var schedules = Schedule.ReadScheduleJson();
        Assert.IsInstanceOfType(schedules, typeof(List<Schedule>), "schedule read doesn't work");
        Assert.IsTrue(schedules.Count > 0, "schedule count is zero");
    
        try {
            Schedule schedule = new("test", "2", "12/12/2019 19:21:00");
        } catch (Exception) {
            Assert.IsTrue(false, "schedule creation failed");
        }
        Schedule scheduleObj = new("test", "2", "12/12/2019 19:21:00");
        Assert.IsInstanceOfType(scheduleObj.Seats[0], typeof(Seat), "Schedule seat generation failed");

        Assert.AreEqual(scheduleObj.Seats[1].ID, "1-2", "Schedule seat generation id failed");
        Assert.IsTrue(scheduleObj.Seats[0].IsAvailable, "Schedule seat generation availability failed");
        
    }
}