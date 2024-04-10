public class UserAccess : DatabaseHandler{
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

    /// <summary>
    /// gets all users from  database. 
    /// </summary>
    /// <returns>returns a list with all the users that are registered</returns>
    public List<User> GetAllUsers(){
        List<User> users = new List<User>();
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Users";
        using (SQLiteCommand Show = new SQLiteCommand(NewQuery, _Conn))
        {
            SQLiteDataReader reader = Show.ExecuteReader();
            while (reader.Read())
            {
                UserRole r;
                Enum.TryParse(reader["Role"].ToString(), out r);
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string lastname = reader.GetString(2);
                string email = reader.GetString(3);
                string password = reader.GetString(4);
                string Role = reader.GetString(5);
                users.Add(new User(id, name, lastname, email, password, r));
            }
        }
        _Conn.Close();
        return users;
    }
    public bool DeleteUser(User user){
        _Conn.Open();
        string NewQuery = @"DELETE FROM Users WHERE ID = @Id ";
        using(SQLiteCommand Remove = new SQLiteCommand(NewQuery, _Conn))
        {
            Remove.Parameters.AddWithValue("@Id", user.Id);
            int rowsAffected = Remove.ExecuteNonQuery();
            _Conn.Close();
            return rowsAffected > 0;
        }
    }
}
