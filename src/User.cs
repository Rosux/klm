public class User
{
    public string firstName;
    public string lastName;
    public string email;
    public string password;
    public string Role;

    public User(string firstName, string lastName, string email, string password, string Role)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
        this.password = password;
        this.Role = Role;
    }
}
