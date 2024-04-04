using System.Linq;
using System.Text;
public class SearchLogic
{
    private static FilmAcesser Film = new FilmAcesser();
    public List<Genre> Genres { get; private set; }
    public List<Film> AllFilms { get; private set; }

    public SearchLogic()
    {
        Genres =  Film.Get_Genres();
    }

    public static List<Film> Search(string Title, List<Genre> givenGenres)
    {
        List<Film> filteredFilms = GenreSearch(givenGenres);
        if (!string.IsNullOrEmpty(Title))
        {
            filteredFilms = filteredFilms.OrderBy(film => film.Title.IndexOf(Title, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return filteredFilms;
    }

    public static List<Film> GenreSearch(List<Genre> givenGenres = null)
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

    public List<Genre> CheckSelectedFilters()
    {
        Console.WriteLine("\nSelected genres:");
        List<Genre> SelectedGenres = new List<Genre>();
        foreach (Genre genre in Genres)
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


    public static string GenreToString(List<Genre> genrelist)
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


    public void SelectFilters()
    {
        Console.WriteLine("All genres: ");
        Console.WriteLine("Select genres by entering their index (starting from 1) and press Enter. Enter 0 to finish selection.");
        for (int i = 0; i < Genres.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Genres[i].Name}");
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

                if (index >= 1 && index <= Genres.Count)
                {
                    Genres[index - 1].IsSelected = true;
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
}