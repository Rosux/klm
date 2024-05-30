using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
public class MovieListManipulator
{
    public List<Movie> Get_MovieList()
    {
        string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"tmdb_movies.json"));
        string string_movies = File.ReadAllText(path);
        List<Movie> list_movies = JsonConvert.DeserializeObject<List<Movie>>(string_movies);
        return list_movies;
    }
    public void ChangeJson()
    {
        List<Movie> movielist = Get_MovieList();
        AddMovieIds(movielist); // Add unique IDs to each movie
        StreamWriter writer = new(@"ManipulatedMovieList.json");
        string string_movies = JsonConvert.SerializeObject(movielist);
        writer.Write(string_movies);
        writer.Close();
    }
    private void AddMovieIds(List<Movie> movies)
    {
        for (int i = 0; i < movies.Count; i++)
        {
            movies[i].Id = i + 1; // Generate IDs starting from 1
        }
    }
    
    public void PrintMovies(Func<List<Movie>> GetMovieList)
    {
        List<Movie> movieList = GetMovieList();
        Console.WriteLine($" Initialized movies: {movieList.Count}");
    }
}