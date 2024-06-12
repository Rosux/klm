using Newtonsoft.Json;

public struct MediaJsonStruct
{
    public int CurrentFilmId = 0;
    public List<Film> Films { get; set; } = new List<Film>();
    public int CurrentSerieId = 0;
    public List<Serie> Series { get; set; } = new List<Serie>();

    public MediaJsonStruct(){}

    [JsonConstructor]
    public MediaJsonStruct(int currentFilmId, List<Film> films, List<Serie> series, int currentSerieId)
    {
        this.CurrentFilmId = currentFilmId;
        this.Films = films;
        this.CurrentSerieId = currentSerieId;
        this.Series = series;
    }
}