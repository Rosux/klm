public static class ConsumptionMenu
{   
    private static ConsumptionAcces c = new ConsumptionAcces();

    public static Consumption? AddConsumptionMenu()
    {
    string Name = "";
    double Price = 0;
    TimeOnly StartTimer = new TimeOnly(), EndTimer = new TimeOnly();
    bool adding = true;
    do
    {
        //New menuhelper added on the adding consumption
        MenuHelper.SelectOptions("Adding Consumption", new Dictionary<string, Action>()
        {
            {"Name", () =>
                {
                    Console.WriteLine("Enter the name of the product:");
                    Name = Console.ReadLine().Trim();
                    if (string.IsNullOrWhiteSpace(Name))
                    {
                        Console.WriteLine("Name cannot be empty. Please try again.");
                        return;
                    }
                }
            },
            {"Price", () =>
                {
                    Console.WriteLine("Enter the price of the product:");
                    if (!double.TryParse(Console.ReadLine(), out Price))
                    {
                        Console.WriteLine("Invalid price format. Please try again.");
                        return;
                    }
                }
            },
            {"Start time", () =>
                {
                    Console.WriteLine("Enter the opening time of the product (HH:MM):");
                    StartTimer = MenuHelper.SelectTime(StartTimer);
                }
            },
            {"End time", () =>
                {
                    Console.WriteLine("Enter the closing time of the product (HH:MM):");
                    EndTimer = MenuHelper.SelectTime(EndTimer);
                }
            },
            {"Add Item", () =>
                {
                    if (string.IsNullOrWhiteSpace(Name) || Price <= 0)
                    {
                        Console.WriteLine("Please fill in all fields before adding the item.");
                        return;
                    }else {
                        adding = false;
                    }
                }
            },
            {"Discard Changes", () =>
                {
                    adding = false; //Exit the loop if Admin decides to discard 
                }
            }
        });
    }  while (adding);
    if (!string.IsNullOrWhiteSpace(Name) && Price > 0 )
    {
        Consumption consumption = new Consumption(Name, Price, StartTimer, EndTimer);
        return consumption;
    }

    return null;
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
                {"Discard Changes", () =>
                {
                    Console.WriteLine("Changes discarded.");
                    editedConsumption = null; 
                    changing = false; //Exit the loop if Admin decides to discard 
                }
            }
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
        Console.WriteLine("There is nothing to edit.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
    public static void NoItemsToRemove(){
        Console.Clear();
        Console.WriteLine("There are no products stored to remove.\n\nPress any key to continue");
        Console.ReadKey(true);
    }
        public static void NoItemsToAdd(){
        Console.Clear();
        Console.WriteLine("Changes discarded.\nPress any key to continue");
        Console.ReadKey(true);
    }
}