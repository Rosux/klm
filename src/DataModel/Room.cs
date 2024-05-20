using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;

public class Room
{
    public int Id;
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

    public Room(int capacity)
    {
        Capacity = capacity;
    }

    public Room(int id, int capacity)
    {
        this.Id = id;
        Capacity = capacity;
    }

    /// <summary>
    /// Create a new room. Automatically counts all the available seats.
    /// </summary>
    /// <param name="layout">A jagged array of booleans indicating where the seats are</param>
    public Room(bool[][] layout)
    {
        // set layout
        Seats = layout;
        // count the seats available (all true's)
        int capacity = 0;
        for(int i=0;i<Seats.Length;i++)
        {
            for(int j=0;j<Seats.Length;j++)
            {
                if(Seats[i][j]){
                    capacity++;
                }
            }
        }
        // set available seats
        Capacity = capacity;
    }
}