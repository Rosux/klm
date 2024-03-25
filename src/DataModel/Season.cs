public class Season
{
    public string Title;
    public List<Episode> Episodes;
    public int Id;
    public Season(string title = "",int id = 0)
    {
        Id = id;
        Title = title;
        Episodes = new List<Episode>();
    }
}