[TestFixture]
public class EditTest
{
    private SQLiteConnection _Conn;
    UserAccess? u = null;

    [OneTimeSetUp]
    public void Setup()
    {
        File.Delete("./TEST.db");
        SQLiteConnection.CreateFile("./TEST.db");
        string connectionString = "Data Source=./TEST.db;Version=3";
        _Conn = new SQLiteConnection(connectionString);
        Directory.CreateDirectory("./DataSource/");
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
            FirstName: "Testname",
            LastName: "Testlast",
            Email: "Test@mail.com",
            Password: "123",
            Role: UserRole.USER
        );

        var UpdatedUser = new User(
            Id: 1, 
            FirstName: "Upatedname",
            LastName: "Updatedlast",
            Email: "Test@mail.com",
            Password: "123",
            Role: UserRole.ADMIN
        );

        string insertSql = @"
            INSERT INTO Users (FirstName, LastName, Email, Password, Role)
            VALUES (@FirstName, @LastName, @Email, @Password, @Role);
            SELECT last_insert_rowid();
        ";

        long lastInsertedId = 0;

        using (SQLiteCommand command = new SQLiteCommand(insertSql, _Conn))
        {
            command.Parameters.AddWithValue("@FirstName", User.FirstName);
            command.Parameters.AddWithValue("@LastName", User.LastName);
            command.Parameters.AddWithValue("@Email", User.Email);
            command.Parameters.AddWithValue("@Password", User.Password);
            command.Parameters.AddWithValue("@Role", User.Role.ToString());

            _Conn.Open();
            lastInsertedId = (long)command.ExecuteScalar();
        }

        u.UpdateUser(UpdatedUser);

        User retrievedUser = null;
        string selectUserSql = "SELECT * FROM Users WHERE Id = @Id";

        using (SQLiteCommand selectCommand = new SQLiteCommand(selectUserSql, _Conn))
        {
            selectCommand.Parameters.AddWithValue("@Id", UpdatedUser.Id);

            using (SQLiteDataReader reader = selectCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    retrievedUser = new User(
                        Id: Convert.ToInt32(reader["Id"]),
                        FirstName: reader["FirstName"].ToString(),
                        LastName: reader["LastName"].ToString(),
                        Email: reader["Email"].ToString(),
                        Password: reader["Password"].ToString(),
                        Role: (UserRole)Enum.Parse(typeof(UserRole), reader["Role"].ToString())
                    );
                }
            }
        }

        Assert.NotNull(retrievedUser);
        Assert.AreEqual(retrievedUser.FirstName, UpdatedUser.FirstName);
    }
}
