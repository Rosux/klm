public class Genre
{
    public bool IsSelected { get; set; }
    public string Name { get; set; }
    public Genre()
    {
        IsSelected = false;
        Name = string.Empty;
    }
    public Genre(string name)
    {
        IsSelected = false;
        Name = name;
    }

    public void GenreSelect()
    {
        IsSelected = true; 
    }
}