using System.Text.RegularExpressions;

public class SearchAccess
{
    private FilmAcesser FilmAccessor = new FilmAcesser();
    private SerieAcesser SerieAccessor = new SerieAcesser();
    private List<Film> AllFilms = new List<Film>();
    private List<Serie> AllSeries = new List<Serie>();
    public SearchAccess(){
        AllFilms = FilmAccessor.Get_info();
        AllSeries = SerieAccessor.Get_info();
    }

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
                }else if(f.Genre.ToLower().Contains(token.ToLower())){
                    films.Add(f);
                    break;
                }
            }
        }
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
        films.Sort((x, y) => x.Rating.CompareTo(y.Rating));
        return films;
    }
}