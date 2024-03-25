using Newtonsoft.Json;
class FilmSerieMenu
{
    public static void UI()
    {
        Console.WriteLine("what do you want to change\n1. movies\n2. series\nchoice: ");
        int choice_1 = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();
        if (choice_1 == 1)
        {
            Console.WriteLine("1. all movies\n2. add movie\n3. remove movie\n4. change movie\nchoice:");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            FilmLogic filmlogic_obj = new FilmLogic();

            if (choice == 1)
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
            if (choice == 2)
            {
                int id = 0;
                Console.WriteLine("Title: ");
                string title = Console.ReadLine();
                Console.WriteLine("Genre: ");
                string genre = Console.ReadLine();
                Console.WriteLine("Duration(min): ");
                int duration = Convert.ToInt32(Console.ReadLine());
                filmlogic_obj.Add_film(new Film(title, genre, duration));
            }
            if (choice == 3)
            {
                if(filmlogic_obj.Check_film())
                {
                    Console.WriteLine(filmlogic_obj.info());
                    Console.WriteLine("Id: ");
                    int film_id = Convert.ToInt32(Console.ReadLine());
                    if(filmlogic_obj.Check_film(film_id))
                    {
                        Console.WriteLine(filmlogic_obj.Remove_film(film_id));
                    }
                    else
                    {
                        Console.WriteLine($"film with id: {film_id} does not exist.");
                    }
                }
                else
                {
                    Console.WriteLine("There are no films to remove.");
                }
            }
            if (choice == 4)
            {
                if(filmlogic_obj.Check_film())
                {
                    Console.WriteLine(filmlogic_obj.info());
                    Console.WriteLine("Id of movie you want to change: ");
                    int film_id = Convert.ToInt32(Console.ReadLine());
                    if(filmlogic_obj.Check_film(film_id))
                    {
                        Console.WriteLine("What do you want to change:\n1. Title.\n2. Genre.\n3. Duration.\nchoice: ");
                        int choice_3 = Convert.ToInt32(Console.ReadLine());
                        if(choice_3 == 1)
                        {
                            Console.WriteLine("New title: ");
                            string new_title = Console.ReadLine();
                            Console.WriteLine(filmlogic_obj.change_title(film_id, new_title));
                        }
                        if(choice_3 == 2)
                        {
                            Console.WriteLine("New genre: ");
                            string new_genre = Console.ReadLine();
                            Console.WriteLine(filmlogic_obj.change_genre(film_id, new_genre));
                        }
                        if(choice_3 == 3)
                        {
                            Console.WriteLine("New duration: ");
                            int new_duration = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine(filmlogic_obj.change_duration(film_id, new_duration));
                        }
                    }
                    else
                    {
                        Console.WriteLine($"film with id: {film_id} does not exist."); 
                    }
                }
                else
                {
                    Console.WriteLine("There are no films to change.");
                }
            }
        }

        if (choice_1 == 2)
        {
            Console.WriteLine("1. all Series\n2. add Serie\n3. add Season\n4. add Episode\n5. remove Serie\nchoice:");
            int choice_2 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            SerieLogic serie_obj = new SerieLogic();

            if (choice_2 == 1)
            {

                if (serie_obj.Check_Serie())
                {
                    Console.WriteLine(serie_obj.info());
                }
                else
                {
                    Console.WriteLine("There are no series.");
                }
                
            }
            if (choice_2 == 2)
            {
                Console.WriteLine("Serie title: ");
                string serie_title = Console.ReadLine();
                
                Console.WriteLine("Serie genre: ");
                string serie_genre = Console.ReadLine();
                Serie new_serie = new Serie(serie_title, serie_genre);

                serie_obj.Add_Serie(new_serie);
            }
            if (choice_2 == 3)
            {

                if (serie_obj.Check_Serie())
                {

                    Console.WriteLine(serie_obj.All_id());

                    Console.WriteLine("Serie Id: ");
                    int serie_id = Convert.ToInt32(Console.ReadLine());
                
                    Console.WriteLine(serie_obj.Add_Season(serie_id));
                }
                else
                {
                    Console.WriteLine("There are no series to add a season to.");
                }
                
            }
            if (choice_2 == 4)
            {
                Console.WriteLine(serie_obj.All_id());

                if (serie_obj.Check_Serie())
                {
                    SeasonLogic season_obj = new SeasonLogic();
                    
                    Console.WriteLine("Serie Id: ");
                    int serie_id = Convert.ToInt32(Console.ReadLine());

                    if (serie_obj.Check_Serie(serie_id))
                    {
                        Console.WriteLine(season_obj.All_Seasons(serie_id));

                        Console.WriteLine("Season to add episode: ");
                        int season_id = Convert.ToInt32(Console.ReadLine());
                        
                        if (season_obj.Check_Seasons(serie_id, season_id))
                        {
                            Console.WriteLine("Episode title: ");
                            string episode_title = Console.ReadLine();
                
                            Console.WriteLine("Episode duration(min): ");
                            int episode_duration = Convert.ToInt32(Console.ReadLine());
                
                            Episode new_episode = new Episode(episode_title, episode_duration);
                            Console.WriteLine(season_obj.Add_Episode(new_episode, serie_id, season_id));
                        }
                        else
                        {
                            Console.WriteLine($"This serie does not contain Season {season_id}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Serie with id: {serie_id} does not exist.");
                    }
                }
                else
                {
                    Console.WriteLine("There are no series.");
                }
                
                
            }
            if (choice_2 == 5)
            {

                if (serie_obj.Check_Serie())
                {
                    Serie serier = new Serie();
                    Console.WriteLine(serie_obj.All_id());
                    Console.WriteLine("Id: ");
                    int serie_id = Convert.ToInt32(Console.ReadLine());
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
                    Console.WriteLine("There are no series to remove.");
                }
            }
        }
    }
}