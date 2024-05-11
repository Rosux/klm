using Newtonsoft.Json;
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
                {"1. All movies", ()=>{
                    //views all movies.
                    movie_info(filmlogic_obj);
                    Console.Write($"\n\nPress any key to continue...");
                    Console.ReadKey();
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
        if(filmlogic_obj.Check_film())
        {
            Console.WriteLine(filmlogic_obj.info());
        }
        else
        {
            Console.WriteLine("There are no films.");
        }
    }
    private static void movie_add(FilmLogic filmlogic_obj)
    {
        int id = 0;

        Console.WriteLine("Input title: ");
        string Title = Console.ReadLine();
        if (string.IsNullOrEmpty(Title))
        {
            Title = "No title given";
        }

        List<string> givengenres = GetGenres();

        Console.WriteLine("Original language: ");
        string OriginalLanguage = Console.ReadLine();
        if(OriginalLanguage == "" || OriginalLanguage == null)
        {
            OriginalLanguage = "English";
        }
        
        Console.WriteLine("Overview (movie plot)");
        string Overview = Console.ReadLine();
        if (Overview == "" || Overview == null)
        {
            Overview = "No movie plot";
        }
        DateOnly x = MenuHelper.SelectDate("Select release date");
        string ReleaseDate = x.ToString("yyyy-MM-dd");

        int runtime = getDuration();

        double vote_average = getVoteAverage();

        Console.WriteLine("Certification (PG)");
        string certification = Console.ReadLine();
        
        Console.WriteLine("Director");
        string director = Console.ReadLine();
        // Create a dictionary to store director information
        Dictionary<string, string> directorInfo = new Dictionary<string, string>();
        directorInfo.Add("-1", director);
        // add dict to list
        List<Dictionary<string, string>> directorsList = new List<Dictionary<string, string>>();
        directorsList.Add(directorInfo);
        id = filmlogic_obj.CreateID();
        Console.WriteLine(filmlogic_obj.Add_film(new Film(id, givengenres, OriginalLanguage, Overview, ReleaseDate, runtime, Title, vote_average, certification, directorsList)));
    }

    private static void movie_remove(FilmLogic filmlogic_obj)
    {
        if(filmlogic_obj.Check_films_exist())
        {
            Console.WriteLine(filmlogic_obj.info());
            Console.WriteLine("Id: ");
            if (int.TryParse(Console.ReadLine(), out int film_id))
            {
                if(filmlogic_obj.Check_film(film_id))
                {
                    Console.WriteLine(filmlogic_obj.Remove_film(film_id));
                }
                else
                {
                    Console.WriteLine($"film with id: {film_id} does not exist, try again.");
                }
            }
            else
            {
                Console.WriteLine("Id has to be an interger, try again.");
                Console.WriteLine();
                movie_remove(filmlogic_obj);
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
            Console.WriteLine("Search for a movie you want to change");
            Film SelectedFilm = MenuHelper.SelectMovie();
            int film_id = SelectedFilm.Id;
            if (SelectedFilm != null)
            {
                MenuHelper.SelectOptions("Choose what part you want to change", new Dictionary<string, Action>(){
                {"Genres", ()=>{
                    List<string> GivenGenres = GetGenres();
                    Console.WriteLine(filmlogic_obj.change_genre(film_id, GivenGenres));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {"Original language", ()=>{
                    Console.WriteLine("");
                    Console.Write("Original Language: ");
                    string givenlanguage = Console.ReadLine();
                    if(givenlanguage == "" || givenlanguage == null)
                    {
                        givenlanguage = "English";
                        Console.WriteLine("Invalid input, language changed to: English ");
                        string a = filmlogic_obj.change_language(film_id, givenlanguage);
                        Console.WriteLine("Press enter to continue... ");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine(filmlogic_obj.change_language(film_id, givenlanguage));
                        Console.WriteLine("Press enter to continue... ");
                        Console.ReadKey();
                    }
                }},
                {"Overview (plot)", ()=>{
                    Console.WriteLine("New overview (plot): ");
                    string Overview = Console.ReadLine();
                    if (Overview == "" || Overview == null)
                    {
                        Overview = "No movie plot";
                    }
                    Console.WriteLine(filmlogic_obj.change_overview(film_id, Overview));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {"Release date", ()=>{
                    DateOnly x = MenuHelper.SelectDate("Select new release date");
                    string ReleaseDate = x.ToString("yyyy-MM-dd");
                    Console.WriteLine(filmlogic_obj.change_releasedate(film_id, ReleaseDate));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {"Runtime", ()=>{
                    int i = getDuration();
                    Console.WriteLine(filmlogic_obj.change_duration(film_id, i));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {"Title", ()=>{
                    Console.WriteLine("New title: ");
                    string new_title = Console.ReadLine();
                    Console.WriteLine(filmlogic_obj.change_title(film_id, new_title));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {"Vote average", ()=>{
                    double d = getVoteAverage();
                    Console.WriteLine(filmlogic_obj.change_vote(film_id, d));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {"Certification", ()=>{
                    Console.WriteLine("Certification (PG)");
                    string certification = Console.ReadLine();
                    Console.WriteLine(filmlogic_obj.change_certification(film_id, certification));
                    Console.WriteLine("Press enter to continue... ");
                    Console.ReadKey();
                }},
                {"Director", ()=>{
                    Console.WriteLine("Director");
                    string director = Console.ReadLine();
                    // Create a dictionary to store director information
                    Dictionary<string, string> directorInfo = new Dictionary<string, string>();
                    directorInfo.Add("-1", director);
                    // add dict to list
                    List<Dictionary<string, string>> directorsList = new List<Dictionary<string, string>>();
                    directorsList.Add(directorInfo);
                    Console.WriteLine(filmlogic_obj.change_director(film_id, directorsList));
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

        Console.WriteLine("Enter genres (Enter to submit)");

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
    private static int getDuration()
    {
        Console.WriteLine("Input Duration (min)");
        string durationString = Console.ReadLine();
        int duration;
        if (!int.TryParse(durationString, out duration))
        {
            Console.WriteLine("Invalid input. Duration set to 0.");
            duration = 0;
        }
        return duration;
    }
    private static double getVoteAverage()
    {
        Console.WriteLine("Rating (/10)");
        string voteAverageString = Console.ReadLine();
        double voteAverage;
        if (string.IsNullOrEmpty(voteAverageString))
        {
            return voteAverage = 0;
        }
        if (!double.TryParse(voteAverageString, out voteAverage))
        {
            Console.WriteLine("Invalid input. Vote average set to 0.");
            voteAverage = 0;
        }
        return voteAverage;
    }
}
