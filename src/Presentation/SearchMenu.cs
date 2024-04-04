public static class SearchMenu
{
    public static List<Genre> SelectedFilters;
    public static void Start(string userinput = "", List<Genre> ListGenres = null)
    {
        while(Program.CurrentUser == null){
                MenuHelper.SelectOptions("Select an option", new Dictionary<string, Action>(){
                    {$"Input search: {userinput}", ()=>{
                        // run search logic
                        Console.WriteLine("Enter search: ");
                        string userinput = Console.ReadLine();
                        Start(userinput);
                    }},
                    {$"{SearchLogic.GenreToString(ListGenres)}", ()=>{
                        SearchLogic logic = new SearchLogic();
                        logic.SelectFilters();
                        SelectedFilters = logic.CheckSelectedFilters();
                        Start(userinput, SelectedFilters);
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
                            SearchResult = SearchLogic.Search(userinput, ListGenres);
                        }
                        foreach (Film film in SearchResult)
                        {
                            Console.WriteLine(film.Title);
                        }
                        
                    }},
                    {"Exit", ()=>{
                        // close application
                        Environment.Exit(1);
                    }},
                });
            } 
    }
    public static void Filters()
        {
        while(Program.CurrentUser == null){
                MenuHelper.SelectOptions("Filter by ", new Dictionary<string, Action>(){
                    {$"Genre", ()=>{
                        // run search logic
                        
                    }},
                    {"Director ETC ETC ETC ", ()=>{
                        // run search logic
                        
                    }},
                    {"Exit", ()=>{
                        // close application
                        Environment.Exit(1);
                    }},
                });
            } 
    }

}