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
        if(filmlogic_obj.Check_films_exist())
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
        Console.WriteLine("Title: ");
        string title = Console.ReadLine();
        Console.WriteLine("Genre: ");
        string genre = Console.ReadLine();
        Console.WriteLine("Duration(min): ");
        if (int.TryParse(Console.ReadLine(), out int duration))
        {
            Console.WriteLine(filmlogic_obj.Add_film(new Film(title, genre, duration)));
        }
        else
        {
            Console.WriteLine("duration has to be an interger, try again.");
            Console.WriteLine();
            movie_add(filmlogic_obj);
        }
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
            Console.WriteLine(filmlogic_obj.info());
            Console.WriteLine("Id of movie you want to change: ");
            if (int.TryParse(Console.ReadLine(), out int film_id))
            {
                if(filmlogic_obj.Check_film(film_id))
                {
                    Console.WriteLine("What do you want to change:\n1. Title.\n2. Genre.\n3. Duration.\nchoice: ");
                    if (int.TryParse(Console.ReadLine(), out int choice_2))
                    {
                        switch(choice_2)
                        {
                            case 1:
                                Console.WriteLine("New title: ");
                                string new_title = Console.ReadLine();
                                Console.WriteLine(filmlogic_obj.change_title(film_id, new_title));
                                break;
                            case 2:
                                Console.WriteLine("New genre: ");
                                string new_genre = Console.ReadLine();
                                Console.WriteLine(filmlogic_obj.change_genre(film_id, new_genre));
                                break;
                            case 3:
                                Console.WriteLine("New duration: ");
                                if (int.TryParse(Console.ReadLine(), out int new_duration))
                                {
                                    Console.WriteLine(filmlogic_obj.change_duration(film_id, new_duration));
                                }
                                else
                                {
                                    Console.WriteLine("please enter a valid interger, try again.");
                                    Console.WriteLine();
                                    movie_change(filmlogic_obj);
                                }
                                break;
                            default:
                                Console.WriteLine("please enter a valid interger, try again.");
                                Console.WriteLine();
                                movie_change(filmlogic_obj);
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("please enter a valid interger, try again");
                        Console.WriteLine();
                        movie_change(filmlogic_obj);
                    }
                }
                else
                {
                    Console.WriteLine($"film with id: {film_id} does not exist, please try again.");
                }
            }
            else
            {
                Console.WriteLine("Id has to be an interger, try again.");
                Console.WriteLine();
                movie_change(filmlogic_obj); 
            }
        }
        else
        {
            Console.WriteLine("There are no films to change.");
        }
    }   
}