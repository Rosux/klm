public class Film : Media
{
    public List<string> Actors = new List<string>();
    public List<string> Writers = new List<string>();

    public Film(int id, string title, int runtime, string description, float rating, string language, List<Genre> genres, DateOnly releaseDate, Certification certification, List<string> directors, List<string> actors, List<string> writers) : base(id, title, runtime, description, rating, language, genres, releaseDate, certification, directors)
    {
        this.Actors = actors;
        this.Writers = writers;
    }

    public Film(string title, int runtime, string description, float rating, string language, List<Genre> genres, DateOnly releaseDate, Certification certification, List<string> directors, List<string> actors, List<string> writers) : base(-1, title, runtime, description, rating, language, genres, releaseDate, certification, directors)
    {
        this.Actors = actors;
        this.Writers = writers;
    }
}
