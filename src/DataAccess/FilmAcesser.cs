using Newtonsoft.Json;
public class FilmAcesser
{
    public List<Film> Get_info()
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSource\Films.json"));
        string string_movies = File.ReadAllText(path);
        List<Film> list_movies = JsonConvert.DeserializeObject<List<Film>>(string_movies)!;
        return list_movies;
    }

    public void Return_info(List<Film> list_movies)
    {
        StreamWriter writer = new(@"DataSource\Films.json");
        string string_movies = JsonConvert.SerializeObject(list_movies);
        writer.Write(string_movies);
        writer.Close();
    }
    public List<Genre> Get_Genres()
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSource\Films.json"));
        string string_movies = File.ReadAllText(path);
        List<Film> list_movies = JsonConvert.DeserializeObject<List<Film>>(string_movies);
        List<Genre> genres = list_movies
            .Select(film => film.Genre)
            .Distinct()
            .Select(genreName => new Genre(genreName))
            .ToList();
        return genres;
    }
}