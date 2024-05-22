public class UserTable{

    private static UserAccess u = new UserAccess();
    /// <summary>
    /// Display a table with all users. Where u can edit the users by pressing enter and then selected what to edit. Like Firstname lastname Email or Role.
    ///    ┌──────────────────────────────────────────────────────────────┐
    ///    │ <-                           1/1                          -> │
    ///    │───────────┬──────────────┬──────────┬────────────────┬───────┤
    ///    │ Id        │ Firstname    │ Lastname │ Email          │ Role  │
    ///    ├───────────┼──────────────┼──────────┼────────────────┼───────┤
    ///    │ 1         │ fff          │ email    │ sdfsd          │ USER  │
    ///    │ 2         │ bbbsdfgsdgsd │ sdf      │ sdfdsf         │ USER  │
    ///    │ 3         │ sdf          │ sdf      │ sdf@sdf        │ USER  │
    ///    │ 4         │ gsdgsd       │ Fra      │ sdfsdfs@sdf    │ ADMIN │
    ///    │ 5         │ sdfsf        │ dddddddd │ d@d.d          │ USER  │
    ///    │ 6         │ admin        │ admin    │ admin@mail.com │ ADMIN │
    ///    │ 58        │ muhammed     │ aktas    │ aktas@mail.com │ ADMIN │
    ///    └───────────┴──────────────┴──────────┴────────────────┴───────┘
    /// 
    /// </summary>
    public static void EditUsers()
        {
            int CurrentSelected = 0;
            ConsoleKey key;
            int currentPage = 0;
            bool g = false;
            int currentEditSelected = 1;
            string? first = "";
            string? last  = "";
            string? mail = "";
            int j = 0;
            User? editedUser = null;
            do
            {
                int maxedited = 0;
                int maxorg = 0;
                List<int> id = new List<int>();
                List<string> firstname = new List<string>();
                List<string> lastname = new List<string>();
                List<string> email = new List<string>();
                List<string> role = new List<string>();
                List<User> users = u.GetAllUsers();
                List<string> maxEdited = new List<string>();
                List<string> maxOrg = new List<string>();
                foreach (User user in u.GetAllUsers()){
                    id.Add(user.Id);
                    firstname.Add(user.FirstName);
                    lastname.Add(user.LastName);
                    email.Add(user.Email);
                    role.Add(user.Role.ToString());
                }
                List<List<User>> chunks = new List<List<User>>();
                for(int i = 0;i < users.Count;i+=10)
                {
                    chunks.Add(users.Skip(i).Take(10).ToList());
                }
                int maxPage = chunks.Count;
                int maxLength = 0;
                int idMax = 0;
                if (editedUser != null)
                {
                    maxEdited.Add(editedUser.FirstName);
                    maxEdited.Add(editedUser.LastName);
                    maxEdited.Add(editedUser.Email);
                    maxEdited.Add(editedUser.Role.ToString());
                    maxOrg.Add(chunks[currentPage][CurrentSelected].FirstName);
                    maxOrg.Add(chunks[currentPage][CurrentSelected].LastName);
                    maxOrg.Add(chunks[currentPage][CurrentSelected].Email);
                    maxOrg.Add(chunks[currentPage][CurrentSelected].Role.ToString());
                    foreach(var list in maxEdited)
                    {
                        if (list.ToString().Length > maxedited )
                        {
                            maxedited = list.ToString().Length;
                        }
                    }
                    foreach(var list in maxOrg)
                    {
                        if (list.ToString().Length > maxorg )
                        {
                            maxorg = list.ToString().Length;
                        }
                    }
                }
                maxedited += maxorg;
                foreach (var list in id)
                {
                    if (list.ToString().Length > idMax )
                    {
                        idMax = list.ToString().Length;
                    }
                }
                if (idMax < 9)
                {
                    idMax = 9;
                }
                int firstMax = 0;
                foreach (var list in firstname)
                {
                    if (list.Length > firstMax )
                    {
                        firstMax = list.Length;
                    }
                }
                if (firstMax < 9)
                {
                    firstMax = 9;
                }
                int lastMax = 0;
                foreach (var list in lastname)
                {
                    if (list.Length > lastMax)
                    {
                        lastMax = list.Length;
                    }
                }
                if (lastMax < 8)
                {
                    lastMax = 8;
                }
                int mailMax = 0;
                foreach (var list in email )
                {
                    if (list.Length > mailMax)
                    {
                        mailMax = list.Length;
                    }
                }
                if (mailMax < 4)
                {
                    mailMax = 4;
                }
                int roleMax = 0;
                foreach (var list in role)
                {
                    if (list.Length > roleMax)
                    {
                        roleMax = list.Length;
                    }
                }
                if (roleMax < 5)
                {
                    roleMax = 5;
                }
                maxLength += firstMax + lastMax + mailMax + roleMax + 17;
                Console.Clear();
                Console.WriteLine("Press escape to go back");
                if (editedUser != null)
                {
                    Console.WriteLine($"max:{maxedited}  firstlenght: {editedUser.Email.Length}");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"┌─{new string('─', Math.Max(0, maxLength + 4))}─┐\n");
                if (CurrentSelected == -1)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                string x = $"{currentPage + 1}/{maxPage}";
                string time = $"{x}";
                if(maxPage != 1)
                {
                    for(int i = 1; i <= Math.Max(2, maxLength - x.Length); i++)
                    {
                        time = ((i % 2 == 1) ? " " : "") + time + ((i % 2 == 0) ? " " : "");
                    }
                    if (currentPage  < chunks.Count - 1)
                    {
                        time = "  " + time;
                    }else
                    {
                        time = "<-" + time;
                    }
                    if (currentPage > 0 )
                    {
                        time = time + "  " ;
                    }else
                    {
                        time = time + "->" ;
                    }
                    if (currentPage > 0  && currentPage  < chunks.Count - 1)
                    {
                        if(maxLength%2 !=0)
                        {
                            j = 2;
                        }
                        else
                        {
                            j = 1;
                        }
                        time = "<-" +$"{new string(' ', Math.Max(0, maxLength/2-x.Length+2))}"+$"{x}"+$"{new string(' ', Math.Max(0, maxLength/2-x.Length+j))}"+"->" ;
                    } 
                }else
                {
                    if(maxLength%2 !=0)
                        {
                            j = 4;
                        }
                        else
                        {
                            j = 3;
                        }
                    time =$"{new string(' ', Math.Max(0, maxLength/2-x.Length+4))}"+$"{x}"+$"{new string(' ', Math.Max(0, maxLength/2-x.Length+j))}" ;
                }
                Console.Write($"│ {time} │\n");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"│─{new string('─', Math.Max(0, idMax))}─┬─{new string('─', Math.Max(0, firstMax))}─┬─{new string('─', Math.Max(0, lastMax))}─┬─{new string('─', Math.Max(0, mailMax))}─┬─{new string('─', Math.Max(0, roleMax))}─┤\n");
                Console.Write($"│ Id{new string(' ', Math.Max(0, idMax - 1))}│ Firstname{new string(' ', Math.Max(0, firstMax - 8))}│ Lastname{new string(' ', Math.Max(0, lastMax - 7))}│ Email{new string(' ', Math.Max(0, mailMax - 4))}│ Role{new string(' ', Math.Max(0, roleMax - 3))}│\n");
                Console.Write($"├─{new string('─', Math.Max(0, idMax))}─┼─{new string('─', Math.Max(0, firstMax))}─┼─{new string('─', Math.Max(0, lastMax))}─┼─{new string('─', Math.Max(0, mailMax))}─┼─{new string('─', Math.Max(0, roleMax))}─┤\n");
                for (int i = 0; i < Math.Max(chunks[currentPage].Count, 9); i++)
                {
                    if(i < chunks[currentPage].Count)
                    {
                        Console.Write($"│ ");
                        if (CurrentSelected == i && !g)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                        Console.Write($"{chunks[currentPage][i].Id}{new string(' ', Math.Max(0, idMax - chunks[currentPage][i].Id.ToString().Length))}");
                        Console.Write($" │ {chunks[currentPage][i].FirstName}{new string(' ', Math.Max(0, firstMax - chunks[currentPage][i].FirstName.Length))}");
                        Console.Write($" │ {chunks[currentPage][i].LastName}{new string(' ', Math.Max(0, lastMax - chunks[currentPage][i].LastName.Length))}");
                        Console.Write($" │ {chunks[currentPage][i].Email}{new string(' ', Math.Max(0, mailMax - chunks[currentPage][i].Email.Length))}");
                        Console.Write($" │ {chunks[currentPage][i].Role}{new string(' ', Math.Max(0, roleMax - chunks[currentPage][i].Role.ToString().Length))}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($" │    ");
                        if( i == 0 && g == true)
                        {
                            Console.Write($"┌─editUser{new String('─', Math.Max(0, maxedited + 7))}─┐");
                        }else if(i == 1 && g == true)
                        {
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Firstname: {chunks[currentPage][CurrentSelected].FirstName} -> {editedUser.FirstName}{new String(' ', Math.Max(0, maxedited - editedUser.FirstName.Length - chunks[currentPage][CurrentSelected].FirstName.Length))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }else if(i == 2 && g == true)
                        {
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Lastname : {chunks[currentPage][CurrentSelected].LastName} -> {editedUser.LastName}{new String(' ', Math.Max(0, maxedited - editedUser.LastName.Length - chunks[currentPage][CurrentSelected].LastName.Length ))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }else if (i == 3 && g == true)
                        {
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Email    : {chunks[currentPage][CurrentSelected].Email} -> {editedUser.Email}{new String(' ', Math.Max(0, maxedited - editedUser.Email.Length - chunks[currentPage][CurrentSelected].Email.Length ))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }else if (i == 4 && g == true)
                        {
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Role     : {chunks[currentPage][CurrentSelected].Role} -> {editedUser.Role}{new String(' ', Math.Max(0, maxedited - editedUser.Role.ToString().Length - chunks[currentPage][CurrentSelected].Role.ToString().Length))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }else if (i == 5 && g == true)
                        {
                            Console.Write($"│─{new String('─', Math.Max(0, maxedited + 15))}─│");
                        }else if (i == 6 && g == true)
                        {
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Save changes{new String(' ', Math.Max(0, maxedited + 3))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }else if (i == 7 && g == true)
                        {
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Discard changes{new String(' ', Math.Max(0, maxedited ))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }else if (i == 8 && g == true)
                        {
                            Console.Write($"└─{new String('─', Math.Max(0, maxedited + 15))}─┘");
                        }
                        
                            Console.Write($"\n");    
                        
                    }else{
                        if(i == chunks[currentPage].Count)
                        {
                            Console.Write($"└─{new string('─', Math.Max(0, idMax ))}─┴─{new string('─', Math.Max(0, firstMax ))}─┴─{new string('─', Math.Max(0, lastMax ))}─┴─{new string('─', Math.Max(0, mailMax ))}─┴─{new string('─', Math.Max(0, roleMax ))}─┘");
                        }
                        if( i == 1 && g == true && chunks[currentPage].Count <= 1 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, 4))}");
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Firstname: {chunks[currentPage][CurrentSelected].FirstName} -> {editedUser.FirstName}{new String(' ', Math.Max(0, maxedited - editedUser.FirstName.Length - chunks[currentPage][CurrentSelected].FirstName.Length))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }
                        if(i == 2 && chunks[currentPage].Count <= 1 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, maxLength + 12))}");
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Lastname : {chunks[currentPage][CurrentSelected].LastName} -> {editedUser.LastName}{new String(' ', Math.Max(0, maxedited - editedUser.LastName.Length - chunks[currentPage][CurrentSelected].LastName.Length ))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }else if(i == 2 && chunks[currentPage].Count > 1 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, 4))}");
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Lastname : {chunks[currentPage][CurrentSelected].LastName} -> {editedUser.LastName}{new String(' ', Math.Max(0, maxedited - editedUser.LastName.Length - chunks[currentPage][CurrentSelected].LastName.Length ))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }
                        if(i == 3 && chunks[currentPage].Count >= 3  && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, 4))}");
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Email    : {chunks[currentPage][CurrentSelected].Email} -> {editedUser.Email}{new String(' ', Math.Max(0, maxedited - editedUser.Email.Length - chunks[currentPage][CurrentSelected].Email.Length ))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }else if(i == 3 && chunks[currentPage].Count <= 2 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, maxLength + 12))}");
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Email    : {chunks[currentPage][CurrentSelected].Email} -> {editedUser.Email}{new String(' ', Math.Max(0, maxedited - editedUser.Email.Length - chunks[currentPage][CurrentSelected].Email.Length ))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }
                        if(i == 4 && chunks[currentPage].Count <= 3 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0,  maxLength + 12))}");
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Role     : {chunks[currentPage][CurrentSelected].Role} -> {editedUser.Role}{new String(' ', Math.Max(0, maxedited - editedUser.Role.ToString().Length - chunks[currentPage][CurrentSelected].Role.ToString().Length))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }else if(i == 4 && chunks[currentPage].Count >= 4 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, 4))}");
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Role     : {chunks[currentPage][CurrentSelected].Role} -> {editedUser.Role}{new String(' ', Math.Max(0, maxedited - editedUser.Role.ToString().Length - chunks[currentPage][CurrentSelected].Role.ToString().Length))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }
                        if(i == 5 && chunks[currentPage].Count <= 4 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, maxLength + 12))}");
                            Console.Write($"│─{new String('─', Math.Max(0, maxedited + 15))}─│");
                        }else if(i == 5 && chunks[currentPage].Count >= 4 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0,  4))}");
                            Console.Write($"│─{new String('─', Math.Max(0, maxedited + 15))}─│");
                        }
                        if(i == 6 && chunks[currentPage].Count < 6 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, maxLength + 12 ))}");
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Save changes{new String(' ', Math.Max(0, maxedited + 3))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }else if(i == 6 && chunks[currentPage].Count >= 6 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, 4))}");
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Save changes{new String(' ', Math.Max(0, maxedited + 3))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }
                        if(i == 7 && chunks[currentPage].Count <= 6 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, maxLength + 12 ))}");
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Discard changes{new String(' ', Math.Max(0, maxedited  ))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }else if(i == 7 && chunks[currentPage].Count >= 7 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, 4))}");
                            Console.Write("│ ");
                            if (currentEditSelected == i && g)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            Console.Write($"Discard changes{new String(' ', Math.Max(0, maxedited ))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" │");
                        }
                        if(i == 8 && chunks[currentPage].Count >= 8 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, 4))}");
                            Console.Write($"└─{new String('─', Math.Max(0, maxedited + 15))}─┘");
                        }else if(i == 8 && chunks[currentPage].Count <= 7 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, maxLength + 12))}");
                            Console.Write($"└─{new String('─', Math.Max(0, maxedited + 15))}─┘");
                        }
                        Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write($"\n");   

                    }
                }
                if(chunks[currentPage].Count >= 9)
                {
                    Console.Write($"└─{new string('─', Math.Max(0, idMax ))}─┴─{new string('─', Math.Max(0, firstMax ))}─┴─{new string('─', Math.Max(0, lastMax ))}─┴─{new string('─', Math.Max(0, mailMax ))}─┴─{new string('─', Math.Max(0, roleMax ))}─┘");
                }
                Console.Write($"\n");
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow && !g)
                {
                    CurrentSelected--;
                }
                else if (key == ConsoleKey.DownArrow && !g)
                {
                    CurrentSelected++;
                }
                else if (key == ConsoleKey.RightArrow && !g)
                {
                    currentPage += 1;
                }
                else if (key == ConsoleKey.LeftArrow && !g)
                {
                    currentPage -= 1;
                }
                else if (key == ConsoleKey.Enter && !g)
                {
                    editedUser = chunks[currentPage][CurrentSelected];
                    g = true;
                }
                else if (key == ConsoleKey.Escape && g)
                {
                    g = false;
                }
                else if (key == ConsoleKey.UpArrow  && g)
                {
                    if (currentEditSelected == 6)
                    {
                        currentEditSelected  -= 2;
                    }else
                    {
                        currentEditSelected  -= 1;
                    }
                }
                else if (key == ConsoleKey.DownArrow  && g)
                {
                    if (currentEditSelected == 4)
                    {
                        currentEditSelected  += 2;
                    }else
                    {
                        currentEditSelected  += 1;
                    }
                }else if (key == ConsoleKey.Enter && g)
                {
                    if (currentEditSelected == 1)
                    {
                        Console.Clear();
                        first = UserMenu.GetValidInput("Enter your Firstname:", 3, 20);
                        if (first != null)
                        {
                            editedUser.FirstName = first;
                        }
                    }else if(currentEditSelected == 2)
                    {
                        Console.Clear();
                        last = UserMenu.GetValidInput("Enter your Lastname:", 3, 20);
                        if (last != null)
                        {
                            editedUser.LastName = last;
                        }
                    }else if(currentEditSelected == 3)
                    {
                        Console.Clear();
                        mail = UserMenu.GetValideEmail("Enter your email:", 3, 20);
                        if (mail != null)
                        {
                            editedUser.Email = mail;
                        }
                    }else if(currentEditSelected == 4)
                    {
                        Console.Clear();
                        MenuHelper.SelectOptions("Choose what to change the Role to", new Dictionary<string, Action>(){
                            {"1. User", ()=>{
                                editedUser.Role = UserRole.USER;
                            }},
                            {"2. Admin", ()=>{
                                editedUser.Role = UserRole.ADMIN;
                            }},
                        });
                    }else if(currentEditSelected == 6)
                    {
                        Console.Clear();
                        bool answers = u.UpdateUser(editedUser);
                        if (answers)
                        {
                            Console.WriteLine($"Updated. click any button to continue");
                            Console.ReadLine();
                            g = false;
                        }else
                        {
                            Console.WriteLine("nope");
                            Console.ReadLine();
                            g = false;
                        }
                    }else if(currentEditSelected == 7)
                    {
                        Console.WriteLine("Discard");
                        g = false;
                    }
                }
                currentPage = Math.Clamp(currentPage, 0, chunks.Count - 1);
                CurrentSelected = Math.Clamp(CurrentSelected, - 0, chunks[currentPage].Count - 1);
                currentEditSelected = Math.Clamp(currentEditSelected, + 1, 8 - 1);
            } while (key != ConsoleKey.Escape);
        }
        
}

// ┌ ─ ┬  ┐ 

// │   │  │ 

// ├ ─ ┼  ┤ 

// └ ─ ┴  ┘