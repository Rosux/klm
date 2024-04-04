using Newtonsoft.Json;
public class IdAccess
{
    public List<int> Get_Id()
    {
        string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSource\Id.json"));
        string string_Id = File.ReadAllText(path);
        List<int> list_Id = JsonConvert.DeserializeObject<List<int>>(string_Id)!;
        return list_Id;
    }

    public void Return_Id(List<int> list_Id)
    {
        StreamWriter writer = new(@"DataSource\Id.json");
        string string_Id = JsonConvert.SerializeObject(list_Id);
        writer.Write(string_Id);
        writer.Close();
    }
    
}