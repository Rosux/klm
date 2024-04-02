public class SearchLogic
{
    FilmAcesser Film = new FilmAcesser();
    public List<Genre> Genres { get; private set; }
    public SearchLogic()
    {
        Genres =  Film.Get_Genres();
    }

    public static void Search()
    {

    }

    public void CheckSelectedFilters()
    {
        Console.WriteLine("\nSelected genres:");
        foreach (var genre in Genres)
        {
            if (genre.IsSelected)
            {
                Console.Write(genre.Name + ", ");
            }
        }
        Console.WriteLine("\n\nPress Enter to continue...");
        Console.ReadKey();
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