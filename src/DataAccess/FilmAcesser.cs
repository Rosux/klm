using Newtonsoft.Json;
public class FilmAcesser
{
    //Get list of all movies
    public List<Film> Get_info()
    {
        if(Environment.GetEnvironmentVariable("FILM_PATH") == null){
            throw new Exception("Environment FILM_PATH not set.");
        }
        string path = Environment.GetEnvironmentVariable("FILM_PATH");
        string string_movies = File.ReadAllText(path);
        List<Film> list_movies = JsonConvert.DeserializeObject<List<Film>>(string_movies);
        return list_movies;
    }

    //Write all movies to the JSON
    public void Return_info(List<Film> list_movies)
    {
        if(Environment.GetEnvironmentVariable("FILM_PATH") == null){
            throw new Exception("Environment FILM_PATH not set.");
        }
        StreamWriter writer = new(Environment.GetEnvironmentVariable("FILM_PATH"));
        string string_movies = JsonConvert.SerializeObject(list_movies);
        writer.Write(string_movies);
        writer.Close();
        SearchAccess.UpdateMedia();
    }
    
    //Return a list of string with all genres
    public List<string> Get_Genres()
    {
        if(Environment.GetEnvironmentVariable("FILM_PATH") == null){
            throw new Exception("Environment FILM_PATH not set.");
        }
        string path = Environment.GetEnvironmentVariable("FILM_PATH");
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