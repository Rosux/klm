/// <summary>
/// This class holds methods for searching a specific movie/series
/// </summary>
public class SearchAccess
{
    private static FilmAcesser FilmAccessor = new FilmAcesser();
    private static SerieAcesser SerieAccessor = new SerieAcesser();
    private static List<Film> AllFilms = new List<Film>();
    private static List<Serie> AllSeries = new List<Serie>();
    public SearchAccess(){
        AllFilms = FilmAccessor.Get_info();
        AllSeries = SerieAccessor.Get_info();
    }

    public static void UpdateMedia(){
        AllFilms = FilmAccessor.Get_info();
        AllSeries = SerieAccessor.Get_info();
    }

    /// <summary>
    /// Search a movie/series by string seperated by ',' and return a list of Media.
    /// </summary>
    /// <param name="searchPattern">A string containing Title/Genre text optionally with seperator , between multiple search options.</param>
    /// <param name="filmOnly">A boolean when set to true only searches for Film and not series. When set to false searches for everything.</param>
    /// <returns>A list of Media objects holding every movie/serie where the title/genre contains the searchPattern.</returns>
    public List<Media> Search(string searchPattern, bool filmOnly=false){
        List<Media> films = new List<Media>();
        List<Serie> AllSerie = AllSeries;
        List<Film> AllMovies = AllFilms;
        string[] tokens = searchPattern.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries);
        foreach (Film f in AllMovies){
            foreach (string token in tokens){
                if(f.Title.ToLower().Contains(token.ToLower())){
                    films.Add(f);
                    break;
                }else{
                    bool stop = false;
                    foreach(string directorName in f.Directors){
                        if(directorName.ToLower().Contains(token.ToLower())){
                            films.Add(f);
                            stop = true;
                            break;
                        }
                    }
                    if(stop){break;}
                    foreach(string genre in f.Genres){
                        if(genre.ToLower().Contains(token.ToLower())){
                            films.Add(f);
                            break;
                        }
                    }
                }
            }
        }
        if(!filmOnly){
            foreach (Serie f in AllSerie){
                foreach (string token in tokens){
                    if(f.Title.ToLower().Contains(token.ToLower())){
                        films.Add(f);
                        break;
                    }else if(f.Genre.ToLower().Contains(token.ToLower())){
                        films.Add(f);
                        break;
                    }
                }
            }
        }
        films.Sort((x, y) => x.Rating.CompareTo(y.Rating));
        return films;
    }
}