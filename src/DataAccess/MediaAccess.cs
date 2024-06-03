using System;
using Newtonsoft.Json;

public static class MediaAccess
{
    private static string _mediaPath = Environment.GetEnvironmentVariable("MEDIA_PATH") ?? "";

    public static bool SaveMedia<T>(T media){
        if(Environment.GetEnvironmentVariable("MEDIA_PATH") == null){
            throw new Exception("Environment MEDIA_PATH not set.");
        }
        if(typeof(T) != typeof(Film) && typeof(T) != typeof(Serie)){
            return false;
        }


        if(!File.Exists(_mediaPath)){
            File.Create(_mediaPath);
            StreamWriter jsonWriter = new StreamWriter(_mediaPath);
            // jsonWriter.Write();
        }

        string mediaJsonString = File.ReadAllText(_mediaPath);
        MediaJsonStructure mediaStructure = JsonConvert.DeserializeObject<MediaJsonStructure>(mediaJsonString);


        // StreamWriter jsonWriter = new StreamWriter(_mediaPath);



        return false;
    }

    // current Id    LastInsertedId = 2032
    // [2033,
    // .[filmsList]]

    // [[serieId, []], [filmId, []]]
}

public struct MediaJsonStructure
{
    public KeyValuePair<int, List<Film>> Films { get; set; } = new KeyValuePair<int, List<Film>>(-1, new List<Film>());
    public KeyValuePair<int, List<Serie>> Series { get; set; } = new KeyValuePair<int, List<Serie>>(-1, new List<Serie>());

    public MediaJsonStructure(){}
    public MediaJsonStructure(KeyValuePair<int, List<Film>> films, KeyValuePair<int, List<Serie>> series)
    {
        this.Films = films;
        this.Series = series;
    }
}