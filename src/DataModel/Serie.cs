public class Serie : Media, IComparable
{
    public string Title;
    public string Genre;
    public double Rating;
    public List<Season> Seasons;
    public int Id;
    public Serie(string title = "", string genre = "", int id = 0)
    {
        Id = id;
        Title = title;
        Genre = genre;
        Seasons = new List<Season>();
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