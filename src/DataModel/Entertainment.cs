/// <summary>
/// Holds the entertainment of a specific seat
/// </summary>
public class Entertainment
{
    public int Id = -1; // id saved in the database -1 if not saved
    public DateTime Time; // when the entertainment should start
    public string Text = ""; // the text explaining what kind of entertainment should be happening (like strippers or birthday cake)
    public int SeatRow; // x axis (row)
    public int SeatColumn; // y axis (column)

    /// <summary>
    /// Create a new entertainment for a specific seat.
    /// </summary>
    /// <param name="time">When should the entertainment start.</param>
    /// <param name="text">Describes the entertainment.</param>
    /// <param name="seatRow">An integer indicating the seat row.</param>
    /// <param name="seatColumn">An integer indicating the seat column.</param>
    public Entertainment(DateTime time, string text, int seatRow, int seatColumn)
    {
        Time = time;
        Text = text;
        SeatRow = seatRow;
        SeatColumn = seatColumn;
    }

    /// <summary>
    /// Create a new entertainment for a specific seat.
    /// </summary>
    /// <param name="id">The id of the entertainment.</param>
    /// <param name="time">When should the entertainment start.</param>
    /// <param name="text">Describes the entertainment.</param>
    /// <param name="seatRow">An integer indicating the seat row.</param>
    /// <param name="seatColumn">An integer indicating the seat column.</param>
    public Entertainment(int id, DateTime time, string text, int seatRow, int seatColumn)
    {
        Id = id;
        Time = time;
        Text = text;
        SeatRow = seatRow;
        SeatColumn = seatColumn;
    }
}