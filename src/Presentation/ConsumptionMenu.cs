public static class ConsumptionMenu
{
    private static ConsumptionAcces c = new ConsumptionAcces();
    public static Consumption AddConsumptionMenu(){
        Console.WriteLine("Please type the name of the product.");  
        string Name = Console.ReadLine();
        Console.WriteLine("Please provide the price of the product.");
        double Price = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine("Please enter the starttime of when the product can be ordered (HH-MM).");
        string StartTime = Console.ReadLine();
        TimeOnly StartTimer = TimeOnly.Parse(StartTime);
        Console.WriteLine("Please enter the endtime of when the product can be ordered (HH-MM).");
        string EndTime = Console.ReadLine();
        TimeOnly EndTimer = TimeOnly.Parse(EndTime);
        Consumption consumption = new Consumption(Name, Price, StartTimer, EndTimer);
        return consumption;
    }
    public static Consumption RemoveConsumptionMenu(){
        Dictionary<string, Consumption> d = new Dictionary<string, Consumption>();
        foreach (Consumption consumption in c.ReadConsumption()){
            d.Add(consumption.Name, consumption);
        }
        return MenuHelper.SelectFromList("Select id to delete", d);

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
}