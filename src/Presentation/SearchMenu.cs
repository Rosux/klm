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
                            foreach (Film film in SearchResult)
                            {
                                Console.WriteLine(film.Title);
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
                        List<Serie> SearchResult = new List<Serie>();
                        if (userinput == null && ListGenres == null)
                        {
                            Console.WriteLine("Please fill in a search query\n\nPress Enter to continue...");
                            Console.ReadKey();
                        }
                        else
                        {
                            SearchResult = SearchLogic.SearchSeries(userinput, ListGenres);
                            foreach (Serie serie in SearchResult)
                            {
                                Console.WriteLine(serie.Title);
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

}