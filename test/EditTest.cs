[TestFixture]
public class EditTest
{
    UserAccess? u = null;

    [OneTimeSetUp]
    public void Setup(){
        File.Delete("./TEST.db");
        u = new UserAccess("./TEST.db");
    }

    [TearDown]
    public void Cleanup()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    [Test]
    public void EditTesting()
    {
        var User = new User(
            Id: 1,
            FirstName: "Testname",
            LastName: "Testlast",
            Email: "Test@mail.com",
            Password: "123",
            Role: UserRole.USER
        );
        u.AddUser(User);

        var UpdatedUser = new User(
            Id: 1,
            FirstName: "Upatedname",
            LastName: "Updatedlast",
            Email: "Test@mail.com",
            Password: "123",
            Role: UserRole.ADMIN
        );
        u.UpdateUser(UpdatedUser);
        var checkUser = u.CheckUser(UpdatedUser);
        Assert.AreEqual(checkUser.FirstName, UpdatedUser.FirstName);
    }
}