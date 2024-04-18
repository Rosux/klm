
public class Film : Media
{
    public string Title;
    public string Genre;
    public int Id;
    public int Duration;
        
    public Film(string title, string genre, int duration, int id = 0)
    {
        Id = id;
        Genre = genre;
        Title = title;
        Duration = duration;
    }
}
