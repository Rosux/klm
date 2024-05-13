using Newtonsoft.Json;

public class Film : Media, IComparable
{
    public string Title;
    public string Genre;
    public double Rating = 00.00;
    public int Id;
    public int Duration;

    public Film(string title, string genre, int duration, int id = -1)
    {
        Id = id;
        Genre = genre;
        Title = title;
        Duration = duration;
    }

    [JsonConstructor]
    public Film(int id, string title, string genre, int duration)
    {
        Id = id;
        Genre = genre;
        Title = title;
        Duration = duration;
    }

    public int CompareTo(object obj)
    {
        if (!(obj is Film) && !(obj is Serie))
        {
            throw new ArgumentException("Object is not a Film or Serie");
        }
        double objRating = (obj is Film) ? ((Film)obj).Rating : ((Serie)obj).Rating;
        return Rating.CompareTo(objRating);
    }
}
