public class RoomMenu
{   
    private static RoomAccess r = new RoomAccess();
    public static void AdminOverView()
    {
        bool running = true;
        while(running)
        {
            /// gives the admin an UI with all options
            Console.WriteLine("Welcome to Admin overview for Rooms");
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. Show all rooms", ()=>{
                    Action1();
                }},
                {"2. Add a room", ()=>{
                    Action2();
                }},
                {"3. Remove a room", ()=>{
                    Action3();
                }},
                {"4. Edit a room", ()=>{
                    Action4();
                }},
                {"5. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }
    /// calls the method print allrooms
    private static void Action1()
    {
        Console.Clear();
        PrintAllRooms();
    }
    /// first asks the user for a capacity after that calls the method createroom with the given capacity to create a new room
    public static void Action2()
    {
        Console.Clear();
        /// asks the user for a capcity he can also cancel action
        int? GivenCapacity_p = MenuHelper.SelectInteger("Select capacity for new room: ", "", true, 0, 1, 2147483647);
        if (GivenCapacity_p  != null)
        {
            int GivenCapacity = (int)GivenCapacity_p;
            RoomLogic.CreateRoom(GivenCapacity);
        }
        else
        {
            Console.WriteLine("Action cancelled.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
    /// first cheks if there even are rooms then calls the method ChooseRoom wich returns a Room or null
    /// if it returns a room it calls the method RemoveRoom with the chosen room
    /// if it returns null it cancels the action
    public static void Action3()
    {
        List<Room> listroom = r.GetAllRooms();
        /// checks if there even are rooms
        if (listroom.Count != 0)
        {
            /// lets user choose room from all rooms
            Room? selectRoom_p = RoomLogic.ChooseRoom("Pick room to remove");
            if (selectRoom_p == null)
            {
                Console.WriteLine("Action cancelled.");
                Console.Write($"\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
            else
            {
                Room selectedRoom = (Room)selectRoom_p;
                RoomLogic.RemoveRoom(selectedRoom);
            }
        }
        else
        {
            Console.WriteLine("There are no rooms to remove.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
    /// lets user edit a room the room is either given via the parameter or chosen with method ChooseRoom
    /// <param name="selectedroom">Room object to edit.</param>
    public static void Action4(Room selectedroom = null)
    {
        List<Room> listroom = r.GetAllRooms();
        /// checks if there even are rooms
        if (listroom.Count != 0)
        {
            Room? selectRoom_p = null;
            /// checks if there is a room given via paramaeter else lets user choose a room with method ChooseRoom
            if(selectedroom != null)
            {
                selectRoom_p = selectedroom;
            }
            else
            {
                /// lets user choose room from all rooms to edit
                selectRoom_p = RoomLogic.ChooseRoom("Pick room to edit");
            }
            if (selectRoom_p == null)
            {
                Console.WriteLine("Action cancelled.");
                Console.Write($"\n\nPress any key to continue...");
                Console.ReadKey(true);
            }
            else
            {
                Room selectedRoom = (Room)selectRoom_p;
                /// asks user for new capacity
                int? new_capacity_p = MenuHelper.SelectInteger("Select new capacity: ", $"Current capacity: {selectedRoom.Capacity}.", true, 0, 1, 2147483647);
                if(new_capacity_p == null)
                {
                    Action4();
                }
                else
                {
                    /// cals method EditRoom with the slected room and new capacity
                    int  new_capacity = (int) new_capacity_p;
                    RoomLogic.EditRoom(selectedRoom, new_capacity);
                }
            }
        }
        else
        {
            Console.WriteLine("There are no rooms to edit.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    /// gives the user an Interface where he can see all the rooms 
    /// each page holds a maximum of 10 rooms
    private static void PrintAllRooms()
    {
        int page = 0;
        int i = 0;
        int j = 0;
        /// holds the pages
        List<List<Room>> allroomlist = new List<List<Room>>();
        /// holds all rooms
        List<Room> roomlist_room = r.GetAllRooms();
        /// is a list that gets filled with all rooms per page and then added to allroomlist
        List<Room> roomlist_p = new List<Room>();
        /// holds longest sting per page
        List<int> alllength = new List<int>();
        int longest = 0;
        bool isEmpty = !roomlist_room.Any();
        /// checks for longest string per page and adds it to list alllength
        foreach(Room room in roomlist_room)
        {
            string room_str = $"Id: {room.Id}, capacity: {room.Capacity}";
            if(room_str.Length > longest)
            {
                longest = room_str.Length;
            }
            if(i == 9)
            {
                alllength.Add(longest);
                longest = 0;
                i = 0;
            }
            i++;
        }
        if(longest != 0)
        {
            alllength.Add(longest);
        }
        /// checks if there even are rooms
        if(!isEmpty)
        {
            i = 0;
            /// makes and adds all pages to allroomlist
            foreach(Room room in roomlist_room)
            {
                roomlist_p.Add(room);
                i++;
                if (i == 10)
                {
                    allroomlist.Add(roomlist_p);
                    roomlist_p = new List<Room>();
                    i = 0;
                }
            }
            if(roomlist_p.Count != 0)
            {
                allroomlist.Add(roomlist_p);
            }
            /// prints the UI
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("Press escape to exit.\n");
                /// checks if the longest string is even or odd so the printing of the UI is corect
                if(alllength[page] % 2 == 0)
                {
                    j = 2;
                }
                else
                {
                    j = 1;
                }
                ///prints header
                Console.WriteLine($"┌─All rooms{new String('─', Math.Max(0, alllength[page] - "All rooms".Length))}─┐");
                /// prints all info
                foreach(Room room in allroomlist[page])
                {
                    string zin = $"Id: {room.Id}, capacity: {room.Capacity}";
                    Console.Write("│ ");
                    Console.Write(zin);
                    Console.Write($"{new String(' ', Math.Max(0, alllength[page] - zin.Length))} │\n");
                }
                /// prints the arrows under the page
                if(allroomlist.Count == 1 )
                {
                    Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                    Console.Write($"│ {new String(' ', Math.Max(0, alllength[page]/2 - 2 ))} {page+1}/{allroomlist.Count}");
                    Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2 - 1 * j ))} │\n");
                    Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                }
                else
                {
                    if(page == 0)
                    {
                        Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                        Console.Write($"│ {new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length + 1 ))} {page+1}/{allroomlist.Count}");
                        Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length - 1 * j ))} -> │\n");
                        Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                    }
                    else if(page == allroomlist.Count-1)
                    {
                        Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                        Console.Write($"│ <- {new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length - 2 ))} {page+1}/{allroomlist.Count}");
                        Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length + 2 - j + 1 ))} │\n");
                        Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                    }
                    else
                    {
                        Console.Write($"├─{new string('─', Math.Max(0, alllength[page] ))}─┤\n");
                        Console.Write($"│ <-{new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length - 1 ))} {page+1}/{allroomlist.Count}");
                        Console.Write($"{ new String(' ', Math.Max(0, alllength[page]/2 - $"{page+1}/{allroomlist.Count}".Length - j ))} -> │\n");
                        Console.Write($"└─{new string('─', Math.Max(0, alllength[page] ))}─┘\n");
                    }
                }
                /// lets user go through pages
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.RightArrow && page != allroomlist.Count-1)
                {  
                    Console.Clear();
                    page ++;
                }
                else if (key == ConsoleKey.LeftArrow && page != 0)
                {  
                    Console.Clear();
                    page --;

                }
            } while (key != ConsoleKey.Escape);   
        }
        else
        {
            Console.WriteLine("There are no rooms currently.");
            Console.Write($"\n\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
}