public class UserTable{

    private static UserAccess u = new UserAccess();

    public static void EditUsers()
        {
            int CurrentSelected = 0;
            ConsoleKey key;
            int currentPage = 0;
            bool g = false;
            int currentEditSelected = 0;
            do
            {
                List<int> id = new List<int>();
                List<string> firstname = new List<string>();
                List<string> lastname = new List<string>();
                List<string> email = new List<string>();
                List<string> password = new List<string>();
                List<User> users = u.GetAllUsers();
                foreach (User user in u.GetAllUsers()){
                    id.Add(user.Id);
                    firstname.Add(user.FirstName);
                    lastname.Add(user.LastName);
                    email.Add(user.Email);
                    password.Add(user.Password);
                }
                List<List<User>> chunks = new List<List<User>>();
                for(int i = 0;i < users.Count;i+=10)
                {
                    chunks.Add(users.Skip(i).Take(10).ToList());
                }
                int maxPage = chunks.Count;
                int maxLength = 0;

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
                
                maxLength += firstMax + lastMax + mailMax + 14;
                Console.Clear();
                Console.WriteLine($"first: {firstMax} last: {lastMax} mail: {mailMax}, max: {maxLength} chunks :{chunks[currentPage].Count}, {g}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"┌─{new string('─', Math.Max(0, maxLength + 4))}─┐\n");
                if (CurrentSelected == -1)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                
                string x = $"{currentPage + 1}/{maxPage}";
                string time = $"{x}";
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
                
                Console.Write($"│ {time} │\n");
            
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"│─{new string('─', Math.Max(0, idMax))}─┬─{new string('─', Math.Max(0, firstMax))}─┬─{new string('─', Math.Max(0, lastMax))}─┬─{new string('─', Math.Max(0, mailMax))}─┤    ");
                if(true && g == true){
                    Console.Write($"┌─editUser{new String('─', Math.Max(0, 12))}─┐\n");
                }else{
                    Console.Write("\n");
                }
                Console.Write($"│ id{new string(' ', Math.Max(0, idMax - 1))}│ firstname{new string(' ', Math.Max(0, firstMax - 8))}│ lastname{new string(' ', Math.Max(0, lastMax - 7))}│ email{new string(' ', Math.Max(0, mailMax - 4))}│    ");
                if(true && g == true){
                    Console.Write($"│   firstname{new String(' ', Math.Max(0, 10))}│\n");
                }else{
                    Console.Write("\n");
                }
                Console.Write($"├─{new string('─', Math.Max(0, idMax))}─┼─{new string('─', Math.Max(0, firstMax))}─┼─{new string('─', Math.Max(0, lastMax))}─┼─{new string('─', Math.Max(0, mailMax))}─┤    ");
                if(true && g == true){
                    Console.Write($"│   lastname{new String(' ', Math.Max(0, 11))}│\n");
                }else{
                    Console.Write("\n");
                }
                for (int i = 0; i < Math.Max(chunks[currentPage].Count, 8); i++)
                {
                    if (CurrentSelected == i && !g)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    if(i < chunks[currentPage].Count)
                    {
                        Console.Write($"│ ");
                        Console.Write($"{chunks[currentPage][i].Id}{new string(' ', Math.Max(0, idMax - chunks[currentPage][i].Id.ToString().Length))}");
                        Console.Write($" │ {chunks[currentPage][i].FirstName}{new string(' ', Math.Max(0, firstMax - chunks[currentPage][i].FirstName.Length))}");
                        Console.Write($" │ {chunks[currentPage][i].LastName}{new string(' ', Math.Max(0, lastMax - chunks[currentPage][i].LastName.Length))}");
                        Console.Write($" │ {chunks[currentPage][i].Email}{new string(' ', Math.Max(0, mailMax - chunks[currentPage][i].Email.Length))}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($" │    ");
                        if (currentEditSelected == i && g)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                        if( i == 0 && g == true)
                        {
                            Console.Write($"│   email{new String(' ', Math.Max(0, 14))}│");
                        }else if(i == 1 && g == true)
                        {
                            Console.Write($"│   Role{new String(' ', Math.Max(0, 15))}│");
                        }
                        else if(i == 2 && g == true)
                        {
                            Console.Write($"└─{new String('─', Math.Max(0, 20))}─┘");
                        }
                            Console.Write($"\n");    
                        
                    }else{
                        if(i == chunks[currentPage].Count)
                        {
                            Console.Write($"└─{new string('─', Math.Max(0, idMax ))}─┴─{new string('─', Math.Max(0, firstMax ))}─┴─{new string('─', Math.Max(0, lastMax ))}─┴─{new string('─', Math.Max(0, mailMax ))}─┘");
                        }
                        
                        if( i == 0 && g == true)
                        {
                            Console.Write($"│   email{new String(' ', Math.Max(0, 14))}│");
                        }
                        if(i == 1 && chunks[currentPage].Count <= 1 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, 4))}");
                            Console.Write($"│   Role{new String(' ', Math.Max(0, 15))}│");
                        }else if(i == 1 && chunks[currentPage].Count > 1 && g == true)
                        {
                            Console.Write($"│   Role{new String(' ', Math.Max(0, 15))}│");
                        }
                        if(i == 2 && chunks[currentPage].Count <= 1 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, 83))}");
                            Console.Write($"└─{new String('─', Math.Max(0, 20))}─┘");
                        }else if(i == 2 && chunks[currentPage].Count <= 2 && g == true)
                        {
                            Console.Write($"{new string(' ', Math.Max(0, 4))}");
                            Console.Write($"└─{new String('─', Math.Max(0, 20))}─┘");
                        }
                        Console.Write($"{new string(' ', Math.Max(0, idMax + 5))}");
                        Console.Write($"{new string(' ', Math.Max(0, firstMax + 4))}");
                        Console.Write($"{new string(' ', Math.Max(0, lastMax + 4))}");
                        Console.Write($"{new string(' ', Math.Max(0, mailMax + 4))}");
                        Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write($"\n");   
                    }
                }
                if(chunks[currentPage].Count == 10)
                {
                    Console.Write($"└─{new string('─', Math.Max(0, idMax ))}─┴─{new string('─', Math.Max(0, firstMax ))}─┴─{new string('─', Math.Max(0, lastMax ))}─┴─{new string('─', Math.Max(0, mailMax ))}─┘");
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
                    g = true;
                }
                else if (key == ConsoleKey.Escape && g)
                {
                    g = false;
                }
                currentPage = Math.Clamp(currentPage, 0, chunks.Count - 1);
                CurrentSelected = Math.Clamp(CurrentSelected, -1, chunks[currentPage].Count - 1);

            } while (key != ConsoleKey.Escape);

        }
}

// ┌ ─ ┬  ┐ 

// │   │  │ 

// ├ ─ ┼  ┤ 

// └ ─ ┴  ┘