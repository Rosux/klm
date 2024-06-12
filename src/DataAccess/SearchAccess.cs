/// <summary>
/// This class holds methods for searching a specific movie/series
/// </summary>
public class SearchAccess
{
    private static List<Film> AllFilms = new List<Film>();
    private static List<Serie> AllSeries = new List<Serie>();
    public SearchAccess(){
        AllFilms = MediaAccess.GetAllFilms();
        AllSeries = MediaAccess.GetAllSeries();
    }

    public static void UpdateMedia(){
        AllFilms = MediaAccess.GetAllFilms();
        AllSeries = MediaAccess.GetAllSeries();
    }

    /// <summary>
    /// Search a movie/series by string seperated by ',' and return a list of Media.
    /// </summary>
    /// <param name="searchPattern">A string containing Title/Genre text optionally with seperator , between multiple search options.</param>
    /// <returns>A list of Media objects holding every movie/serie where the title/genre contains the searchPattern.</returns>
    public List<Media> Search(string searchPattern){
        List<Media> results = new List<Media>();
        List<Serie> AllSerie = AllSeries;
        List<Film> AllMovies = AllFilms;
        string[] tokens = searchPattern.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries);
        foreach (Film film  in AllMovies){
            foreach (string token in tokens){
                string lowerToken = token.ToLower();
                if(
                    film.Title.ToLower().Contains(lowerToken) ||
                    film.Genres.Any(g => g.ToString().ToLower().Contains(lowerToken)) ||
                    film.Directors.Any(d => d.ToLower().Contains(lowerToken))
                ){
                    results.Add(film);
                    break;
                }
            }
        }
        foreach(Serie serie in AllSerie){
            foreach (string token in tokens){
                string lowerToken = token.ToLower();
                if(
                    serie.Title.ToLower().Contains(lowerToken) ||
                    serie.Genres.Any(g => g.ToString().ToLower().Contains(lowerToken)) ||
                    serie.Directors.Any(d => d.ToLower().Contains(lowerToken))
                ){
                    results.Add(serie);
                    break;
                }
            }
        }
        results.Sort((x, y) => x.Rating.CompareTo(y.Rating));
        return results;
    }
}