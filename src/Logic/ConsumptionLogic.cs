public static class ConsumptionLogic
{
    private static ConsumptionAcces c = new ConsumptionAcces();
    public static void Consumption(){
        MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
        {"1. Add Consumption", ()=>{
        Consumption consumption = ConsumptionMenu.AddConsumptionMenu();
        bool added = c.CreateConsumption(consumption);
        }},
        {"2. Remove Consumption", ()=>{
        Consumption consumption =  ConsumptionMenu.RemoveConsumptionMenu();
        bool removed = c.DeleteConsumption(consumption);
        }},
    });
    }
}