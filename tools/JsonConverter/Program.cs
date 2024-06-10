namespace JsonConverter;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Converting Films...");
        string filmJsonString = File.ReadAllText("../tmdb_movies.json");
        Console.WriteLine(filmJsonString);
    }
}
