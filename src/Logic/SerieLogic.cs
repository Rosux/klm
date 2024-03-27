
public class SerieLogic
{
    
    public string Add_Serie(Serie serie)
    {
        SerieAcesser serieacesser = new();
        List<Serie> list_serie = serieacesser.Get_info();
        serie.Id = list_serie.Count();
        list_serie.Add(serie);
        serieacesser.Return_info(list_serie);
        return $"you succesfully added the serie {serie.Title}.";
    }

    public string Add_Season(int id)
    {
        string info = $"could not find a serie with Id: {id}";
        int Amount_seasons = 0;
        SerieAcesser serieacesser = new();
        List<Serie> list_series = serieacesser.Get_info();
        foreach(Serie serie in list_series)
        {
            if(serie.Id == id)
            {
                Amount_seasons = serie.Seasons.Count() +1;
                serie.Seasons.Add(new Season($"Season {Amount_seasons}", Amount_seasons));
                info = $"you added season {Amount_seasons}.";
            }
        }
        serieacesser.Return_info(list_series);
        return info;
    }

    public string All_id()
    {
        string all_info = "";
        SerieAcesser serieacesser = new();
        List<Serie> list_series = serieacesser.Get_info();
        foreach(Serie serie in list_series)
        {
            all_info = all_info + $"\nSerie Id: {serie.Id}\nSerie title: {serie.Title}\nAmount of seasons: {serie.Seasons.Count()}\n";
        }
        return all_info;
    }

    public bool Check_Serie(int id = 0)
    {
        bool all_info = false;
        SerieAcesser serieacesser = new();
        List<Serie> list_series = serieacesser.Get_info();
        foreach(Serie serie in list_series)
        {
            if(serie.Id == id)
            {
                all_info = true;
            }
        }
        return all_info;
    }

    public string info()
    {
        string all_info = "";
        SerieAcesser serieacesser = new();
        List<Serie> list_series = serieacesser.Get_info();
        foreach(Serie serie in list_series)
        {
            all_info = all_info + $"Serie Id: {serie.Id}\nSerie title: {serie.Title}\nSerie genre: {serie.Genre}\n";
            foreach (Season season in serie.Seasons)
            {
                all_info = all_info + $"{season.Title}:";
                foreach (Episode episode in season.Episodes)
                {
                    all_info = all_info + $"\nEpisode {episode.Id}: {episode.Title}, {episode.Length} min.";
                }
                all_info = all_info + "\n\n";
            }
            all_info = all_info + $"_________________________________________\n";
        }
        return all_info;
    }
    
    public string Remove_serie(int id)
    {
        Serie r_serie = null;
        SerieAcesser serieacesser = new();
        List<Serie> list_series = serieacesser.Get_info();
        foreach(Serie serie in list_series)
        {
            if(serie.Id == id)
            {
                r_serie = serie;
            }
        }

        list_series.Remove(r_serie);
        int i = 0;
        string info = $"you sucesfully removed {r_serie.Title}.";
        foreach(Serie serie in list_series)
        {
            serie.Id = i;
            i++;
        }
        serieacesser.Return_info(list_series);
        return info;
    }
}