
public static class ConsumptionMenu
{
    private static ConsumptionAccess c = new ConsumptionAccess();
    public static Consumption? AddConsumptionMenu(){
        string prompt = "Name: \nPrice: \nStartTime: \nEndTime: \n";

        string Name;
        while(true){
            string? name = MenuHelper.SelectText(prompt+"\nType the name of the product:", "", true, 2, 30);
            if(name == null){
                return null;
            }
            if(!c.ConsumptionExists(name)){
                Name = name;
                break;
            }else{
                Console.Write($"Name must be unique!\n\nName: '{name}' already exists.\n\nPress any key to continue.");
                Console.ReadKey(true);
            }
        }
        prompt = $"Name: {Name}\nPrice: \nStartTime: \nEndTime: \n";
        double? Price = MenuHelper.SelectPrice(prompt+"\nPlease provide the price of the product:", "", true);
        if(Price == null){
            return null;
        }
        prompt = $"Name: {Name}\nPrice: {Price}\nStartTime: \nEndTime: \n";
        TimeOnly? StartTime = MenuHelper.SelectTime(prompt+"\nPlease enter the start time of when the product can be ordered:", "", true, TimeOnly.MinValue, null, null);
        if(StartTime == null){
            return null;
        }
        prompt = $"Name: {Name}\nPrice: {Price}\nStartTime: {StartTime}\nEndTime: \n";
        TimeOnly? EndTime = MenuHelper.SelectTime(prompt+"\nPlease enter the endtime of when the product can be ordered:", "", true, TimeOnly.MinValue, null, null);
        if(EndTime == null){
            return null;
        }
        prompt = $"Are you sure you want to save the following data:\nName: {Name}\nPrice: {Price}\nStartTime: {StartTime}\nEndTime: {EndTime}";
        if(!MenuHelper.Confirm(prompt)){
            return null;
        }
        Consumption consumption = new Consumption(Name, (double)Price, (TimeOnly)StartTime, (TimeOnly)EndTime);
        return consumption;
    }

    public static Consumption? RemoveConsumptionMenu(){
        List<Consumption> consumptions = c.ReadConsumption();
        if (consumptions.Count == 0) {
            NoItemsToRemove();
            return null;
        }else {
            Dictionary<string, Consumption> d = new Dictionary<string, Consumption>();
            foreach (Consumption consumption in c.ReadConsumption()){
                d.Add(consumption.Name, consumption);
            }
            return MenuHelper.SelectFromList("Select product to delete", true, d);
        }
    }

    public static Consumption? EditConsumption(){
        // get all consumptions and put them in a dictionary
        Dictionary<string, Consumption> cons = new Dictionary<string, Consumption>();
        foreach (Consumption consumption in c.ReadConsumption()){
            cons.Add(consumption.Name, consumption);
        }
        if (cons.Count == 0)
        {
            ConsumptionMenu.NoItems();
            return null;
        }
        // let the user select 1 item from the dict
        Consumption? editedConsumption = MenuHelper.SelectFromList("Select product to edit", true, cons);
        if(editedConsumption == null){
            return null;
        }
        // keep asking what the user wants to change
        bool saving = false;
        bool changing = true;
        while (changing)
        {
            MenuHelper.SelectOptions("Select product property to edit", new Dictionary<string, Action>(){
                // change the name of the Consumption
                {"Name", ()=>{
                    string prompt = $"Current Name: {editedConsumption.Name}\nCurrent Price: {editedConsumption.Price}\nCurrent StartTime: {editedConsumption.StartTime}\nCurrent EndTime: {editedConsumption.EndTime}\n";
                    string NewName;
                    while(true){
                        string? name = MenuHelper.SelectText(prompt+"\nType the new name of the product:", "", true, 2, 30);
                        if(name == null){
                            return;
                        }
                        if(!c.ConsumptionExists(name)){
                            NewName = name;
                            break;
                        }else{
                            Console.Write($"Name must be unique!\n\nName: '{name}' already exists.\n\nPress any key to continue.");
                            Console.ReadKey(true);
                        }
                    }
                    editedConsumption.Name = NewName;
                }},
                // change the price of the Consumption
                {"Price", ()=>{
                    string prompt = $"Current Name: {editedConsumption.Name}\nCurrent Price: {editedConsumption.Price}\nCurrent StartTime: {editedConsumption.StartTime}\nCurrent EndTime: {editedConsumption.EndTime}\n";
                    double? Price = MenuHelper.SelectPrice(prompt+"\nPlease provide the new price of the product:", "", true);
                    if(Price == null){
                        return;
                    }
                    editedConsumption.Price = (double)Price;
                }},
                // change the start time of the Consumption
                {"Start time", ()=>{
                    string prompt = $"Current Name: {editedConsumption.Name}\nCurrent Price: {editedConsumption.Price}\nCurrent StartTime: {editedConsumption.StartTime}\nCurrent EndTime: {editedConsumption.EndTime}\n";
                    TimeOnly? StartTime = MenuHelper.SelectTime(prompt+"\nPlease enter the new start time of when the product can be ordered:", "", true, TimeOnly.MinValue, null, null);
                    if(StartTime == null){
                        return;
                    }
                    editedConsumption.StartTime = (TimeOnly)StartTime;
                }},
                // change the end time of the Consumption
                {"End time", ()=>{
                    string prompt = $"Current Name: {editedConsumption.Name}\nCurrent Price: {editedConsumption.Price}\nCurrent StartTime: {editedConsumption.StartTime}\nCurrent EndTime: {editedConsumption.EndTime}\n";
                    TimeOnly? EndTime = MenuHelper.SelectTime(prompt+"\nPlease enter the new end time of when the product can be ordered:", "", true, TimeOnly.MinValue, null, null);
                    if(EndTime == null){
                        return;
                    }
                    editedConsumption.EndTime = (TimeOnly)EndTime;
                }},
                {"Cancel edit", ()=>{
                    changing = false;
                    saving = false;
                }},
                // return the changed consumption item to the logic layer
                {"Save changes", ()=>{
                    changing = false;
                    saving = true;
                }},
            });
        }
        if(saving){
            return editedConsumption;
        }else{
            return null;
        }
    }

    public static void Error(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("An error occured. Please try again later.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void Saved(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("Your changes are saved succesfully.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void Deleted(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("Items are removed succesfully.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void NoItems(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("There are no products stored to edit.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void NoItemsToRemove(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("There are no products stored to remove.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
}