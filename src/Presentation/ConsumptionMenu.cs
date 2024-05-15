
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

    public static Consumption RemoveConsumptionMenu(){
        List<Consumption> consumptions = c.ReadConsumption();
        if (consumptions.Count == 0) {
            Console.WriteLine("There are no consumptions available to remove.");
            return null;
        }else {
        Dictionary<string, Consumption> d = new Dictionary<string, Consumption>();
        foreach (Consumption consumption in c.ReadConsumption()){
            d.Add(consumption.Name, consumption);
        }
        return MenuHelper.SelectFromList("Select id to delete", d);
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
            return null;
        }
        // let the user select 1 item from the dict
        Consumption editedConsumption = MenuHelper.SelectFromList("Select product to edit", cons);
        // keep asking what the user wants to change
        bool changing = true;
        while (changing)
        {
            MenuHelper.SelectOptions("Select product property to edit", new Dictionary<string, Action>(){
                // change the name of the Consumption
                {"Name", ()=>{
                    Console.WriteLine("Enter the new name of the product:");
                    editedConsumption.Name = Console.ReadLine();
                }},
                // change the price of the Consumption
                {"Price", ()=>{
                    Console.WriteLine("Enter the new price of the product:");
                    editedConsumption.Price = Convert.ToDouble(Console.ReadLine());
                }},
                // change the start time of the Consumption
                {"Start time", ()=>{
                    Console.WriteLine("Enter the new opening time of the product:");
                    editedConsumption.StartTime = MenuHelper.SelectTime(editedConsumption.StartTime);
                }},
                // change the end time of the Consumption
                {"End time", ()=>{
                    Console.WriteLine("Enter the new closing time of the product:");
                    editedConsumption.EndTime = MenuHelper.SelectTime(editedConsumption.EndTime);
                }},
                // return the changed consumption item to the logic layer
                {"Save changes", ()=>{
                    changing = false;
                }},
            });
        }
        
        return editedConsumption;
    }

    public static void Error(){
        Console.Clear();
        Console.WriteLine("An error occured. Please try again later.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void Saved(){
        Console.Clear();
        Console.WriteLine("Your changes are saved succesfully.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void NoItems(){
        Console.Clear();
        Console.WriteLine("There are no products stored to edit.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void NoItemsToRemove(){
        Console.Clear();
        Console.WriteLine("There are no products stored to remove.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
}