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
            command.Parameters.AddWithValue("@Role", user.Role);

            object result = command.ExecuteScalar();
            if (result != null && int.TryParse(result.ToString(), out userId)){
                user.Id = userId;
            }
        }
        _Conn.Close();
        return userId != -1;
    }
}
