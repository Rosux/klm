public static class ConsumptionLogic
{
    private static ConsumptionAccess _consumptionAccess = new ConsumptionAccess();

    /// <summary>
    /// Asks the user to select an option among Edit consumption and Exit to main menu.
    /// </summary>
    public static void Consumption(){
        MenuHelper.Table<Consumption>(
            _consumptionAccess.ReadConsumption(),
            new Dictionary<string, Func<Consumption, object>>(){
                {"Id", c=>c.Id},
                {"Name", c=>c.Name},
                {"Price", c=>c.Price},
                {"Starting Time", c=>c.StartTime},
                {"Ending Time", c=>c.EndTime},
            },
            false,
            true,
            true,
            new Dictionary<string, PropertyEditMapping<Consumption>>(){
                {"Name", new(c=>c.Name, GetValidName)},
                {"Price", new(c=>c.Price, GetValidPrice)},
                {"Start Time", new(c=>c.StartTime, GetValidStartTime)},
                {"Ebd Time", new(c=>c.EndTime, GetValidEndTime)},
            },
            SaveEditedConsumption,
            true,
            CreateNewConsumption,
            true,
            DeleteConsumption
        );
    }

    /// <summary>
    /// Creates a new consumption by asking the current user to fill in the fields and then saves this new consumption and notifies the user if the saving was successful.
    /// </summary>
    /// <returns>NULL in the case the user doesnt fill in all the fields. Returns a consumption instance with the new consumption data if the user filled in everything correctly.</returns>
    private static Consumption? CreateNewConsumption(){
        Consumption? newConsumption = ConsumptionMenu.GetNewConsumption();
        if(newConsumption == null){
            return null;
        }
        bool success = _consumptionAccess.CreateConsumption(newConsumption);
        if(success){
            ConsumptionMenu.ConsumptionAdded(success);
            return newConsumption;
        }else{
            return null;
        }
    }

    /// <summary>
    /// Update an edited consumption. and notifies the user if the update was successful.
    /// </summary>
    /// <param name="consumption">An edited consumption to update.</param>
    /// <returnsA boolean indicating if the update went correctly.</returns>
    private static bool SaveEditedConsumption(Consumption consumption){
        bool confirmation = MenuHelper.Confirm($"Are you sure you want to save the following edited data:\n\nId: {consumption.Id}\nName: {consumption.Name}\nPrice: {consumption.Price}\nStartTime: {consumption.StartTime}\nEndTime: {consumption.EndTime}");
        if(confirmation){
            bool success = _consumptionAccess.UpdateConsumption(consumption);
            ConsumptionMenu.ConsumptionUpdated(success);
            return success;
        }else{
            return false;
        }
    }

    /// <summary>
    /// Deletes a given consumption and returns a boolean if the deletion was succesfull with a confirmation message.
    /// </summary>
    /// <param name="consumption">The consumption instance to delete from the database.</param>
    /// <returns>A boolean indicating if the deletion was successful.</returns>
    private static bool DeleteConsumption(Consumption consumption){
        bool confirmation = MenuHelper.Confirm($"Are you sure you want to delete the following consumption:\n\nId: {consumption.Id}\nName: {consumption.Name}\nPrice: {consumption.Price}\nStartTime: {consumption.StartTime}\nEndTime: {consumption.EndTime}");
        if(confirmation){
            bool success = _consumptionAccess.DeleteConsumption(consumption);
            ConsumptionMenu.ConsumptionRemoved(success);
            return success;
        }else{
            return false;
        }
    }

    /// <summary>
    /// Asks the user to select a name and returns it.
    /// </summary>
    /// <param name="previousConsumption">The current consumption before editing any members.</param>
    /// <returns>A string of the new member value.</returns>
    private static string GetValidName(Consumption previousConsumption){
        string prompt = $"Current Name: {previousConsumption.Name}\nCurrent Price: {previousConsumption.Price}\nCurrent StartTime: {previousConsumption.StartTime}\nCurrent EndTime: {previousConsumption.EndTime}\n\n";
        string? newName = MenuHelper.SelectText(prompt+"Enter the new name of the product:", "", true, 2, 30);
        return newName ?? previousConsumption.Name;
    }

    /// <summary>
    /// Asks the user to select a price and returns it.
    /// </summary>
    /// <param name="previousConsumption">The current consumption before editing any members.</param>
    /// <returns>A string of the new member value.</returns>
    private static object GetValidPrice(Consumption previousConsumption){
        string prompt = $"Current Name: {previousConsumption.Name}\nCurrent Price: {previousConsumption.Price}\nCurrent StartTime: {previousConsumption.StartTime}\nCurrent EndTime: {previousConsumption.EndTime}\n\n";
        double? newPrice = MenuHelper.SelectPrice(prompt+"Enter the new price of the product:", "", true);
        return newPrice ?? previousConsumption.Price;
    }

    /// <summary>
    /// Asks the user to select a start time and returns it.
    /// </summary>
    /// <param name="previousConsumption">The current consumption before editing any members.</param>
    /// <returns>A string of the new member value.</returns>
    private static object GetValidStartTime(Consumption previousConsumption){
        string prompt = $"Current Name: {previousConsumption.Name}\nCurrent Price: {previousConsumption.Price}\nCurrent StartTime: {previousConsumption.StartTime}\nCurrent EndTime: {previousConsumption.EndTime}\n\n";
        TimeOnly? newStartTime = MenuHelper.SelectTime(prompt+"Enter the new start time of when the product can be ordered:", "", true, TimeOnly.MinValue, null, null);
        return newStartTime ?? previousConsumption.StartTime;
    }

    /// <summary>
    /// Asks the user to select a end time and returns it.
    /// </summary>
    /// <param name="previousConsumption">The current consumption before editing any members.</param>
    /// <returns>A string of the new member value.</returns>
    private static object GetValidEndTime(Consumption previousConsumption){
        string prompt = $"Current Name: {previousConsumption.Name}\nCurrent Price: {previousConsumption.Price}\nCurrent StartTime: {previousConsumption.StartTime}\nCurrent EndTime: {previousConsumption.EndTime}\n\n";
        TimeOnly? newEndTime = MenuHelper.SelectTime(prompt+"Enter the new end time of when the product can be ordered:", "", true, TimeOnly.MinValue, null, null);
        return newEndTime ?? previousConsumption.StartTime;
    }
}