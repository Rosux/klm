public class Serie : Media
{
    public string Title;
    public string Genre;
    public List<Season> Seasons;
    public int Id;
    public Serie(string title = "", string genre = "", int id = 0)
    {
        Id = id;
        Title = title;
        Genre = genre;
        Seasons = new List<Season>();
    }
}