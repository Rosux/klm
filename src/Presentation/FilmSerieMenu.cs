using Newtonsoft.Json;
using System.Threading;
class FilmSerieMenu
{
    public static void UI()
    {
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. Manage Movies", ()=>{
                    //takes admin to movies menu.
                    movie_options();
                }},
                {"2. Manage series", ()=>{
                    //takes admin to series menu.
                    serie_options();
                }},
                {"3. Exit to main menu", ()=>{
                    running = false;
                }},
            });
        }
    }
//--------------------------------------------------------------------------------------------------------------
    private static void serie_options()
    {
        SerieLogic serie_obj = new SerieLogic();
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. All series", ()=>{
                    //views all series.
                    serie_info(serie_obj);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"2. Add serie", ()=>{
                    //makes admin be able to add serie.
                    serie_add(serie_obj);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"3. Add season", ()=>{
                    //makes admin be able to add season.
                    season_add(serie_obj);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"4. Add episode", ()=>{
                    //makes admin be able to add episode.
                    episode_add(serie_obj);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"5. Remove serie", ()=>{
                    //makes admin be able to remove serie.
                    serie_remove(serie_obj);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"6. Go back", ()=>{
                    running = false;
                }},
            });
        }
    }
    private static void movie_options()
    {
        FilmLogic filmlogic_obj = new FilmLogic();
        bool running = true;
        while(running)
        {
            MenuHelper.SelectOptions("Choose an option", new Dictionary<string, Action>(){
                {"1. Search all movies", ()=>{
                    //Search all movies and display information.
                    movie_info(filmlogic_obj);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey(true);
                }},
                {"2. Add movie", ()=>{
                    //makes admin be able to add movie.
                    movie_add(filmlogic_obj);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"3. Remove movie", ()=>{
                    //makes admin be able to remove movie.
                    movie_remove(filmlogic_obj);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"4. Change movie", ()=>{
                    //makes admin be able to change movie.
                    movie_change(filmlogic_obj);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
                }},
                {"5. Go back", ()=>{
                    running = false;
                }},
            });
        }
    }
//-----------------------------------------------------------------------------------------------------------------  
    private static void serie_remove(SerieLogic serie_obj)
    {
        if (serie_obj.Check_Series_exist())
        {
            Console.WriteLine(serie_obj.All_id());
            Console.WriteLine("Id: ");
            if(int.TryParse(Console.ReadLine(), out int serie_id))
            {
                if (serie_obj.Check_Serie(serie_id))
                {
                    Console.WriteLine(serie_obj.Remove_serie(serie_id));
                }
                else
                {
                    Console.WriteLine($"Serie with id: {serie_id} does not exist.");
                }
            }
            else
            {
                Console.WriteLine("please enter a valid interger, try again");
                serie_remove(serie_obj);
            }
        }
        else
        {
            Console.WriteLine("There are no series to remove");
        }
    }

    private static void episode_add(SerieLogic serie_obj)
    {
        Console.WriteLine(serie_obj.All_id());
        if (serie_obj.Check_Series_exist())
        {
            SeasonLogic season_obj = new SeasonLogic();
                        
            Console.WriteLine("Serie Id: ");
            if (int.TryParse(Console.ReadLine(), out int serie_id))
            {

                if (serie_obj.Check_Serie(serie_id))
                {

                    Console.WriteLine(season_obj.All_Seasons(serie_id));

                    Console.WriteLine("Season to add episode: ");
                    if (int.TryParse(Console.ReadLine(), out int season_id))
                    {
                            
                        if (season_obj.Check_Seasons(serie_id, season_id))
                        {
                            Console.WriteLine("Episode title: ");
                            string episode_title = Console.ReadLine();
                    
                            Console.WriteLine("Episode duration(min): ");
                            if (int.TryParse(Console.ReadLine(), out int episode_duration))
                            {
                    
                                Episode new_episode = new Episode(episode_title, episode_duration);
                                Console.WriteLine(season_obj.Add_Episode(new_episode, serie_id, season_id));
                            }
                            else
                            {
                                Console.WriteLine("please enter a valid interger, try again");
                                episode_add(serie_obj);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"This serie does not contain Season {season_id}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("please enter a valid interger, try again");
                        episode_add(serie_obj);
                    }
                }
                else
                {
                    Console.WriteLine($"Serie with id: {serie_id} does not exist.");
                }       
            }
            else
            {
                Console.WriteLine("please enter a valid interger, try again");
                episode_add(serie_obj);
            }
        }
        else
        {
            Console.WriteLine("There are no series.");
        }
    }

    private static void season_add(SerieLogic serie_obj)
    {
        if (serie_obj.Check_Series_exist())
        {

            Console.WriteLine(serie_obj.All_id());

            Console.WriteLine("Serie Id: ");
            if(int.TryParse(Console.ReadLine(), out int serie_id))
            {
                Console.WriteLine(serie_obj.Add_Season(serie_id));
            }
            else
            {
                Console.WriteLine("please enter a valid interger.");
                season_add(serie_obj);
            }
        }
        else
        {
            Console.WriteLine("There are no series to add a season to.");
        }
    }
    private static void serie_info(SerieLogic serie_obj)
    {
        if (serie_obj.Check_Series_exist())
        {
            Console.WriteLine(serie_obj.info());
        }
        else
        {
            Console.WriteLine("There are no series.");
        }
    }

    private static void serie_add(SerieLogic serie_obj)
    {
        Console.WriteLine("Serie title: ");
        string serie_title = Console.ReadLine();
                
        Console.WriteLine("Serie genre: ");
        string serie_genre = Console.ReadLine();
        
        Console.WriteLine(serie_obj.Add_Serie(new Serie(serie_title, serie_genre)));
    }
// --------------------------------------------------------------------------------------------------------------
    private static void movie_info(FilmLogic filmlogic_obj)
    {
        if(filmlogic_obj.Check_films_exist())
        {
            //Select film to display 
            Film d = MenuHelper.SelectMovie();
            if (d != null)
            {   
                string Selectedmoviegenres = String.Join(", ", d.Genres);
                string Selecteddirectors = String.Join(", ", d.Directors);
                Console.WriteLine($"All movie info:\nId: {d.Id}\nGenres: {Selectedmoviegenres}\nOriginal Language: {d.OriginalLanguage}\nOverview: {d.Overview}\nRelease date: {d.ReleaseDate} \nRuntime: {d.Runtime}\nTitle: {d.Title}\nVote average: {d.VoteAverage}\nCertification: {d.Certification}\nDirectors: {Selecteddirectors}");
            }
            else
            {
                return;
            }
        }
        else
        {
            Console.WriteLine("There are no films to remove.");
        }
    }
    private static void movie_add(FilmLogic filmlogic_obj)
    {
        //List which keeps track of selected / non selected movie parts
        List<string> Selected = new List<string>() { "X", "X", "X", "X", "X", "X", "X", "X", "X"};

        List<string> Genres = new List<string>(); // Film parts
        string? Language = "";
        string Overview = "";
        string OverviewDisplay = "";
        string Releasedate = "";
        int Runtime = 0;
        string Title = "";
        string TitleDisplay = "";
        double Voteaverage = 0;
        string Certification = "";
        List<string> Directors = new List<string>();
        string Selectedmoviegenres = "";
        string Selecteddirectors = "";

        int AddLoop = 1;
        while(AddLoop == 1)
        {
            if(filmlogic_obj.Check_films_exist())
            {
                MenuHelper.SelectOptions("Choose what part you want to change", new Dictionary<string, Action>(){
                {$"Genres {Selected[0]} {Selectedmoviegenres}", ()=>{
                    Genres = GetGenres();
                    if (Genres == null)
                    {
                        Console.WriteLine("No genre given. Enter to continue");
                        Selected[0] = "X";
                    }
                    else
                    {
                        Selectedmoviegenres = String.Join(", ", Genres);
                        Selected[0] = "✓";
                        Console.WriteLine("Genres successfully added to selection.\nPress enter to continue... ");
                        Console.ReadKey();
                    }
                }},
                {$"Original language {Selected[1]} {Language}", ()=>{
                    Language = getLanguage();
                    if (Language == null)
                    {

                    }
                    else
                    {
                        Selected[1] = "✓";
                        Console.WriteLine($"Language successfully set to: {Language} \nPress enter to continue... ");
                        Console.ReadKey();
                    }
                }},
                {$"Overview (plot) {Selected[2]} {OverviewDisplay}", ()=>{
                    Console.WriteLine("Overview (plot): ");
                    Overview = Console.ReadLine();
                    if (Overview == "" || Overview == null)
                    {
                        Overview = "No movie plot";
                    }
                    Selected[2] = "✓";
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                    OverviewDisplay = GetDisplayString(Overview);
                    if (Overview.Length > Console.WindowWidth)
                    {
                        OverviewDisplay = $"{Overview.Substring(0, (Console.WindowWidth - 25))}...";
                    }
                    else
                    {
                        OverviewDisplay = Overview;
                    }
                }},
                {$"Release date {Selected[3]} {Releasedate}", ()=>{
                    DateOnly x = MenuHelper.SelectDate("Select a release date");
                    Releasedate = x.ToString("yyyy-MM-dd");
                    Selected[3] = "✓";
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {$"Runtime {Selected[4]} {Runtime}", ()=>{
                    int? i = getDuration();
                    if (i == null)
                    {
                        Console.WriteLine("No duration selected");
                        Selected[4] = "X";
                        return;
                    }
                    else
                    {
                        Runtime = (int)i;
                        Selected[4] = "✓";
                        Console.WriteLine("Runtime successfully selected.\nPress enter to continue... ");
                        Console.ReadKey();
                    }

                }},
                {$"Title {Selected[5]} {TitleDisplay}", ()=>{
                    Console.WriteLine("Enter title: ");
                    Title = Console.ReadLine();
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                    TitleDisplay = GetDisplayString(Title);
                    Selected[5] = "✓";
                }},
                {$"Vote average {Selected[6]} {Voteaverage}", ()=>{
                    Voteaverage = (double)getVoteAverage();
                    if (Voteaverage == null)
                    {
                        
                    }
                    else
                    {
                        Console.WriteLine("Press enter to continue... ");
                        Console.ReadKey();
                        Selected[6] = "✓";
                    }
                }},
                {$"Certification {Selected[7]} {Certification}", ()=>{
                    Console.WriteLine("Certification (PG)");
                    Certification = Console.ReadLine();
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                    Selected[7] = "✓";
                }},
                {$"Director {Selected[8]} {Selecteddirectors}", ()=>{
                    Console.WriteLine("Director");
                    string director = Console.ReadLine();
                    // Create a list to store director information
                    Directors.Add(director);
                    Selecteddirectors = String.Join(", ", Directors);
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                    Selected[8] = "✓";
                }},
                {"Exit", ()=>{
                    AddLoop = 0;
                }},
                {"Save", ()=>{
                    if (Selected.Contains("X"))
                    {
                        Console.WriteLine("You have not yet added all elements\nPress enter to continue editing.");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        // use string.join to display all contents of the lists
                        string Selectedmoviegenres = String.Join(", ", Genres);
                        string Selecteddirectors = String.Join(", ", Directors);
                        bool ConfirmAdd = MenuHelper.Confirm($"Genres: {Selectedmoviegenres}\nLanguage: {Language}\nOverview: {Overview}\n\nRelease date: {Releasedate}\nRuntime: {Runtime}\nTitle: {Title}\nVote average (/10): {Voteaverage}\nCertification: {Certification}\nDirectors: {Selecteddirectors}\n Are you sure you want to add this movie?");
                        if (ConfirmAdd)
                        {
                            int id = filmlogic_obj.CreateID();
                            Console.WriteLine(filmlogic_obj.Add_film(new Film(id, Genres, Language, Overview, Releasedate, (int)Runtime, Title, Voteaverage, Certification, Directors)));
                            Thread.Sleep(5000);
                            AddLoop = 0;
                        }
                        if (!ConfirmAdd)
                        {
                            MenuHelper.SelectOptions("Select an action", new Dictionary<string, Action>(){
                                {$"Keep editing movie", ()=>{

                                }},
                                {$"Exit without saving", ()=>{
                                    AddLoop = 0;
                                }},
                            });
                        }
                    }
                }},
            });
            }
        }
    }

    private static void movie_remove(FilmLogic filmlogic_obj)
    {
        if(filmlogic_obj.Check_films_exist())
        {
            Film? ToRemove = MenuHelper.SelectMovie();
            if(ToRemove == null){return;}
            bool a = MenuHelper.Confirm("Are you sure you want to remove this movie?");
            if (a)
            {   
                filmlogic_obj.Remove_film(ToRemove.Id);
            }
            else
            {
                return;
            }
        }
        else
        {
            Console.WriteLine("There are no films to remove.");
        }
    }
    private static void movie_change(FilmLogic filmlogic_obj)
    {
        if(filmlogic_obj.Check_films_exist())
        {
            Film? SelectedFilm = MenuHelper.SelectMovie();
            Console.WriteLine("Search for a movie you want to change");
            string Selectedmoviegenres = String.Join(", ", SelectedFilm.Genres);
            string Selecteddirectors = String.Join(", ", SelectedFilm.Directors);
            string OverviewDisplay = GetDisplayString(SelectedFilm.Overview);
            if(SelectedFilm == null){return;}
            int film_id = SelectedFilm.Id;
            if (SelectedFilm != null)
            {
                MenuHelper.SelectOptions("Choose what part you want to change", new Dictionary<string, Action>(){
                {$"Genres: {Selectedmoviegenres}", ()=>{
                    List<string> GivenGenres = GetGenres();
                    Selectedmoviegenres = String.Join(", ", GivenGenres);
                    Console.WriteLine(filmlogic_obj.change_genre(film_id, GivenGenres));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {$"Original language: {SelectedFilm.OriginalLanguage}", ()=>{
                    string givenlanguage = getLanguage();
                    Console.WriteLine(filmlogic_obj.change_language(film_id, givenlanguage));
                    Console.WriteLine($"Language succesfully changed to {givenlanguage}\nPress enter to continue...");
                    Console.ReadKey();
                }},
                {$"Overview (plot): {OverviewDisplay}", ()=>{
                    Console.WriteLine($"------------------------\nOld overview: {SelectedFilm.Overview}\n------------------------");
                    Console.WriteLine("\nNew overview (plot): ");
                    string Overview = Console.ReadLine();
                    if (Overview == "" || Overview == null)
                    {
                        Overview = "No movie plot";
                    }
                    Console.WriteLine(filmlogic_obj.change_overview(film_id, Overview));
                    OverviewDisplay = GetDisplayString(Overview);
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {$"Release date: {SelectedFilm.ReleaseDate}", ()=>{
                    DateOnly x = MenuHelper.SelectDate("Select new release date");
                    string Releasedate = x.ToString("yyyy-MM-dd");
                    Console.WriteLine(filmlogic_obj.change_releasedate(film_id, Releasedate));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {$"Runtime: {SelectedFilm.Runtime}", ()=>{
                    int? i = getDuration();
                    if (i == null)
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine(filmlogic_obj.change_duration(film_id, (int)i));
                        Console.WriteLine("Press enter to continue... ");
                        Console.ReadKey();
                    }

                }},
                {$"Title: {SelectedFilm.Title}", ()=>{
                    Console.WriteLine("New title: ");
                    string new_title = Console.ReadLine();
                    Console.WriteLine(filmlogic_obj.change_title(film_id, new_title));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {$"Vote average: {SelectedFilm.VoteAverage}", ()=>{
                    double d = (double)getVoteAverage();
                    Console.WriteLine(filmlogic_obj.change_vote(film_id, d));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {$"Certification: {SelectedFilm.Certification}", ()=>{
                    Console.WriteLine("Certification (PG)");
                    string certification = Console.ReadLine();
                    Console.WriteLine(filmlogic_obj.change_certification(film_id, certification));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {$"Director: {Selecteddirectors}", ()=>{
                    Console.WriteLine("Director");
                    string director = Console.ReadLine();
                    // Create a list to store director information
                    List<string> directorInfo = new List<string>();
                    directorInfo.Add(director);
                    Console.WriteLine(filmlogic_obj.change_director(film_id, directorInfo));
                    Selecteddirectors = String.Join(", ", directorInfo);
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {"Exit", ()=>{
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
            });
            }
        }
        else
        {
            Console.WriteLine("There are no films to change.");
        }
    }

    private static List<string> GetGenres()
    {
        List<string> givenGenres = new List<string>();

        Console.WriteLine("Enter (multiple) genres. \nPress ENTER after every entry.\nENTER on empty entry to continue.");

        while (true)
        {
            string givenGenre = Console.ReadLine();

            if (givenGenre == "") // If the user presses Enter without typing anything
                break;

            givenGenres.Add(givenGenre);
        }

        if (givenGenres.Count == 0)
        {
            givenGenres.Add("No genre");
        }

        return givenGenres;
    }
    // returns true if 
    private static int? getDuration()
    {
        return MenuHelper.SelectInteger("Select a duration ", "Minutes", true, 1, 0);
    }
    private static double? getVoteAverage()
    {
        return MenuHelper.SelectPrice("Select a vote average /10", "", true, 0.0, 10.0);
    }

    private static string? getLanguage()
    {
        return  MenuHelper.SelectText("Select a language", "", true, 0, int.MaxValue, "([A-z]| )");
    }


    //Checks the current windowWidth and caps 
    //the given string to this to not break menuhelper
    public static string GetDisplayString(string givenstring)
    {
        string returnstring;
        if (givenstring.Length > Console.WindowWidth)
        {
            returnstring = $"{givenstring.Substring(0, (Console.WindowWidth - 25))}...";
        }
        else
        {
            returnstring = givenstring;
        }
        return returnstring;
    }
}
