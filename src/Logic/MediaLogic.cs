public class MediaLogic
{
    public static void Media(){
        MenuHelper.Table<Media>(
            MediaAccess.GetAllMedia(),
            new Dictionary<string, Func<Media, object>>(){
                {"Id", m=>m.Id},
                {"Title", m=>m.Title},
                {"Language", m=>m.Language},
                {"Genres", m=>string.Join(", ", m.Genres)},
                {"Release Date", m=>m.ReleaseDate},
                {"Certification", m=>m.Certification},
            },
            false,
            true,
            true,
            new Dictionary<string, PropertyEditMapping<Media>>(){
                {"Title", new(m=>m.Title, GetValidTitle)},
                {"Language", new(m=>m.Title, GetValidTitle)},
                {"Genres", new(m=>m.Title, GetValidTitle)},
                {"Release Date", new(m=>m.Title, GetValidTitle)},
                {"Certification", new(m=>m.Title, GetValidTitle)},
            },
            CreateNewMedia,
            true,
            CreateNewMedia,
            true,
            CreateNewMedia
        );
        MenuHelper.SelectFromTable(
            MediaAccess.GetAllMedia(),
            new Dictionary<string, Func<Media, object>>(){
                {"Id", m=>m.Id},
                {"Title", m=>m.Title},
                {"Language", m=>m.Language},
                {"Genres", m=>string.Join(", ", m.Genres)},
                {"Release Date", m=>m.ReleaseDate},
                {"Certification", m=>m.Certification},
            },
            true
        )
    }
}