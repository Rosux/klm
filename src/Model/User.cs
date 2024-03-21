public class User
{
    public int Id;
    public string FirstName;
    public string LastName;
    public string Email;
    public string Password;
    public UserRole Role = UserRole.USER;

    public User(string FirstName, string LastName, string Email, string Password, UserRole Role=UserRole.USER)
    {
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Email = Email;
        this.Password = Password;
        this.Role = Role;
    }

    public User(int Id, string FirstName, string LastName, string Email, string Password, UserRole Role=UserRole.USER)
    {
        this.Id = Id;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Email = Email;
        this.Password = Password;
        this.Role = Role;
    }
}