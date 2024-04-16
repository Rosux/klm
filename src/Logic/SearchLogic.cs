using System.Linq;
using System.Text;
public class SearchLogic
{
    private static FilmAcesser Film = new FilmAcesser();
    private static SerieAcesser serieAcesser = new SerieAcesser();
    public static List<Genre> FilmGenres { get; private set; }
    public static  List<Genre> SeriesGenres { get; private set; }
    public List<Film> AllFilms { get; private set; }

    public SearchLogic()
    {
        SeriesGenres = serieAcesser.Get_Genres();
    }

    public static List<Film> SearchMovie(string Title, List<Genre> givenGenres)
    {
        List<Film> filteredFilms = FilmGenreSearch(givenGenres);
        if (!string.IsNullOrEmpty(Title))
        {
            // Prioritize matches at the beginning of the title
            filteredFilms = filteredFilms
                .OrderBy(film => film.Title.StartsWith(Title, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                .ThenBy(film => film.Title)
                .ToList();
        }
        return filteredFilms;
    }

    public static List<Film> FilmGenreSearch(List<Genre> givenGenres = null)
    {
        
        List<Film> allFilms = Film.Get_info();

        // see if there are given genres
        if (givenGenres == null || !givenGenres.Any(genre => genre.IsSelected))
        {
            return allFilms;
        }

        // Filter films based on selected genres using LINQ Where statement 
        var selectedGenres = givenGenres.Where(genre => genre.IsSelected).Select(genre => genre.Name);
        List<Film> filteredFilms = allFilms.Where(film => selectedGenres.Contains(film.Genre)).ToList();

        return filteredFilms;
    }
    public static List<Serie> SearchSeries(string Title, List<Genre> givenGenres)
    {
        List<Serie> filteredSeries = SeriesGenreSearch(givenGenres);
        if (!string.IsNullOrEmpty(Title))
        {
            // Prioritize matches at the beginning of the title
            filteredSeries = filteredSeries
                .OrderBy(serie => serie.Title.StartsWith(Title, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                .ThenBy(serie => serie.Title)
                .ToList();
        }
        return filteredSeries;
    }

    public static List<Serie> SeriesGenreSearch(List<Genre> givenGenres = null)
    {
        List<Serie> allSeries = serieAcesser.Get_info();

        if (givenGenres == null || !givenGenres.Any(genre => genre.IsSelected))
        {
            return allSeries;
        }

        var selectedGenres = givenGenres.Where(genre => genre.IsSelected).Select(genre => genre.Name);
        List<Serie> filteredSeries = allSeries.Where(serie => selectedGenres.Contains(serie.Genre)).ToList();

        return filteredSeries;

    }

    public static List<Genre> CheckSelectedFiltersMovie() //Displays the selected filters in SelectFilters()

    {
        Console.WriteLine("\nSelected genres:");
        List<Genre> SelectedGenres = new List<Genre>();
        foreach (Genre genre in FilmGenres)
        {
            if (genre.IsSelected)
            {
                Console.Write(genre.Name + ", ");
                SelectedGenres.Add(genre);
            }
        }
        Console.WriteLine("\n\nPress Enter to continue...");
        Console.ReadKey();
        return SelectedGenres;
    }

    public static List<Genre> CheckSelectedFiltersSeries() //Displays the selected filters in SelectFilters()

    {
        Console.WriteLine("\nSelected genres:");
        List<Genre> SelectedGenres = new List<Genre>();
        foreach (Genre genre in SeriesGenres)
        {
            if (genre.IsSelected)
            {
                Console.Write(genre.Name + ", ");
                SelectedGenres.Add(genre);
            }
        }
        Console.WriteLine("\n\nPress Enter to continue...");
        Console.ReadKey();
        return SelectedGenres;
    }


    public static string GenreToString(List<Genre> genrelist) //Genre list to string - used to display selected genre in search menu
    {
        System.Text.StringBuilder genres = new System.Text.StringBuilder("Change filters: ");
        if (genrelist == null)
        {
            return "Select Filters";
        }
        else
        {
            foreach (Genre genre in genrelist)
            {
                genres.Append(genre.Name);
            }
            return genres.ToString();
        }
    }


    public void SelectMovieFilters() //Select genre filter for movie
    {
        Console.WriteLine("All genres: ");
        Console.WriteLine("Select genres by entering their index (starting from 1) and press Enter. Enter 0 to finish selection.");
        for (int i = 0; i < FilmGenres.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {FilmGenres[i].Name}");
        }
        Console.WriteLine();
        while (true)
        {
            Console.Write("Enter the index of the genre to select (or 0 to finish): ");
            string input = Console.ReadLine();
            int index;
            if (int.TryParse(input, out index))
            {
                if (index == 0)
                    break;

                if (index >= 1 && index <= FilmGenres.Count)
                {
                    FilmGenres[index - 1].IsSelected = true;
                }
                else
                {
                    Console.WriteLine("Invalid index. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }

    }

    public void SelectSerieFilters() //Select genre filter for series
    {
        Console.WriteLine("All genres: ");
        Console.WriteLine("Select genres by entering their index (starting from 1) and press Enter. Enter 0 to finish selection.");
        for (int i = 0; i < SeriesGenres.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {SeriesGenres[i].Name}");
        }
        Console.WriteLine();
        while (true)
        {
            Console.Write("Enter the index of the genre to select (or 0 to finish): ");
            string input = Console.ReadLine();
            int index;
            if (int.TryParse(input, out index))
            {
                if (index == 0)
                    break;

                if (index >= 1 && index <= SeriesGenres.Count)
                {
                    SeriesGenres[index - 1].IsSelected = true;
                }
                else
                {
                    Console.WriteLine("Invalid index. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }

    }
    public static List<Genre> Get_Genres(bool IsMovie)
    {
        if (IsMovie == true)
        {
            FilmGenres = Film.Get_Genres();
            return FilmGenres;
        }
        else if (IsMovie == false)
        {
            SeriesGenres = serieAcesser.Get_Genres();
            return SeriesGenres;
        }
        else return null;
    }
}