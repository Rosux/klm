public class UserAccess : DatabaseHandler{
    public UserAccess(string DatabasePath="./DataSource/CINEMA.db") : base(DatabasePath){

    }

    public bool AddUser(User user){
        _Conn.Open();
        string insertSql = @"
            INSERT INTO Users (FirstName, LastName, Email, Password, Role)
            VALUES (@FirstName, @LastName, @Email, @Password, @Role);
            SELECT last_insert_rowid();
        ";
        int userId = -1;
        using (SQLiteCommand command = new SQLiteCommand(insertSql, _Conn))
        {
            command.Parameters.AddWithValue("@FirstName", user.FirstName);
            command.Parameters.AddWithValue("@LastName", user.LastName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Role", user.Role.ToString());

            object result = command.ExecuteScalar();
            if (result != null && int.TryParse(result.ToString(), out userId)){
                user.Id = userId;
            }
        }
        _Conn.Close();
        return userId != -1;
    }

    public User CheckUser(User login)
    {
        _Conn.Open();
        string selectSql = @"
            SELECT *
            FROM Users
            WHERE Email = @Email AND Password = @Password
        ";

        User currentUser = null;

        using (SQLiteCommand command = new SQLiteCommand(selectSql, _Conn))
        {
            command.Parameters.AddWithValue("@Email", login.Email);
            command.Parameters.AddWithValue("@Password", login.Password);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    UserRole r;
                    Enum.TryParse(reader["Role"].ToString(), out r);
                    currentUser = new User(
                        Int32.Parse(reader["Id"].ToString()),
                        reader["firstName"].ToString(),
                        reader["lastName"].ToString(),
                        reader["email"].ToString(),
                        reader["password"].ToString(),
                        r
                    );
                }
            }
        }

        _Conn.Close();

        return currentUser;
    }
}
