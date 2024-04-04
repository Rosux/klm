public static class ReservationOverviewLogic
{
    public static void ReservationOverview()
    {
                bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. Show all rooms", ()=>{
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
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"2. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }
}