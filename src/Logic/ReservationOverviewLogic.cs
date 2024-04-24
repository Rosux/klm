using Newtonsoft.Json;
public class ReservationOverviewLogic
{
    public string Overview(Reservation selected_res)
    {
        string overview = "";
        List<string> list_info = new List<string>();
        List<string> list_info_timeline = new List<string>();
        if (selected_res != null)
        {
            ConsumptionAccess con_accesser = new ConsumptionAccess();
            FilmAcesser film_accesser = new FilmAcesser();
            SerieAcesser serie_acesser = new SerieAcesser();
            bool condition_con = false;
            bool condition_film = false;
            bool condition_episode = false;
            Consumption cons_cor = new(-1,"",4, TimeOnly.Parse("12:00:00"), TimeOnly.Parse("23:00:00"));
            Film film_cor = new("","",-1,-1);
            Episode episode_cor = new("",-1,-1);
            Season season_cor = new("",-1);
            Serie serie_cor = new("","",-1);
            Console.Clear();

            foreach(TimeLine.Item actie in selected_res.TimeLine.t)
            {
                condition_con = false;
                condition_film = false;
                condition_episode = false;
                foreach(Consumption cons in con_accesser.ReadConsumption())
                {
                    TimeLine.Item timeline_item = new TimeLine.Item(cons, actie.StartTime, actie.EndTime);
                    string string_movies = JsonConvert.SerializeObject(timeline_item);
                    TimeLine.Item timeline = JsonConvert.DeserializeObject<TimeLine.Item>(string_movies);
                    string timeline_str = timeline.Action.ToString();
                    string actie_str = actie.Action.ToString();
                    if(String.Compare(timeline_str, actie_str) == 0)
                    {
                        cons_cor = cons;
                        condition_con = true;
                        break;
                    }
                }
                foreach(Serie serie in serie_acesser.Get_info())
                {
                    foreach(Season season in serie.Seasons)
                    {
                        foreach(Episode episode in season.Episodes)
                        {
                            TimeLine.Item timeline_item = new TimeLine.Item(episode, actie.StartTime, actie.EndTime);
                            string string_movies = JsonConvert.SerializeObject(timeline_item);
                            TimeLine.Item timeline = JsonConvert.DeserializeObject<TimeLine.Item>(string_movies);
                            string timeline_str = timeline.Action.ToString();
                            string actie_str = actie.Action.ToString();
                            if(String.Compare(timeline_str, actie_str) == 0)
                            {
                                episode_cor = episode;
                                season_cor = season;
                                serie_cor = serie;
                                condition_episode = true;
                                break;
                            }
                        }
                    }
                }
                foreach(Film film in film_accesser.Get_info())
                {
                    TimeLine.Item timeline_item = new TimeLine.Item(film, actie.StartTime, actie.EndTime);
                    string string_movies = JsonConvert.SerializeObject(timeline_item);
                    TimeLine.Item timeline = JsonConvert.DeserializeObject<TimeLine.Item>(string_movies);
                    string timeline_str = timeline.Action.ToString();
                    string actie_str = actie.Action.ToString();
                    if(String.Compare(timeline_str, actie_str) == 0)
                    {
                        film_cor = film;
                        condition_film = true;
                        break;
                    }
                }
                if(condition_con == true && condition_film == false && condition_episode == false)
                {
                    string cons_str = $"At {actie.StartTime} a order of {cons_cor.Name} for {cons_cor.Price} euro.";
                    list_info_timeline.Add(cons_str);
                }
                else if(condition_con == false && condition_film == true && condition_episode == false)
                {
                    string film_str = $"watching the movie {film_cor.Title} from {actie.StartTime} to {actie.EndTime}.";
                    list_info_timeline.Add(film_str);
                }
                else if(condition_con == false && condition_film == false && condition_episode == true)
                {
                    string episode_str = $"watching episode {episode_cor.Id}: {episode_cor.Title} from {serie_cor.Title} {season_cor.Title}, from {actie.StartTime} to {actie.EndTime}.";
                    list_info_timeline.Add(episode_str);
                }
                else
                {
                    string break_str = $"Break form {actie.StartTime} to {actie.EndTime}.";
                    list_info_timeline.Add(break_str);
                }
            }
            int longest = 0;
            foreach(string str in list_info_timeline)
            {
                if(str.Length > longest)
                {
                    longest = str.Length+4;
                }
            }

            string id_string = $"Id: {selected_res.Id}";
            string room_string = $"Room: {selected_res.RoomId}";
            string groupsize_string = $"Group size: {selected_res.GroupSize}";
            string userid_string = $"User id: {selected_res.UserId}";
            string start_string = $"Start date: {selected_res.StartDate}";
            string end_string = $"End date: {selected_res.EndDate}";
            list_info.Add(id_string);
            list_info.Add(room_string);
            list_info.Add(groupsize_string);
            list_info.Add(userid_string);
            list_info.Add(start_string);
            list_info.Add(end_string);

            foreach(string str in list_info)
            {
                if(str.Length > longest)
                {
                    longest = str.Length;
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
            overview = overview + $"┌─{new string('─', Math.Max(0, longest))}─┐\n";
            overview = overview + $"│";
            overview = overview + $"reservation info:";
            overview = overview + $"{new string(' ', Math.Max(0, longest+2- "reservation info:".Length))}│\n";
            Console.BackgroundColor = ConsoleColor.Black;
            overview = overview + $"│─{new string('─', Math.Max(0, longest ))}─│\n";
            foreach (string infos in list_info)
            {
                overview = overview + "│" + infos;
                overview = overview + $"{new string(' ', Math.Max(0, longest+2 - infos.Length))}│\n";
            }
            overview = overview + $"│─{new string('─', Math.Max(0, longest ))}─│\n";
            overview = overview + $"│";
            overview = overview + $"timeline:";
            overview = overview + $"{new string(' ', Math.Max(0, longest+2- "timeline:".Length))}│\n";
            int i = 1;
            foreach (string infos in list_info_timeline)
            {
                overview = overview + $"│{i}: " + infos;
                overview = overview + $"{new string(' ', Math.Max(0, longest - infos.Length-1))}│\n";
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