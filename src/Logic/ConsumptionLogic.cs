public static class ConsumptionLogic
{
    private static ConsumptionAccess c = new ConsumptionAccess();
    public static void Consumption(){
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. Add Consumption", ()=>{
                    //Retrieve product from the add menu and sends it to the database.
                    Consumption consumption = ConsumptionMenu.AddConsumptionMenu();
                    if(consumption == null){
                        ConsumptionMenu.NoItemsToAdd();
                        return;
                    }
                    bool added = c.CreateConsumption(consumption);
                }},
                {"2. Remove Consumption", ()=>{
                    //Check if there is a item to remove and if there is it send a message to get it removed.
                    Consumption consumption =  ConsumptionMenu.RemoveConsumptionMenu();
                    if (consumption == null)
                    {
                        ConsumptionMenu.NoItemsToRemove();
                        return;
                    }
                    bool removed = c.DeleteConsumption(consumption);
                }},
                {"3. Edit Consumption", ()=>{
                    // retrieve the changed product
                    Consumption editedConsumption = ConsumptionMenu.EditConsumption();
                    // if there are no products show error and return to the menu
                    if (editedConsumption == null){
                        ConsumptionMenu.NoItems();
                        return;
                    }
                    // send updated Consumption to DataAccess layer
                    bool updated = c.UpdateConsumption(editedConsumption);
                    // show user if its updated or not
                    if (updated){
                        ConsumptionMenu.Saved();
                    }else{
                        ConsumptionMenu.Error();
                    }
                }},
                {"4. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }
}