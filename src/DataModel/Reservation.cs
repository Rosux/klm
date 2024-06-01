public class Reservation
{
    public int? Id = null;
    public int RoomId;
    public int UserId;
    public int GroupSize;
    public DateTime StartDate;
    public DateTime EndDate;
    private double _price;
    public double Price
    {
        get => _price;
        set => _price = Math.Floor(value*100d)/100d;
    }
    public TimeLine.Holder TimeLine = new TimeLine.Holder();
    public List<Entertainment> Entertainments = new List<Entertainment>();

    public Reservation(int Id, int RoomId, int UserId, int GroupSize, DateTime StartDate, DateTime EndDate, double Price, TimeLine.Holder TimeLine, List<Entertainment> Entertainments){
        this.Id = Id;
        this.RoomId = RoomId;
        this.UserId = UserId;
        this.GroupSize = GroupSize;
        this.StartDate = StartDate;
        this.EndDate = EndDate;
        this.Price = Price;
        this.TimeLine = TimeLine;
        this.Entertainments = Entertainments;
    }

    public Reservation(int RoomId, int UserId, int GroupSize, DateTime StartDate, DateTime EndDate, double Price, TimeLine.Holder TimeLine, List<Entertainment> Entertainments){
        this.Id = null;
        this.RoomId = RoomId;
        this.UserId = UserId;
        this.GroupSize = GroupSize;
        this.StartDate = StartDate;
        this.EndDate = EndDate;
        this.Price = Price;
        this.TimeLine = TimeLine;
        this.Entertainments = Entertainments;
    }
}