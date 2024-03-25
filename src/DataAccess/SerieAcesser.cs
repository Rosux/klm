using Newtonsoft.Json;
public class SerieAcesser
{
    public List<Serie> Get_info()
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSource\Series.json"));
        string string_series = File.ReadAllText(path);
        // StreamReader reader = new("Series.json");
        // string string_series = reader.ReadToEnd();
        // reader.Close();
        List<Serie> list_series = JsonConvert.DeserializeObject<List<Serie>>(string_series)!;
        return list_series;
    }

    public void Return_info(List<Serie> list_series)
    {
        StreamWriter writer = new(@"DataSource\Series.json");
        string string_series = JsonConvert.SerializeObject(list_series);
        writer.Write(string_series);
        writer.Close();
    }
}