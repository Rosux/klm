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
}