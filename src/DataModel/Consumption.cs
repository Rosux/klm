public class Consumption{
    public int Id;
    public string Name;
    public double Price;
    public TimeOnly StartTime;
    public TimeOnly EndTime;

    public Consumption(string Name, double Price, TimeOnly StartTime, TimeOnly EndTime){
        this.Name = Name;
        this.Price = Price;
        this.StartTime = StartTime;
        this.EndTime = EndTime;
    }
    public Consumption(int Id, string Name, double Price, TimeOnly StartTime, TimeOnly EndTime){
        this.Id = Id;
        this.Name = Name;
        this.Price = Price;
        this.StartTime = StartTime;
        this.EndTime = EndTime;
    }

}