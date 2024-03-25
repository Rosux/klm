public class RoomMenu
{   
    
    public static void AdminOverView()
    {
        

        Console.Clear();
        Console.WriteLine("Welcome to Admin overview for Rooms\nSelect an action:\n1. Show all rooms\n2. Add a room\n3. Remove a room\n4. Edit a room\n5. Show specified room\n6. Exit");
        if (int.TryParse(Console.ReadLine(), out int option))
        {
            switch(option)
            {
                case 1:
                    Action1();
                    break;
                case 2:
                    Action2();
                    break;
                case 3:
                    Action3();
                    break;
                case 4:
                    Action4();
                    break;
                case 5:
                    Action5();
                    break;
                case 6:
                    break;
                default:
                    Console.WriteLine("Fill in a valid number");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
            Console.ReadKey();
            AdminOverView();
        }


    }
    private static void Action1()
    {
        Console.Clear();
        PrintAllRooms();
        Console.WriteLine("\n\nPress any key to continue");
        Console.ReadKey();
        AdminOverView();
    }
    private static void Action2()
    {
        Console.Clear();
        Console.Write("Creating new room:\nCapacity: ");
        if (int.TryParse(Console.ReadLine(), out int GivenCapacity))
        {
            RoomLogic.CreateRoom(GivenCapacity);
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.\n\n New room not created.");
            Console.ReadKey();
            AdminOverView();
        }
        Console.Clear();
        AdminOverView();
    }
    private static void Action3()
    {
        Console.Clear();
        Console.Write("Removing room:\nRoom id: ");
        string givenid = Console.ReadLine();
        bool checkParse = int.TryParse(givenid, out int roomid);
        if(checkParse == true)
        {
            RoomLogic.RemoveRoom(roomid);
        }
        else
        {
            Console.Write("\nInvalid parameter given, please insert a number.\n\nPress any key to continue...");
            Console.ReadKey();
        }
        Console.Clear();
        AdminOverView();
    }
    private static void Action4()
    {
        Console.Clear();
        Console.Write("Editing room:\nRoom id:");
        string givenid = Console.ReadLine();
        Console.Write("Change room capacity to: ");
        string givenCapacity = Console.ReadLine();
        bool checkParseCapacity = int.TryParse(givenCapacity, out int capacity);
        bool checkParseId = int.TryParse(givenid, out int roomid);
        if(checkParseId == true && checkParseCapacity == true)
        {
            RoomLogic.EditRoom(capacity, roomid);
        }
        else
        {
            Console.Write("\nInvalid parameter given, please insert a number.\n\nPress any key to continue...");
            Console.ReadKey();
        }
        Console.Clear();
        AdminOverView();
    }
    private static void Action5()
    {
        Console.Clear();
        Console.Write("Fetch specific room:\nRoom id: ");
        string givenid = Console.ReadLine();
        bool checkParse = int.TryParse(givenid, out int roomid);
        if(checkParse == true)
        {  
            RoomLogic.FetchRoom(roomid);
        }
        else
        {
            Console.Write("\nInvalid parameter given, please insert a number.\n\nPress any key to continue...");
            Console.ReadKey();
        }
        Console.Clear();
        AdminOverView();

    }
    private static void PrintAllRooms()
    {
        List<string> roomlist = RoomLogic.GetAllRooms();
        bool isEmpty = !roomlist.Any();
        if(!isEmpty)
        {
            foreach(string rooms in roomlist)
            {
                Console.WriteLine(rooms);
            }
        } 
        else
        {
            Console.WriteLine("There are currently no rooms.");
        } 
    }
}