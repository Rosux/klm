using Newtonsoft.Json;

public class Room
{
    public int Id;
    public string RoomName;
    private int _Capacity;
    public int Capacity {
        get{
            return _Capacity;
        }
        set{
            _Capacity = Math.Max(value, 1);
        }
    } // we keep capacity to indicate the total amount of chairs. for optimization and ease of use.
    public bool[][] Seats; // a jagged array that holds the seat layout. true indicates there is a seat, false indicates the lack of a seat

    /// <summary>
    /// Create a new room. Automatically counts all the available seats.
    /// </summary>
    /// <param name="id">The ID of the room.</param>
    /// <param name="layout">A jagged array of booleans indicating where the seats are.</param>
    public Room(int id, bool[][] layout, string roomName)
    {
        Id = id;
        // set layout
        Seats = layout;
        // count the seats available (all true's)
        int capacity = 0;
        for(int i=0;i<Seats.Length;i++)
        {
            for(int j=0;j<Seats[i].Length;j++)
            {
                if(Seats[i][j]){
                    capacity++;
                }
            }
        }
        // set available seats
        Capacity = capacity;
        RoomName = roomName;
    }

    /// <summary>
    /// Create a new room. Automatically counts all the available seats.
    /// </summary>
    /// <param name="layout">A jagged array of booleans indicating where the seats are.</param>
    public Room(bool[][] layout, string roomName) : this(-1, layout, roomName){}

    /// <summary>
    /// Create a new room based on JSON data. Automatically counts all the available seats.
    /// </summary>
    /// <param name="jsonString">A string containing the room seats data.</param>
    public Room(string jsonString, string roomName) : this(JsonConvert.DeserializeObject<bool[][]>(jsonString), roomName){}

    /// <summary>
    /// Create a new room with ID. Automatically counts all the available seats.
    /// </summary>
    /// <param name="id">The ID of the room</param>
    /// <param name="jsonString">A string containing the room seats data.</param>
    public Room(int id, string jsonString, string roomName) : this(id, JsonConvert.DeserializeObject<bool[][]>(jsonString), roomName){}


}