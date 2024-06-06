public class UserAccess : DatabaseHandler{
    public UserAccess(string? DatabasePath=null) : base(DatabasePath){}

    /// <summary>
    /// Adds a user to the database if given a user class.
    /// </summary>
    /// <param name="user">Given the user class adds it to the database.</param>
    /// <returns>Returns a true if the user has been added to the database and a false if not.</returns>
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

    /// <summary>
    /// This method checks if the user exists in the database with the email and password.
    /// </summary>
    /// <param name="login">Given a user checks if it exists in the database.</param>
    /// <returns>Returns the user that it has found. if it doesnt find anything returns a null.</returns>
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

    /// <summary>
    /// gets all users from  database.
    /// </summary>
    /// <returns>returns a list with all the users that are registered.</returns>
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

   /// <summary>
   /// Gets everything from the database with the given email parameter.
   /// </summary>
   /// <param name="Email">Here you give an email string and it gets searched for in the database.</param>
   /// <returns>Returns the user if it has been found if not returns null.</returns>
    public User? VerifyUser(string Email){
        User? user = null;
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Users WHERE Email = @Email";
        using (SQLiteCommand Show = new SQLiteCommand(NewQuery, _Conn))
        {
            Show.Parameters.AddWithValue("@Email", Email);
            SQLiteDataReader reader = Show.ExecuteReader();
            if (reader.Read())
            {
                UserRole r;
                Enum.TryParse(reader["Role"].ToString(), out r);
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string lastname = reader.GetString(2);
                string email = reader.GetString(3);
                string password = reader.GetString(4);
                string Role = reader.GetString(5);
                user = new User(id, name, lastname, email, password, r);
                if (reader.Read())
                {
                    user = null;
                }
            }
        }
        _Conn.Close();
        return user;
    }

    /// <summary>
    /// Deltes the user if given a user class parameter.
    /// </summary>
    /// <param name="user">Given a parameter deltes it if it exists in the database.</param>
    /// <returns>Returns true if the user has been found/delted and false if not.</returns>
    public bool DeleteUser(User user){
        _Conn.Open();
        string NewQuery = @"DELETE FROM Users WHERE ID = @Id;DELETE FROM Reservations WHERE UserId = @Id";
        using(SQLiteCommand Remove = new SQLiteCommand(NewQuery, _Conn))
        {
            Remove.Parameters.AddWithValue("@Id", user.Id);
            int rowsAffected = Remove.ExecuteNonQuery();
            _Conn.Close();
            return rowsAffected > 0;
        }
    }

    /// <summary>
    /// Gets a user from the database with the matching id provided.
    /// </summary>
    /// <param name="id">This is the id you provide that your going to search in the database with.</param>
    /// <returns>Returns the user if it has been found if not returns null.</returns>
    public User? GetUser(int id){
        User? user = null;
        _Conn.Open();
        string NewQuery = @"SELECT * FROM Users WHERE ID = @id";
        using (SQLiteCommand Show = new SQLiteCommand(NewQuery, _Conn))
        {
            Show.Parameters.AddWithValue("@id", id);
            SQLiteDataReader reader = Show.ExecuteReader();
            while (reader.Read())
            {
                UserRole r;
                Enum.TryParse(reader["Role"].ToString(), out r);
                int userId = reader.GetInt32(0);
                string name = reader.GetString(1);
                string lastname = reader.GetString(2);
                string email = reader.GetString(3);
                string password = reader.GetString(4);
                user = new User(userId, name, lastname, email, password, r);
            }
        }
        _Conn.Close();
        return user;
    }

    /// <summary>
    /// updates the user in the database with the given user class parameter.
    /// </summary>
    /// <param name="user">Here you give the updated user.</param>
    /// <returns>Returns true if it has been upated, false if not.</returns>
    public bool UpdateUser(User user){
        _Conn.Open();
        int rowsAffected = -1;
        string updateUser = @"UPDATE Users SET FirstName = @Name, LastName= @LastName, Email = @Email, Password = @Password, Role = @Role WHERE ID = @Id";
        using (SQLiteCommand comm = new SQLiteCommand(updateUser, _Conn)){
            comm.Parameters.AddWithValue("@Name", user.FirstName);
            comm.Parameters.AddWithValue("@LastName", user.LastName);
            comm.Parameters.AddWithValue("@Email", user.Email);
            comm.Parameters.AddWithValue("@Password", user.Password);
            comm.Parameters.AddWithValue("@Role", user.Role.ToString());
            comm.Parameters.AddWithValue("@Id", user.Id);
            rowsAffected = comm.ExecuteNonQuery();
        }
        _Conn.Close();
        return rowsAffected > 0;
    }
}
