
public static class ConsumptionMenu
{
    private static ConsumptionAccess c = new ConsumptionAccess();

    #region Input
    /// <summary>
    /// Asks the user to fill in fields and return a new Consumption object with the field data.
    /// </summary>
    /// <returns>A Consumption object or NULL if the user cancels.</returns>
    public static Consumption? GetNewConsumption(){
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
    #endregion

    #region Text output for validation
    /// <summary>
    /// Notifies the user about if the consumption got updated successfully or not.
    /// </summary>
    /// <param name="success">A boolean indicating if the updating was successfull.</param>
    public static void ConsumptionUpdated(bool success){
        Console.Clear();
        if(success){
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The consumption has successfully been updated.\n\nPress any key to continue");
        }else{
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("An error occured. Please try again later.\n\nPress any key to continue");
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey(true);
    }

    /// <summary>
    /// Notifies the user about if the consumption got added successfully or not.
    /// </summary>
    /// <param name="success">A boolean indicating if the adition was successfull.</param>
    public static void ConsumptionAdded(bool success){
        Console.Clear();
        if(success){
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The consumption has successfully been added.\n\nPress any key to continue");
        }else{
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("An error occured. Please try again later.\n\nPress any key to continue");
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey(true);
    }

    /// <summary>
    /// Notifies the user about if the consumption got removed successfully or not.
    /// </summary>
    /// <param name="success">A boolean indicating if the removal was successfull.</param>
    public static void ConsumptionRemoved(bool success){
        Console.Clear();
        if(success){
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The consumption has successfully been removed.\n\nPress any key to continue");
        }else{
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("An error occured. Please try again later.\n\nPress any key to continue");
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey(true);
    }
    #endregion
}