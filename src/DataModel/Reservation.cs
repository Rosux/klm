public class Reservation
{
    public int? Id = null;
    public int RoomId;
    public int UserId;
    public int GroupSize;
    public DateTime StartDate;
    public DateTime EndDate;
    public double Price;
    public string TimeLine = "";

    public Reservation(int Id, int RoomId, int UserId, int GroupSize, DateTime StartDate, DateTime EndDate, double Price, string TimeLine){
        this.Id = Id;
        this.RoomId = RoomId;
        this.UserId = UserId;
        this.GroupSize = GroupSize;
        this.StartDate = StartDate;
        this.EndDate = EndDate;
        this.Price = Price;
        this.TimeLine = TimeLine;
    }
    
    public Reservation(int RoomId, int UserId, int GroupSize, DateTime StartDate, DateTime EndDate, double Price, string TimeLine){
        this.Id = null;
        this.RoomId = RoomId;
        this.UserId = UserId;
        this.GroupSize = GroupSize;
        this.StartDate = StartDate;
        this.EndDate = EndDate;
        this.Price = Price;
        this.TimeLine = TimeLine;
    }
}