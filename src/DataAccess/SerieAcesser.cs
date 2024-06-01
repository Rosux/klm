using Newtonsoft.Json;
public class SerieAcesser
{
    public List<Serie> Get_info()
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSource\Series.json"));
        string string_series = File.ReadAllText(path);
        List<Serie> list_series = JsonConvert.DeserializeObject<List<Serie>>(string_series)!;
        return list_series;
    }

    public void Return_info(List<Serie> list_series)
    {
        StreamWriter writer = new(@"DataSource\Series.json");
        string string_series = JsonConvert.SerializeObject(list_series);
        writer.Write(string_series);
        writer.Close();
        SearchAccess.UpdateMedia();
    }

    public List<Genre> Get_Genres()
    {
        string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"DataSource\Series.json"));
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