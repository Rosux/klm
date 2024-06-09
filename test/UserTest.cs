

[TestFixture]
public class UserTest
{
    private SQLiteConnection _Conn;
    UserAccess? u = null;

    [SetUp]
    public void Setup(){
        string connectionString = "Data Source=./DataSource/TEST.db;Version=3;";
        _Conn = new SQLiteConnection(connectionString);
        System.IO.Directory.CreateDirectory("./DataSource/");
        u = new UserAccess("./DataSource/TEST.db");
    }

    [TearDown]
    public void Cleanup()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        if (File.Exists("./DataSource/TEST.db"))
        {
            File.Delete("./DataSource/TEST.db");
        }
        if (Directory.Exists("./DataSource"))
        {
            Directory.Delete("./DataSource");
        }
    }

    [Test]
    public void AddTest()
    {
        var User = new User(
            FirstName: "Testname",
            LastName: "Testlast",
            Email: "Test@mail.com",
            Password: "123",
            Role: UserRole.USER
        );
        bool Added = u.AddUser(User);
        Assert.IsTrue(Added, "USER not added");
        User retrievedUser = null;
        _Conn.Open();
        string query = $"SELECT * FROM Users WHERE Id = {User.Id}";
        using (SQLiteCommand command = new SQLiteCommand(query, _Conn))
        {
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
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
        _Conn.Close();
        if (retrievedUser.Id == User.Id && retrievedUser.FirstName == User.FirstName && retrievedUser.LastName == User.LastName && retrievedUser.Email == User.Email && retrievedUser.Password == User.Password && retrievedUser.Role == User.Role)
        {
            Assert.Pass("Inserted User found in the list.");
        }else
        {
            Assert.Fail("Inserted User not found in the list.");
        }
    }

    [Test]
    public void DeleteTest()
    {
        var User = new User(
            FirstName: "Testname",
            LastName: "Testlast",
            Email: "Test@mail.com",
            Password: "123",
            Role: UserRole.USER
        );
        string insertSql = @"
            INSERT INTO Users (FirstName, LastName, Email, Password, Role)
            VALUES (@FirstName, @LastName, @Email, @Password, @Role);
            SELECT last_insert_rowid();
        ";
        _Conn.Open();
        int lastInsertedId = 0;
        using (SQLiteCommand command = new SQLiteCommand(insertSql, _Conn))
        {
            command.Parameters.AddWithValue("@FirstName", User.FirstName);
            command.Parameters.AddWithValue("@LastName", User.LastName);
            command.Parameters.AddWithValue("@Email", User.Email);
            command.Parameters.AddWithValue("@Password", User.Password);
            command.Parameters.AddWithValue("@Role", User.Role.ToString());
            object result = command.ExecuteScalar();
            if (result != null && int.TryParse(result.ToString(), out lastInsertedId)){
                User.Id = lastInsertedId;
            }
        }
        bool Deleted = u.DeleteUser(User);
        Assert.IsTrue(Deleted);
        User retrievedUser = null;
        string query = $"SELECT * FROM Users WHERE Id = {User.Id}";

        using (SQLiteCommand command = new SQLiteCommand(query, _Conn))
        {
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
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
        _Conn.Close();
        if (retrievedUser == null)
        {
            Assert.Pass("Deleted User not found.");
        }else
        {
            Assert.Fail("Deleted User found. So not deleted");
        }
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
            Email: "update@mail.com",
            Password: "1234",
            Role: UserRole.ADMIN
        );
        string insertSql = @"
            INSERT INTO Users (FirstName, LastName, Email, Password, Role)
            VALUES (@FirstName, @LastName, @Email, @Password, @Role);
            SELECT last_insert_rowid();
        ";
        int lastInsertedId = 0;
        using (SQLiteCommand command = new SQLiteCommand(insertSql, _Conn))
        {
            command.Parameters.AddWithValue("@FirstName", User.FirstName);
            command.Parameters.AddWithValue("@LastName", User.LastName);
            command.Parameters.AddWithValue("@Email", User.Email);
            command.Parameters.AddWithValue("@Password", User.Password);
            command.Parameters.AddWithValue("@Role", User.Role.ToString());
            _Conn.Open();
            lastInsertedId = unchecked((int)((long)command.ExecuteScalar()));
        }
        UpdatedUser.Id = lastInsertedId;
        bool answer = u.UpdateUser(UpdatedUser);
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
        _Conn.Close();
        Assert.NotNull(retrievedUser);
        Assert.IsTrue(answer);
        Assert.AreEqual(retrievedUser.FirstName, UpdatedUser.FirstName);
        Assert.AreEqual(retrievedUser.LastName, UpdatedUser.LastName);
        Assert.AreEqual(retrievedUser.Password, UpdatedUser.Password);
        Assert.AreEqual(retrievedUser.Email, UpdatedUser.Email);
        Assert.AreEqual(retrievedUser.Role, UpdatedUser.Role);
    }
}
