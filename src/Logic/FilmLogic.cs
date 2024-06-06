public class FilmLogic
{
    public static ReservationAccess _reservationAccess = new ReservationAccess();
    public string info()
    {
        string all_info = "";
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        if (list_films.Count() == 0)
        {
            return "There are no films.";
        }
        else
        {
            foreach(Film film in list_films)
            {
                all_info = all_info + $"film Id: {film.Id}\ntitle: {film.Title}";
                all_info = all_info + $"\n_______________________________________\n";
            }
            return all_info;
        }
    }


    public string Add_film(Film film)
    {
        bool duplicate = true;
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film_2 in list_films)
        {
            if (film_2.Id != film.Id ){}
            else
            {
                duplicate = false;
            }
        }
        if (duplicate)
        {
            int new_Id = CreateID();
            film.Id = new_Id;
            list_films.Add(film);
            filmacesser.Return_info(list_films, null);
            return $"you succesfully added the movie {film.Title}.";
        }
        else
        {
            return "you can not add duplicate movie.";
        }
    }

    public string Remove_film(int id)
    {
        Film r_film = null;
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                r_film = film;
            }
        }

        list_films.Remove(r_film);
        int i = 0;
        string info = $"you sucesfully removed {r_film.Title}.";
        foreach(Film film in list_films)
        {
            film.Id = i;
            i++;
        }
        filmacesser.Return_info(list_films, r_film);
        return info;
    }


    public int CreateID()
    {
        IdAccess Idacesser = new();
        List<int> list_Id = Idacesser.Get_Id();
        int new_Id = list_Id[0] + 1;
        list_Id.RemoveAt(0);
        list_Id.Insert(0, new_Id);
        Idacesser.Return_Id(list_Id);
        return new_Id;
    }

    public bool Check_film(int id = 0)
    {
        return true;
    }

    public bool Check_films_exist()
    {
        bool all_info = false;
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        if(list_films.Count() != 0)
        {
            all_info = true;
        }
        return all_info;
    }

    public string change_title(int id, string new_title)
    {
        string info = "";
        FilmAcesser filmacesser = new();
        //get list of all films
        List<Film> list_films = filmacesser.Get_info();
        //loop through all films to change the part
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                info = $"you sucesfully changed title from {film.Title} to {new_title}.";
                film.Title = new_title;
            }
        }
        //write new list to the json
        filmacesser.Return_info(list_films, null);
        return info;
    }

    public string change_genre(int id, List<string> new_genres)
    {
        string info = "";
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                info = $"you sucesfully changed genres";
                film.Genres = new_genres;
            }
        }
        filmacesser.Return_info(list_films, null);
        return info;
    }

    public string change_duration(int id, int new_duration)
    {
        string info = "";
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                info = $"you sucesfully changed Runtime from {film.Runtime} min to {new_duration} min.";
                film.Runtime = new_duration;
            }
        }
        filmacesser.Return_info(list_films, null);
        return info;
    }
    public string change_language(int id, string language)
    {
        string info = "";
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                info = $"you sucesfully changed language from {film.OriginalLanguage} min to {language}";
                film.OriginalLanguage = language;
            }
        }
        filmacesser.Return_info(list_films, null);
        return info;
    }

    public string change_releasedate(int id, string releasedate)
    {
        string info = "";
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                info = $"you sucesfully changed language from {film.ReleaseDate} min to {releasedate}";
                film.ReleaseDate = releasedate;
            }
        }
        filmacesser.Return_info(list_films, null);
        return info;
    }

    public string change_vote(int id, double new_vote_average)
    {
        string info = "";
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                info = $"you sucesfully changed vote average from {film.VoteAverage} min to {new_vote_average}";
                film.VoteAverage = new_vote_average;
            }
        }
        filmacesser.Return_info(list_films, null);
        return info;
    }
    public string change_overview(int id, string new_overview)
    {
        string info = "";
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                info = $"you sucesfully changed movie overview from {film.Overview} to {new_overview}";
                film.Overview = new_overview;
            }
        }
        filmacesser.Return_info(list_films, null);
        return info;
    }
    // change age certification of given film (id)
    public string change_certification(int id, string new_certification)
    {
        string info = "";
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                info = $"you sucesfully changed movie certification from {film.Certification} to {new_certification}";
                film.Certification = new_certification;
            }
        }
        filmacesser.Return_info(list_films, null);
        return info;
    }

    //Changes the director of a given film (id)
    public string change_director(int id, List<string> directorsList)
    {
        string info = "";
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                info = $"you sucesfully changed movie director";
                film.Directors = directorsList;
            }
        }
        filmacesser.Return_info(list_films, null);
        return info;
    }

    public bool ExistsReservationCheck(Film film)
    {
        bool exists = false;
        List<Reservation> Reservations = _reservationAccess.GetAllReservations();
        foreach(Reservation Reservation in Reservations)
        {
            foreach(var TimelineObject in Reservation.TimeLine.Items.ToList())
            {
                if(TimelineObject.Action is Film)
                {
                    if(((Film)TimelineObject.Action).Id == film.Id)
                    {
                        exists = true;
                    }
                }
            }
        }
        return exists;
    }
}