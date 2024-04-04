using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;

public class Room
{
    public int id;
    private int _capacity;
    public int Capacity 
    { 
        get { return _capacity; }
        set
        {
            if(value < 0)
            {
                Console.WriteLine($"Invalid Room parameter: must be positive.");
                _capacity = 0;
            }
            else
            {
                _capacity = value;
            }
        }
    }
    public Room(int capacity)
    {
        Capacity = capacity;
    }
    public Room(int id, int capacity)
    {
        this.id = id;
        Capacity = capacity;
    }
}