public class UserTable{

    private static UserAccess u = new UserAccess();

    public static void EditUsers()
        {
            List<int> id = new List<int>();
            foreach (User user in u.GetAllUsers()){
                id.Add(user.Id);
            }
            List<string> firstname = new List<string>();
            foreach (User user in u.GetAllUsers()){
                firstname.Add(user.FirstName);
            }
            List<string> lastname = new List<string>();
            foreach (User user in u.GetAllUsers()){
                lastname.Add(user.LastName);
            }
            List<string> email = new List<string>();
            foreach (User user in u.GetAllUsers()){
                email.Add(user.Email);
            }
            List<string> password = new List<string>();
            foreach (User user in u.GetAllUsers()){
                password.Add(user.Password);
            }
            List<List<string>> allLists = new List<List<string>> { firstname, lastname, email, password };
            int CurrentSelected = 0;
            ConsoleKey key;
            do
            {
                int maxLength = 0;
                foreach (var list in allLists)
                {
                    foreach (var str in list)
                    {
                        if (str.Length > maxLength)
                        {
                            maxLength = str.Length;
                        }
                    }
                }

                int idMax = 0;
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
                if (mailMax < 5)
                {
                    mailMax = 5;
                }

                int passMax = 0;
                foreach (var list in password)
                {
                        if (list.Length > passMax)
                        {
                            passMax = list.Length;
                        }
                        
                }
                if (passMax < 8)
                {
                    passMax = 8;
                }


                Console.Clear();
                Console.WriteLine($"first: {firstMax} last: {lastMax} mail: {mailMax} pass: {passMax}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"┌─{new string('─', Math.Max(0, 4))}─┐\n");
                if (CurrentSelected == -1)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                Console.Write($"│");
                Console.Write($"add");
                Console.Write($"   │\n");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"├─{new string('─', Math.Max(0, 4 ))}─┤\n");
                Console.Write($"│─id{new string('─', Math.Max(0, idMax - 2))}─┬─firstname{new string('─', Math.Max(0, firstMax - 9 ))}─┬─LastName{new string('─', Math.Max(0, lastMax - 8  ))}─┬─Email{new string('─', Math.Max(0, mailMax - 5 ))}─┬─Password{new string('─', Math.Max(0, passMax - 8 ))}─┐\n");
                for (int i = 0; i < firstname.Count; i++)
                {
                    Console.Write($"│ ");
                    if (CurrentSelected == i)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    Console.Write($"{id[i]}{new string(' ', Math.Max(0, idMax - id[i].ToString().Length))}");
                    Console.Write($" │ {firstname[i]}{new string(' ', Math.Max(0, firstMax - firstname[i].Length))}");
                    Console.Write($" │ {lastname[i]}{new string(' ', Math.Max(0, lastMax - lastname[i].Length))}");
                    Console.Write($" │ {email[i]}{new string(' ', Math.Max(0, mailMax - email[i].Length))}");
                    Console.Write($" │ {password[i]}{new string(' ', Math.Max(0, passMax - password[i].Length))}");
                    Console.BackgroundColor = ConsoleColor.Black;

                    Console.Write($" │\n");
                }
                Console.Write($"└─{new string('─', Math.Max(0, idMax ))}─┴─{new string('─', Math.Max(0, firstMax ))}─┴─{new string('─', Math.Max(0, lastMax ))}─┴─{new string('─', Math.Max(0, mailMax ))}─┴─{new string('─', Math.Max(0, passMax ))}─┘");
                Console.Write($"\n");
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow)
                {
                    CurrentSelected--;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    CurrentSelected++;
                }
                else if (key == ConsoleKey.Enter)
                {
                    if (CurrentSelected == -1)
                    {
                        UserMenu.AddNewUser();
                        UserMenu.UserAdded();
                    }
                    else
                    {
                        User selecedUser = u.GetUsers(id[CurrentSelected]);
                        User EditUser = UserMenu.EditUser(selecedUser);
                        u.UpdateUser(EditUser);
                    }
                    break;
                }
                CurrentSelected = Math.Clamp(CurrentSelected, -1, firstname.Count - 1);

            } while (key != ConsoleKey.Escape);

        }
}

// ┌ ─ ┬  ┐ 

// │   │  │ 

// ├ ─ ┼  ┤ 

// └ ─ ┴  ┘