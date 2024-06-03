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