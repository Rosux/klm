using Newtonsoft.Json;

public static class MediaAccess
{
    private static string _mediaPath = Environment.GetEnvironmentVariable("MEDIA_PATH") ?? "";

    /// <summary>
    /// Saves the given media.
    /// </summary>
    /// <param name="media">The Film/Serie object to save.</param>
    /// <typeparam name="T">Can either be Film or Serie.</typeparam>
    /// <returns>A boolean indicating if the media got saved.</returns>
    public static bool AddMedia<T>(T media)
    {
        // if environment is not set throw an exception
        if(Environment.GetEnvironmentVariable("MEDIA_PATH") == null){
            throw new Exception("Environment MEDIA_PATH not set.");
        }
        // if the type is not Film or Serie dont save it
        if(typeof(T) != typeof(Film) && typeof(T) != typeof(Serie)){
            return false;
        }

        CreateFile();

        // convert json to a MediaJsonStruct
        MediaJsonStruct mediaStructure = GetMedia();

        // add the media to the correct list and increment the current id of that list
        if(media is Film film){
            film.Id = (++mediaStructure.CurrentFilmId);
            mediaStructure.Films.Add(film);
        }else if(media is Serie serie){
            serie.Id = (++mediaStructure.CurrentSerieId);
            mediaStructure.Series.Add(serie);
        }

        // save the MediaJsonStruct to the json file
        SetMedia(mediaStructure);

        SearchAccess.UpdateMedia();
        return true;
    }

    /// <summary>
    /// Gets all the films.
    /// </summary>
    /// <returns>A list of all the films.</returns>
    public static List<Film> GetAllFilms()
    {
        return GetMedia().Films;
    }

    /// <summary>
    /// Gets all the series.
    /// </summary>
    /// <returns>A list of all the series.</returns>
    public static List<Serie> GetAllSeries()
    {
        return GetMedia().Series;
    }

    /// <summary>
    /// Gets all media (both films and series).
    /// </summary>
    /// <returns>A list of all media.</returns>
    public static List<Media> GetAllMedia()
    {
        MediaJsonStruct media = GetMedia();
        List<Media> allMedia = new List<Media>();
        allMedia.AddRange(media.Films);
        allMedia.AddRange(media.Series);
        return allMedia;
    }

    /// <summary>
    /// Returns a MediaJsonStruct holding the films/series and the current film/serie id.
    /// </summary>
    /// <returns>A MediaJsonStruct holding the films/series data.</returns>
    private static MediaJsonStruct GetMedia()
    {
        CreateFile();
        string mediaJsonString = File.ReadAllText(_mediaPath);
        return JsonConvert.DeserializeObject<MediaJsonStruct>(mediaJsonString);
    }

    /// <summary>
    /// Saves the given MediaJsonStruct to a json file.
    /// </summary>
    /// <param name="mediaStructure">A MediaJsonStruct holding the new data.</param>
    private static void SetMedia(MediaJsonStruct mediaStructure)
    {
        CreateFile();
        StreamWriter jsonWriter = new StreamWriter(_mediaPath);
        jsonWriter.Write(JsonConvert.SerializeObject(mediaStructure));
        jsonWriter.Close();
    }

    /// <summary>
    /// Create the data file if it does not currently exist.
    /// </summary>
    private static void CreateFile()
    {
        // if the file does not exist: create the file, write the default structure to the file.
        if(!File.Exists(_mediaPath)){
            File.WriteAllText(_mediaPath, JsonConvert.SerializeObject(new MediaJsonStruct()));
        }
    }
}