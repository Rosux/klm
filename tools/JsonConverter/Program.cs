using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JsonConverter;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("Loading json...");
        string filmJsonString = File.ReadAllText("../tmdb_movies.min.json"); // thx imbd
        List<tmbdMovie> x = JsonConvert.DeserializeObject<List<tmbdMovie>>(filmJsonString);
        List<Film> newFilms = new List<Film>();
        Console.WriteLine("Converting Json to Films...");
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
        Console.WriteLine($"{newFilms.Count} Films have been converted.");
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

        Directory.CreateDirectory("./ResultingData");
        Environment.SetEnvironmentVariable("MEDIA_PATH", "./ResultingData/Media.json");

        CreateFile();
        MediaJsonStruct mediaStructure = new MediaJsonStruct();

        Console.WriteLine("Saving films.");
        foreach(Film film in newFilms)
        {
            film.Id = (++mediaStructure.CurrentFilmId);
            mediaStructure.Films.Add(film);
        }

        Console.WriteLine("Fetching series...");
        List<Serie> series = await ConvertSeries(4);
        Console.WriteLine("\nSaving series.");
        foreach(Serie s in series)
        {
            s.Id = (++mediaStructure.CurrentSerieId);
            mediaStructure.Series.Add(s);
        }

        SetMedia(mediaStructure);

        Console.WriteLine("Films and Series saved. You can find them at ./ResultingData/Media.json");
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

    public static readonly string baseUrl = "https://api.tvmaze.com";
    public static async Task<List<Serie>> ConvertSeries(int count)
    {
        List<Serie> series = new List<Serie>();
        for(int i=1;i<=count;i++)
        {
            Console.CursorVisible = false;
            Console.Write("");
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"fetching serie {i} of {count}...");
            Thread.Sleep(1000);
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.GetAsync($"{baseUrl}/shows/{i}?embed[]=episodes&embed[]=seasons&embed[]=crew&embed[]=cast");

            if(res.IsSuccessStatusCode){
                string jsonData = await res.Content.ReadAsStringAsync();

                Show x = JsonConvert.DeserializeObject<Show>(jsonData);

                List<ShowSeason> seasons = x.embeds.seasons;
                List<ShowEpisode> episodes = x.embeds.episodes;

                List<string> patterns = new List<string>(){
                    "creator",
                };
                List<string> directors = new List<string>();
                foreach(ShowCrew crew in x.embeds.crew)
                {
                    if(patterns.Any(s=>crew.type.ToLower().Contains(s)))
                    {
                        directors.Add(crew.person.name);
                    }
                }

                List<string> cast = new List<string>();
                foreach(ShowCast c in x.embeds.cast)
                {
                    cast.Add(c.person.name);
                }

                List<Season> createdSeasons = new List<Season>();
                foreach(ShowSeason season in seasons)
                {
                    createdSeasons.Add(
                        new Season(
                            -1,
                            season.name == "" ? $"Season {season.number}" : season.name,
                            season.number,
                            new List<Episode>()
                        )
                    );
                }

                foreach(ShowEpisode episode in episodes)
                {
                    Episode createdEpisode = new Episode(
                        -1,
                        episode.name,
                        episode.runtime,
                        episode.number,
                        (float)(Math.Round((float)episode.rating.average, 2)),
                        cast
                    );
                    foreach(Season s in createdSeasons)
                    {
                        if(s.SeasonNumber == episode.season)
                        {
                            s.Episodes.Add(createdEpisode);
                            break;
                        }
                    }
                }

                List<Genre> genres = new List<Genre>();
                foreach(string g in x.genres)
                {
                    if(Enum.TryParse(g, true, out Genre genre))
                    {
                        genres.Add(genre);
                    }
                }

                Serie createdSerie = new Serie(
                    -1,
                    x.name,
                    -1,
                    Regex.Replace(x.summary, "<.*?>", string.Empty),
                    0.0f,
                    x.language,
                    genres,
                    DateOnly.ParseExact(x.premiered, "yyyy-MM-dd"),
                    Certification.NONE,
                    directors,
                    false,
                    new List<Season>()
                );
                foreach(Season s in createdSeasons)
                {
                    createdSerie.Seasons.Add(s);
                }
                series.Add(createdSerie);

            }else{
                Console.WriteLine($"Serie fetch resulted in: {res.StatusCode}");
                break;
            }

            client.Dispose();
        }
        return series;
    }
}
