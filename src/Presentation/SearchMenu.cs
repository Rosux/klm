public static class SearchMenu
{
    public static void Start(string userinput = "")
    {
        while(Program.CurrentUser == null){
                MenuHelper.SelectOptions("Select an option", new Dictionary<string, Action>(){
                    {$"Input search: {userinput}", ()=>{
                        // run search logic
                        Console.WriteLine("Enter search: ");
                        string userinput = Console.ReadLine();
                        Start(userinput);
                    }},
                    {"Filters", ()=>{
                        SearchLogic logic = new SearchLogic();
                        logic.SelectFilters();
                        logic.CheckSelectedFilters();
                    }},
                    {"Search", ()=>{
                        // close application
                        
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