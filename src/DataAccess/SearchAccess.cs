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
        List<Media> films = new List<Media>();
        List<Serie> AllSerie = AllSeries;
        List<Film> AllMovies = AllFilms;
        string[] tokens = searchPattern.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries);
        foreach (Film f in AllMovies){
            foreach (string token in tokens){
                if(f.Title.ToLower().Contains(token.ToLower())){
                    films.Add(f);
                    break;
                }
            }
        }
        foreach(Serie s in AllSerie){
            foreach (string token in tokens){
                if(s.Title.ToLower().Contains(token.ToLower())){
                    films.Add(s);
                    break;
                }
            }
        }
        films.Sort((x, y) => x.Rating.CompareTo(y.Rating));
        return films;
    }
}