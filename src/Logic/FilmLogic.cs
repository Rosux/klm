public class FilmLogic
{
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
                all_info = all_info + $"film Id: {film.Id}\ntitle: {film.Title}\ngenre: {film.Genre}\nduration: {film.Duration}\n";
                all_info = all_info + $"_________________________________________\n";
            }
            return all_info;
        }
    }

    public string Add_film(Film film)
    {
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        film.Id = list_films.Count();
        list_films.Add(film);
        filmacesser.Return_info(list_films);
        return $"you succesfully added the movie {film.Title}.";
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
        filmacesser.Return_info(list_films);
        return info;
    }

    public bool Check_film(int id = 0)
    {
        bool all_info = false;
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                all_info = true;
            }
        }
        return all_info;
    }

    public string change_title(int id, string new_title)
    {
        string info = "";
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                info = $"you sucesfully changed title from {film.Title} to {new_title}.";
                film.Title = new_title;
            }
        }
        filmacesser.Return_info(list_films);
        return info;
    }

    public string change_genre(int id, string new_genre)
    {
        string info = "";
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film in list_films)
        {
            if(film.Id == id)
            {
                info = $"you sucesfully changed genre from {film.Genre} to {new_genre}.";
                film.Genre = new_genre;
            }
        }
        filmacesser.Return_info(list_films);
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
                info = $"you sucesfully changed duration from {film.Duration} min to {new_duration} min.";
                film.Duration = new_duration;
            }
        }
        filmacesser.Return_info(list_films);
        return info;
    }
}
