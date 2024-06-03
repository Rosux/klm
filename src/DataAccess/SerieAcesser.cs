using Newtonsoft.Json;
public class SerieAcesser
{
    public List<Serie> Get_info()
    {
        if(Environment.GetEnvironmentVariable("SERIE_PATH") == null){
            throw new Exception("Environment SERIE_PATH not set.");
        }
        string path = Environment.GetEnvironmentVariable("SERIE_PATH");
        string string_series = File.ReadAllText(path);
        List<Serie> list_series = JsonConvert.DeserializeObject<List<Serie>>(string_series)!;
        return list_series;
    }

    public void Return_info(List<Serie> list_series)
    {
        if(Environment.GetEnvironmentVariable("SERIE_PATH") == null){
            throw new Exception("Environment SERIE_PATH not set.");
        }
        StreamWriter writer = new(Environment.GetEnvironmentVariable("SERIE_PATH"));
        string string_series = JsonConvert.SerializeObject(list_series);
        writer.Write(string_series);
        writer.Close();
        SearchAccess.UpdateMedia();
    }

    public List<Genre> Get_Genres()
    {
        if(Environment.GetEnvironmentVariable("SERIE_PATH") == null){
            throw new Exception("Environment SERIE_PATH not set.");
        }
        string path = Environment.GetEnvironmentVariable("SERIE_PATH");
        string string_series = File.ReadAllText(path);
        List<Serie> list_series = JsonConvert.DeserializeObject<List<Serie>>(string_series);
        List<Genre> genres = list_series
            .SelectMany(serie => serie.Seasons.Select(season => serie.Genre))
            .Distinct()
            .Select(genreName => new Genre(genreName))
            .ToList();
        return genres;
    }
}

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