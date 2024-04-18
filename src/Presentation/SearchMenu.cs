public static class SearchMenu
{
    public static List<Genre> SelectedFilters;
    public static string userinput = null;
    public static List<Genre> ListGenres = null;


    public static List<object> SelectSearch()
    {
        List<object> choice = new List<object>();
        bool running = true;
        while(running){
                MenuHelper.SelectOptions("Select an option", new Dictionary<string, Action>(){
                    {$"Search Movies", ()=>{
                        object film = SearchMovieMenu();
                        if(film != null)
                        {
                            choice.Add(film);
                        }
                    }},
                    {$"Search Series", ()=>{
                        List<Episode> episodes = SearchSeriesMenu();
                        if(episodes != null)
                        {
                            foreach(object episode in episodes)
                            {
                                choice.Add(episode);
                            }
                        }
                    }},
                    {$"Exit", ()=>{
                        running = false;
                    }},
                });
            }
        return choice;
    }
    public static Film SearchMovieMenu()
    {
        Film SelectedFilm = null;
        bool running = true;
        while(running){
                MenuHelper.SelectOptions("Select an option", new Dictionary<string, Action>(){
                    {$"Input search: {userinput}", ()=>{
                        // run search logic
                        Console.WriteLine("Enter search: ");
                        userinput = Console.ReadLine();
                    }}, //below outputs: Selected genres:
                    {$"{SearchLogic.GenreToString(ListGenres)}", ()=>{
                        SearchLogic logic = new SearchLogic();
                        logic.SelectMovieFilters();
                        SelectedFilters = logic.CheckSelectedFiltersMovie();
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
                            SelectedFilm = SelectMovie(SearchResult);
                            Console.WriteLine($"\nyou chose {SelectedFilm.Title}.");
                            Console.ReadKey();
                        }
                    }},
                    {"Go back", ()=>{
                        // close application
                        running = false;
                    }},
                });
            }
        return SelectedFilm;
    }
    public static List<Episode> SearchSeriesMenu()
    {
        List<Episode> EpisodeResult = new List<Episode>();
        bool running = true;
        while(running){
                MenuHelper.SelectOptions("Select an option", new Dictionary<string, Action>(){
                    {$"Input search: {userinput}", ()=>{
                        // run search logic
                        Console.WriteLine("Enter search: ");
                        userinput = Console.ReadLine();
                    }}, //below outputs: Selected genres: 
                    {$"{SearchLogic.GenreToString(ListGenres)}", ()=>{
                        SearchLogic logic = new SearchLogic();
                        logic.SelectSerieFilters();
                        SelectedFilters = logic.CheckSelectedFiltersSeries();
                    }},
                    {"Enter search", ()=>{
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
                    {"Go back", ()=>{
                        // close application
                        running = false;
                    }},
                });
            }
        return EpisodeResult;
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