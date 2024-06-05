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
    }

    private static Media CreateNewMedia(){
        
    }

    private static string GetValidTitle(Media previousMedia){
        string prompt = $"Current Title: {previousMedia.Title}\nCurrent Language: {previousMedia.Language}\nCurrent Genres: {previousMedia.Genres}\nCurrent Release Date: {previousMedia.ReleaseDate}\nCurrent Certification: {previousMedia.Certification}";
        string? newTitle = MenuHelper.SelectText(prompt+"Enter the new name of the product:", "", true, 2, 30);
        return newTitle ?? previousMedia.Title;
    }
}