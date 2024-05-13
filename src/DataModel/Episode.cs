using Newtonsoft.Json;

public class Episode : Media
{
    public string Title;
    public int Length;
    public int Id;

    public Episode(string title, int length, int id = -1)
    {
        Id = id;
        Length = length;
        Title = title;
    }


    [JsonConstructor]
    public Episode(int id, string title, int length)
    {
        Id = id;
        Length = length;
        Title = title;
    }
    public Episode(int id, string title, int length)
    {
        Id = id;
        Length = length;
        Title = title;
    }
}