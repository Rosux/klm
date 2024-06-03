public abstract class Media
{
    public int Id = -1;
    public string Title = "";
    public int Runtime = 0;
    public string Description = "";
    public float Rating = 0.0f;
    public string Language = "";
    public List<Genre> Genres = new List<Genre>();
    public DateOnly ReleaseDate = DateOnly.MinValue;
    public Certification Certification = Certification.NONE;
    public List<string> Directors = new List<string>();
}