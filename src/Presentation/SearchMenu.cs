public static class SearchMenu
{
    public static List<Genre> SelectedFilters;


    public static void SelectSearch()
    {
        while(Program.CurrentUser == null){
                MenuHelper.SelectOptions("Select an option", new Dictionary<string, Action>(){
                    {$"Search Movies", ()=>{
                        SearchMovieMenu();
                    }},
                    {$"Search Series", ()=>{
                        SearchSeriesMenu();
                    }},
                });
            }
    }
    public static void SearchMovieMenu(string userinput = null, List<Genre> ListGenres = null)
    {
        while(Program.CurrentUser == null){
                MenuHelper.SelectOptions("Select an option", new Dictionary<string, Action>(){
                    {$"Input search: {userinput}", ()=>{
                        // run search logic
                        Console.WriteLine("Enter search: ");
                        string userinput = Console.ReadLine();
                        SearchMovieMenu(userinput, SelectedFilters);
                    }}, //below outputs: Selected genres: 
                    {$"{SearchLogic.GenreToString(ListGenres)}", ()=>{
                        SearchLogic logic = new SearchLogic();
                        logic.SelectMovieFilters();
                        SelectedFilters = logic.CheckSelectedFiltersMovie();
                        SearchMovieMenu(userinput, SelectedFilters);
                    }},
                    {"Enter search", ()=>{
                        List<Film> SearchResult = new List<Film>();
                        if (userinput == null && ListGenres == null)
                        {
                            Console.WriteLine("Please fill in a search query\n\nPress Enter to continue...");
                            Console.ReadKey();
                        }
                        else
                        {
                            SearchResult = SearchLogic.SearchMovie(userinput, ListGenres);
                            Film SelectedFilm = SelectMovie(SearchResult);
                            Console.WriteLine(SelectedFilm.Title);
                            Console.ReadKey();
                        }
                        
                    }},
                    {"Exit", ()=>{
                        // close application
                        Environment.Exit(1);
                    }},
                });
            } 
    }
    public static void SearchSeriesMenu(string userinput = null, List<Genre> ListGenres = null)
    {
        while(Program.CurrentUser == null){
                MenuHelper.SelectOptions("Select an option", new Dictionary<string, Action>(){
                    {$"Input search: {userinput}", ()=>{
                        // run search logic
                        Console.WriteLine("Enter search: ");
                        string userinput = Console.ReadLine();
                        SearchSeriesMenu(userinput, SelectedFilters);
                    }}, //below outputs: Selected genres: 
                    {$"{SearchLogic.GenreToString(ListGenres)}", ()=>{
                        SearchLogic logic = new SearchLogic();
                        logic.SelectSerieFilters();
                        SelectedFilters = logic.CheckSelectedFiltersSeries();
                        SearchSeriesMenu(userinput, SelectedFilters);
                    }},
                    {"Enter search", ()=>{
                        List<Episode> EpisodeResult = new List<Episode>();
                        List<Serie> SearchResult = new List<Serie>();
                        if (userinput == null && ListGenres == null)
                        {
                            Console.WriteLine("Please fill in a search query\n\nPress Enter to continue...");
                            Console.ReadKey();
                        }
                        else
                        {
                            SearchResult = SearchLogic.SearchSeries(userinput, ListGenres);
                            EpisodeResult = SearchEpisodes(SearchResult);
                            Console.WriteLine("Selected Episodes: ");
                            int i = 1;
                            foreach (Episode episode in EpisodeResult)
                            {
                                Console.WriteLine($"{i}. {episode.Title}");
                                i++;
                            }
                            Console.ReadKey();
                        }
                        
                    }},
                    {"Exit", ()=>{
                        // close application
                        Environment.Exit(1);
                    }},
                });
            } 
    }



    public static Film SelectMovie(List<Film> SearchedMovies)
    {
        Dictionary<string, Film> filmOptions = SearchedMovies.ToDictionary(film => film.Title, film => film);
        Film selectedFilm = MenuHelper.SelectFromList("Select a movie", filmOptions);
        return selectedFilm;
    }
    public static List<Episode> SearchEpisodes(List<Serie> SearchedSeries)
    {
        List<Episode> SelectedEpisodes = new List<Episode>();
        List<Season> SelectedSeries = new List<Season>();
        Console.WriteLine("Select a series by giving its index");
        for (int i = 0; i < SearchedSeries.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {SearchedSeries[i].Title}");
        }
        Console.Write("Select series: ");
        string _selectedSeries = Console.ReadLine();
        int index;
        if (int.TryParse(_selectedSeries, out index))
        {
            if (index == 0)
            {
                Environment.Exit(1);
            }
            if (index >= 1 && index <= SearchedSeries.Count)
            {
                var serie = SearchedSeries[index - 1];
                for (int i = 0; i < serie.Seasons.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {serie.Seasons[i].Title}");
                }
                Console.Write("\nSelect Season: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out index))
                {
                    if (index == 0)
                    {
                        Environment.Exit(1);
                    }
                    if (index >= 1 && index <= serie.Seasons.Count)
                    {
                        var season = serie.Seasons[index - 1];
                        for (int i = 0; i < season.Episodes.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {season.Episodes[i].Title}");
                        }
                        while (true)
                        {
                            Console.Write("\nSelect episodes: (Select 0 to quit)");
                            input = Console.ReadLine();
                            if (int.TryParse(input, out index))
                            {
                                if (index == 0 )
                                {
                                    break;
                                }
                                if (index >= 1 && index <= season.Episodes.Count)
                                {
                                    SelectedEpisodes.Add(season.Episodes[index - 1]);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid index. Please try again.");
                                }
                            } 
                        }
                        return SelectedEpisodes;
                    }
                }
                
            }
            else
            {
                Console.WriteLine("Invalid index");
            }
        }
        return SelectedEpisodes;
    }
}