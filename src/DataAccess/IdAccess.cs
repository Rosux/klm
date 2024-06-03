using Newtonsoft.Json;
public class IdAccess
{
    public List<int> Get_Id()
    {
        if(Environment.GetEnvironmentVariable("ID_PATH") == null){
            throw new Exception("Environment ID_PATH not set.");
        }
        string path = Environment.GetEnvironmentVariable("ID_PATH");
        string string_Id = File.ReadAllText(path);
        List<int> list_Id = JsonConvert.DeserializeObject<List<int>>(string_Id)!;
        return list_Id;
    }

    public void Return_Id(List<int> list_Id)
    {
        if(Environment.GetEnvironmentVariable("ID_PATH") == null){
            throw new Exception("Environment ID_PATH not set.");
        }
        StreamWriter writer = new(Environment.GetEnvironmentVariable("ID_PATH"));
        string string_Id = JsonConvert.SerializeObject(list_Id);
        writer.Write(string_Id);
        writer.Close();
    }
    
}