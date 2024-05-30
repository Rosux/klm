using Newtonsoft.Json;
public class FilmAcesser
{
    //Get list of all movies
    public List<Film> Get_info()
    {
        string path = Path.Combine(Environment.CurrentDirectory, "DataSource", "Films.json");
        string string_movies = File.ReadAllText(path);
        List<Film> list_movies = JsonConvert.DeserializeObject<List<Film>>(string_movies);
        return list_movies;
    }

    //Write all movies to the JSON
    public void Return_info(List<Film> list_movies)
    {
        StreamWriter writer = new(@"DataSource\Films.json");
        string string_movies = JsonConvert.SerializeObject(list_movies);
        writer.Write(string_movies);
        writer.Close();
    }
    
    //Return a list of string with all genres
    public List<string> Get_Genres()
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSource\Films.json"));
        string string_movies = File.ReadAllText(path);
        List<Film> list_movies = JsonConvert.DeserializeObject<List<Film>>(string_movies);
        List<string> allGenres = new List<string>();

        foreach (Film film in list_movies)
        {
            allGenres.AddRange(film.Genres);
        }
        List<string> uniqueGenres = allGenres.Distinct().ToList();
    
        return uniqueGenres;
    }
}