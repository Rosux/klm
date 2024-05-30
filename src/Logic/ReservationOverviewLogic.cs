using Newtonsoft.Json;
public class ReservationOverviewLogic
{
    /// <summary>
    /// takes a reservation object and puts all the information in a tabel does this by taking every object form timeline and matches it to its object (episode, consumption, film, break)
    ///
    /// </summary>
    /// <param name="selected_res"></param>
    /// <returns> a string in the form of a tabel of the given reservation</returns>
    ///┌─────────────────────────────────────────────────────────────────────────────────┐
    // │reservation info:                                                                │
    // │─────────────────────────────────────────────────────────────────────────────────│
    // │Id: 28                                                                           │
    // │Room: 1                                                                          │
    // │Group size: 7                                                                    │
    // │User id: 1                                                                       │
    // │Start date: 26-4-2024 02:00:00                                                   │
    // │End date: 28-4-2024 04:00:00                                                     │
    // │Price: 172 euro                                                                  │
    // │─────────────────────────────────────────────────────────────────────────────────│
    // │timeline:                                                                        │
    // │1: watching the movie Mudrunner from 26-4-2024 15:00:00 to 26-4-2024 18:00:00.   │
    // │2: At 27-4-2024 02:00:00 a order of fries for 4 euro.                            │
    // │3: Break from 28-4-2024 04:00:00 to 28-4-2024 04:55:00.                          │
    // └─────────────────────────────────────────────────────────────────────────────────┘

    public string Overview(Reservation selected_res)
    {
        string overview = "";
        // hold a string format of all reservations info like price, startdate, etcetra
        List<string> list_info = new List<string>();
        // hold a string format of all timeline objects like film, episode, break, consumption
        List<string> list_info_timeline = new List<string>();
        if (selected_res != null)
        {
            bool condition_con = false;
            bool condition_film = false;
            bool condition_episode = false;
            Consumption cons_cor = null;
            Film film_cor = null;
            Episode episode_cor = null;
            Console.CursorVisible = false;
            Console.Clear();
            // matches every object in time line to either consumption, episode, film or break
            foreach(TimeLine.Item actie in selected_res.TimeLine.t)
            {
                condition_con = false;
                condition_film = false;
                condition_episode = false;

                if (actie.Action is Film)
                {
                    condition_film = true;
                    film_cor = (Film)actie.Action;
                }
                else if(actie.Action is Episode)
                {
                    condition_episode = true;
                    episode_cor = (Episode)actie.Action;
                }
                else if(actie.Action is Consumption)
                {
                    condition_con = true;
                    cons_cor = (Consumption)actie.Action;
                }
                // checks if current object from timeline is a consumption and makes a string format with the consumption info an adds it to list list_info_timeline
                if(condition_con == true && condition_film == false && condition_episode == false)
                {
                    string cons_str = $"At {actie.StartTime} a order of {cons_cor.Name} for {cons_cor.Price} euro.";
                    list_info_timeline.Add(cons_str);
                }
                // checks if current object from timeline is a film and makes a string format with the film info an adds it to list list_info_timeline
                else if(condition_con == false && condition_film == true && condition_episode == false)
                {
                    string film_str = $"watching the movie {film_cor.Title} from {actie.StartTime} to {actie.EndTime}.";
                    list_info_timeline.Add(film_str);
                }
                // checks if current object from timeline is a episodde and makes a string format with the episode/sereie/season info an adds it to list list_info_timeline
                else if(condition_con == false && condition_film == false && condition_episode == true)
                {
                    string episode_str = $"watching episode {episode_cor.Id}: {episode_cor.Title}, from {actie.StartTime} to {actie.EndTime}.";
                    list_info_timeline.Add(episode_str);
                }
                // checks if current object from timeline is a break and makes a string format with the break info an adds it to list list_info_timeline
                else
                {
                    string break_str = $"Break from {actie.StartTime} to {actie.EndTime}.";
                    list_info_timeline.Add(break_str);
                }
            }
            int longest = 0;
            int i = 1;
            // sets the lengt of the tabel if there is a timeline
            foreach(string str in list_info_timeline)
            {
                string string_cor = $"│ {i}: " + str;
                if(string_cor.Length > longest)
                {
                    longest = string_cor.Length-2;
                }
                i++;
            }
            // converts all reservation info to a string and adds it to list_info
            string id_string = $" Id: {selected_res.Id}";
            string room_string = $" Room: {selected_res.RoomId}";
            string groupsize_string = $" Group size: {selected_res.GroupSize}";
            string userid_string = $" User id: {selected_res.UserId}";
            string start_string = $" Start date: {selected_res.StartDate}";
            string end_string = $" End date: {selected_res.EndDate}";
            string price_string = $" Price: {selected_res.Price} euro";
            list_info.Add(id_string);
            list_info.Add(room_string);
            list_info.Add(groupsize_string);
            list_info.Add(userid_string);
            list_info.Add(start_string);
            list_info.Add(end_string);
            list_info.Add(price_string);

            // sets the lengt of the tabel in case there is no timeline
            foreach(string str in list_info)
            {
                if(str.Length > longest)
                {
                    longest = str.Length-1;
                }
            }
            // makes the tabel with all the info
            Console.BackgroundColor = ConsoleColor.Black;
            overview = overview + $"┌─{new string('─', Math.Max(0, longest))}─┐\n";
            overview = overview + $"│";
            overview = overview + $" reservation info:";
            overview = overview + $"{new string(' ', Math.Max(0, longest+1- "reservation info:".Length))}│\n";
            Console.BackgroundColor = ConsoleColor.Black;
            overview = overview + $"│─{new string('─', Math.Max(0, longest ))}─│\n";
            // adds all reservation info to the tabel from list_info
            foreach (string infos in list_info)
            {
                overview = overview + "│" + infos;
                overview = overview + $"{new string(' ', Math.Max(0, longest+2 - infos.Length))}│\n";
            }
            overview = overview + $"│─{new string('─', Math.Max(0, longest ))}─│\n";
            overview = overview + $"│";
            overview = overview + $" timeline:";
            overview = overview + $"{new string(' ', Math.Max(0, longest+1- "timeline:".Length))}│\n";
            i = 1;
            // adds all timeline info to the tabel from list_info_timeline
            foreach (string infos in list_info_timeline)
            {
                overview = overview + $"│ {i}: " + infos;
                overview = overview + $"{new string(' ', Math.Max(0, longest - infos.Length-2))}│\n";
                i++;
            }
            overview = overview + $"└─{new string('─', Math.Max(0, longest ))}─┘\n";
            return overview;
        }
        else
        {
            return overview;
        }
    }
}