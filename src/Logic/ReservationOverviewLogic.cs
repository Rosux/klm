using Newtonsoft.Json;
public class ReservationOverviewLogic
{
    public string Overview(Reservation selected_res)
    {
        string overview = "";
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
            overview = overview + $"\nReservation info:\nId: {selected_res.Id}.\nRoom: {selected_res.RoomId}.\nGroup size: {selected_res.GroupSize}.\n\nTimeline:\nStart of reservation at {selected_res.StartDate}.\n";
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
                    overview = overview + $"\nAt {actie.StartTime} a order of {cons_cor.Name} for {cons_cor.Price} euro.";
                }
                else if(condition_con == false && condition_film == true && condition_episode == false)
                {
                    overview = overview + $"\nwatching the movie {film_cor.Title} from {actie.StartTime} to {actie.EndTime}.";
                }
                else if(condition_con == false && condition_film == false && condition_episode == true)
                {
                    overview = overview + $"\nwatching episode {episode_cor.Id}: {episode_cor.Title} from {serie_cor.Title} {season_cor.Title}, from {actie.StartTime} to {actie.EndTime}.";
                }
                else
                {
                    overview = overview + $"\nBreak form {actie.StartTime} to {actie.EndTime}.";
                }
            }
            overview = overview + $"\n\nEnd of reservation at {selected_res.EndDate}.";
            return overview;
        }
        else
        {
            return overview;
        }
    }
}
