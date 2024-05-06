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
        bool duplicate = true;
        FilmAcesser filmacesser = new();
        List<Film> list_films = filmacesser.Get_info();
        foreach(Film film_2 in list_films)
        {
            if (film_2.Title.ToUpper() != film.Title.ToUpper()){}
            else
            {
                duplicate = false;
            }
        }
        if (duplicate)
        {
            IdAccess Idacesser = new();
            List<int> list_Id = Idacesser.Get_Id();
            int new_Id = list_Id[0] + 1;
            film.Id = new_Id;
            list_Id.RemoveAt(0);
            list_Id.Insert(0, new_Id);
            Idacesser.Return_Id(list_Id);
            list_films.Add(film);
            filmacesser.Return_info(list_films);
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