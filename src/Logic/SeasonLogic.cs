public class SeasonLogic
{
        public string Add_Episode(Episode episode, int serie_id, int season_id)
    {
        string all_info = "";
        SerieAcesser serieacesser = new();
        List<Serie> list_series = serieacesser.Get_info();
        foreach(Serie serie in list_series)
        {
            if(serie.Id == serie_id)
            {
                foreach (Season season in serie.Seasons)
                {
                    if(season.Id == season_id)
                    {
                        episode.Id = season.Episodes.Count() +1;
                        all_info = $"You sucesfully added episode {episode.Id}: {episode.Title}.";
                        season.Episodes.Add(episode);
                    }
                }
            }
        }
        serieacesser.Return_info(list_series);
        return all_info;
    }

    public bool Check_Seasons(int serie_id, int season_id)
    {
        bool all_info = false;
        SerieAcesser serieacesser = new();
        List<Serie> list_series = serieacesser.Get_info();
        foreach(Serie serie in list_series)
        {
            if(serie.Id == serie_id)
            {
                foreach (Season season in serie.Seasons)
                {
                    if(season.Id == season_id)
                    {
                        all_info = true;
                    }
                }
            }
        }
        return all_info;
    }

    public string All_Seasons(int id)
    {
        string all_info = "";
        SerieAcesser serieacesser = new();
        List<Serie> list_series = serieacesser.Get_info();
        foreach(Serie serie in list_series)
        {
            if(serie.Id == id)
            {
                foreach (Season season in serie.Seasons)
                {
                    all_info = all_info + $"\n{season.Title}:";
                    foreach (Episode episode in season.Episodes)
                    {
                        all_info = all_info + $"\nEpisode {episode.Id}: {episode.Title}, {episode.Length} min";
                    }
                    all_info = all_info + $"\n";
                }            
            }
        }
        return all_info;
    }
}
