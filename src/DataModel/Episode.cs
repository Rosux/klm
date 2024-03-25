
public class Episode
{
    public string Title;
    public int Length;
    public int Id;
    public Episode(string title, int length, int id = 0)
    {
        Id = id;
        Length = length;
        Title = title;
    }  
}