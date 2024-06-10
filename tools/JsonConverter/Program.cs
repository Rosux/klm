namespace JsonConverter;

using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine(">Loading json...");
        string filmJsonString = File.ReadAllText("../tmdb_movies.min.json"); // thx imbd
        List<tmbdMovie> x = JsonConvert.DeserializeObject<List<tmbdMovie>>(filmJsonString);
        List<Film> newFilms = new List<Film>();
        Console.WriteLine(">Converting Json to Films...");
        foreach(tmbdMovie movie in x)
        {
            newFilms.Add(
                new Film(
                    -1,
                    movie.title,
                    movie.runtime,
                    movie.overview,
                    movie.vote_average,
                    movie.original_language,
                    movie.genres,
                    movie.release_date,
                    movie.certification,
                    movie.directors,
                    movie.cast,
                    movie.writers
                )
            );
        }
        Console.WriteLine($"> {newFilms.Count} Films have been converted.");
        // Console.WriteLine(newFilms.Count);
        // Console.WriteLine("--------------------");
        // Console.WriteLine("Id: "+newFilms[0].Id);
        // Console.WriteLine("Title: "+newFilms[0].Title);
        // Console.WriteLine("Runtime: "+newFilms[0].Runtime);
        // Console.WriteLine("Description: "+newFilms[0].Description);
        // Console.WriteLine("Rating: "+newFilms[0].Rating);
        // Console.WriteLine("Language: "+newFilms[0].Language);
        // Console.WriteLine("Genres: "+ListToString(newFilms[0].Genres));
        // Console.WriteLine("ReleaseDate: "+newFilms[0].ReleaseDate);
        // Console.WriteLine("Certification: "+newFilms[0].Certification);
        // Console.WriteLine("Directors: "+ListToString(newFilms[0].Directors));
        // Console.WriteLine("Actors: "+ListToString(newFilms[0].Actors));
        // Console.WriteLine("Writers: "+ListToString(newFilms[0].Writers));

        Console.WriteLine("Saving films.");
        Directory.CreateDirectory("./ResultingData");
        Environment.SetEnvironmentVariable("MEDIA_PATH", "./ResultingData/Media.json");

        CreateFile();
        MediaJsonStruct mediaStructure = new MediaJsonStruct();

        foreach(Film film in newFilms)
        {
            film.Id = (++mediaStructure.CurrentFilmId);
            mediaStructure.Films.Add(film);
        }

        SetMedia(mediaStructure);

        Console.WriteLine("Films saved. Find them at ./ResultingData/Media.json");
    }

    private static void SetMedia(MediaJsonStruct mediaStructure)
    {
        CreateFile();
        StreamWriter jsonWriter = new StreamWriter(Environment.GetEnvironmentVariable("MEDIA_PATH"));
        jsonWriter.Write(JsonConvert.SerializeObject(mediaStructure));
        jsonWriter.Close();
    }

    private static void CreateFile()
    {
        // if the file does not exist: create the file, write the default structure to the file.
        if(!File.Exists(Environment.GetEnvironmentVariable("MEDIA_PATH"))){
            File.WriteAllText(Environment.GetEnvironmentVariable("MEDIA_PATH"), JsonConvert.SerializeObject(new MediaJsonStruct()));
        }
    }

    public static string ListToString<T>(List<T> items)
    {
        if(items.Count == 0){return "[None]";}
        string result = "[";
        foreach(T t in items){
            result += $"{t.ToString()}, ";
        }
        return result;
    }
}
