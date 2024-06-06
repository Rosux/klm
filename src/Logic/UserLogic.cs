public static class UserLogic
{
    private static UserAccess _userAccess = new UserAccess();
    public static ReservationAccess _reservationAccess = new ReservationAccess();

    /// <summary>
    /// Asks the user to select an option among Edit user and Exit to main menu.
    /// </summary>
    public static void User(){
        MenuHelper.Table<User>(
            _userAccess.GetAllUsers(),
            new Dictionary<string, Func<User, object>>(){
                {"Id", u=>u.Id},
                {"Firstname", u=>u.FirstName},
                {"Lastname", u=>u.LastName},
                {"E-mail", u=>u.Email},
                {"Role", u=>u.Role},
            },
            false,
            true,
            true,
            new Dictionary<string, PropertyEditMapping<User>>(){
                {"First Name", new(u=>u.FirstName, GetValidFirstName)},
                {"Last Name", new(u=>u.LastName, GetValidLastName)},
                {"Email", new(u=>u.Email, GetValidEmail)},
                {"Role", new(u=>u.Role, GetValidRole)},
            },
            SaveEditedUser,
            true,
            CreateNewUser,
            true,
            DeleteUser
        );
    }

    /// <summary>
    /// Deletes a given user and returns a boolean if the deletion was succesfull with a confirmation message.
    /// </summary>
    /// <param name="user">The user instance to delete from the database.</param>
    /// <returns>A boolean indicating if the deletion was successful</returns>
    private static bool DeleteUser(User user){
        bool exists = false;
        bool confirmation = false;
        List<Reservation> allReservations = _reservationAccess.GetAllReservations();
            foreach(Reservation reservation in allReservations)
            {
                if(reservation.UserId == user.Id)
                {
                    exists = true;
                }
            }
        if(exists)
        {
            confirmation = MenuHelper.Confirm($"Are you sure you want to delete the following user:\n\nId: {user.Id}\nFirstname: {user.FirstName}\nLastname: {user.LastName}\nEmail: {user.Email}\nRole: {user.Role}\nThis user has an reservation.", true);
        }else
        {
            confirmation = MenuHelper.Confirm($"Are you sure you want to delete the following user:\n\nId: {user.Id}\nFirstname: {user.FirstName}\nLastname: {user.LastName}\nEmail: {user.Email}\nRole: {user.Role}");
        }
        if(confirmation){
            bool success = _userAccess.DeleteUser(user);
            UserMenu.UserRemoved(success);
            return success;
        }else{
            return false;
        }
    }

    /// <summary>
    /// Creates a new user by asking the current user to fill in the fields and then saves this new user and notifies the user if the saving was successful.
    /// </summary>
    /// <returns>NULL in the case the user doesnt fill in all the fields. Returns a User instance with the new user data if the user filled in everything correctly.</returns>
    private static User? CreateNewUser(){
        User? newUser = UserMenu.GetNewUser();
        if(newUser == null){
            return null;
        }
        bool success = _userAccess.AddUser(newUser);
        if(success){
            UserMenu.UserAdded(success);
            return newUser;
        }else{
            return null;
        }
    }

    /// <summary>
    /// Update an edited user. and notifies the user if the update was successful.
    /// </summary>
    /// <param name="user">An edited user to update.</param>
    /// <returns>A boolean indicating if the update went correctly.</returns>
    private static bool SaveEditedUser(User user){
        bool confirmation = MenuHelper.Confirm($"Are you sure you want to save the following edited data:\n\nId: {user.Id}\nFirstname: {user.FirstName}\nLastname: {user.LastName}\nEmail: {user.Email}\nRole: {user.Role}");
        if(confirmation){
            bool success = _userAccess.UpdateUser(user);
            UserMenu.UserUpdated(success);
            return success;
        }else{
            return false;
        }
    }

    /// <summary>
    /// Asks the user to fill in the first name and returns it.
    /// </summary>
    /// <param name="previousUser">The current user before editing any members.</param>
    /// <returns>A string of the new member value.</returns>
    private static string GetValidFirstName(User previousUser){
        string? value = UserMenu.GetValidInput("Enter the new first name of the user:", 3, 20);
        if(value == null){
            return previousUser.FirstName;
        }else{
            return value;
        }
    }

    /// <summary>
    /// Asks the user to fill in the last name and returns it.
    /// </summary>
    /// <param name="previousUser">The current user before editing any members.</param>
    /// <returns>A string of the new member value.</returns>
    private static string GetValidLastName(User previousUser){
        string? value = UserMenu.GetValidInput("Enter the new last name of the user:", 3, 20);
        if(value == null){
            return previousUser.LastName;
        }else{
            return value;
        }
    }

    /// <summary>
    /// Asks the user to fill in the email and returns it.
    /// </summary>
    /// <param name="previousUser">The current user before editing any members.</param>
    /// <returns>A string of the new member value.</returns>
    private static string GetValidEmail(User previousUser){
        string? value = UserMenu.GetValideEmail("Enter the new email of the user:", 3, 20);
        if(value == null){
            return previousUser.Email;
        }else{
            return value;
        }
    }

    /// <summary>
    /// Asks the user to select a UserRole and returns it.
    /// </summary>
    /// <param name="previousUser">The current user before editing any members.</param>
    /// <returns>A string of the new member value.</returns>
    private static object GetValidRole(User previousUser){
        UserRole? roleSelection = MenuHelper.SelectFromList(
            "Select the new user role",
            true,
            new Dictionary<string, UserRole>(){
                {"User", UserRole.USER},
                {"Admin", UserRole.ADMIN},
            }
        );
        if(roleSelection == UserRole.NONE){
            if(previousUser.Role.ToString() == UserRole.USER.ToString()){
                return UserRole.USER;
            }else if(previousUser.Role.ToString() == UserRole.ADMIN.ToString()){
                return UserRole.ADMIN;
            }
            return previousUser.Role;
        }else{
            return roleSelection;
        }
    }

    /// <summary>
    /// Asks the user for credentials and sets the CurrentUser in case it is correct.
    /// </summary>
    public static void Login()
    {
        User? Credentials = UserMenu.GetLoginCredentials();
        if(Credentials == null){
            Program.CurrentUser = null;
            return;
        }
        User? user = _userAccess.VerifyUser(Credentials.Email);
        if(user != null && BCrypt.Net.BCrypt.EnhancedVerify(Credentials.Password, user.Password)){
            Program.CurrentUser = user;
        }else{
            Program.CurrentUser = null;
            UserMenu.WrongLogin();
        }
    }

    /// <summary>
    /// Asks the user to fill in new user data and creates a new user.
    /// </summary>
    public static void Register()
    {
        User? Credentials = UserMenu.GetNewUser();
        if (Credentials == null)
        {
            return;
        }
        bool added = _userAccess.AddUser(Credentials);
        UserMenu.UserAdded(added);
    }
}