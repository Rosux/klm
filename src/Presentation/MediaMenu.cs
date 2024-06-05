public static class MediaMenu
{
    public static Media? GetNewMedia(){
        string prompt = "Title: \nLanguage: \nGenres: \nRelease Date: \nCertification: \n";

        string Title;
        while(true){
            string? title = MenuHelper.SelectText(prompt+"\nType the title of the film or serie.", "", true, 2, 30);
            if(title == null){
                return null;
            }
            if(MediaAccess.MediaExists(title)){
                Title = title;
                break;
            }else{
                Console.Write($"Title must be unique!\nTitle: '{title}' already exists.\n\nPress any key to continue.");
                Console.ReadKey(true);
            }
        }
            /// <param name="directors">A list of directors.</param>
            int runtime = MenuHelper.SelectInteger("Please enter the runtime in minutes:", "", 0, 0,1000);
            string? description = MenuHelper.SelectText("Please enter the description: ", "", true, 10, 500);
            float? rating = MenuHelper.SelectFloat("Please enter a rating from 1-10:", "", true, 0, 0, 10);
            string? language = MenuHelper.SelectText("Please enter a language: ", "", true, 0, 30);
            List<Genre> genres = new List<Genre>
            {
                Genre.NONE,
                Genre.HORROR,
                Genre.ACTION,
                Genre.COMEDY,
                Genre.FAMILY,
                Genre.DRAMA,
                Genre.ADVENTURE,
                Genre.FANTASY,
                Genre.THRILLER,
                Genre.MYSTERY,
                Genre.CRIME
            };
            Genre? selectedGenre = MenuHelper.SelectFromEnum<Genre>("Please select a genre:", "", false, 0, 40,"([A-z]| )", genres);
            DateOnly? releaseDate = MenuHelper.SelectDate("Please select a date", true);
            List<Certification> certifications = new List<Certification>
            {
                Certification.NONE,
                Certification.G,
                Certification.PG,
                Certification.PG13,
                Certification.PG18,
                Certification.R,
                Certification.NC17,
                Certification.TVY,
                Certification.TVY7,
                Certification.TVG,
                Certification.TVPG,
                Certification.TV14,
                Certification.TVMA,
                Certification.X
            };
            Certification? selectedCertification = MenuHelper.SelectFromEnum<Certification>("Please select a certification:", "", false, 0, 40,"([A-z]| )", certifications);
            string? directors = MenuHelper.SelectText("Please enter a director: ", "", true, 0, 30);
            // Media media = new Media(Title, (int)runtime, (string)description, (float)rating, (string)language, selectedGenre, (DateOnly)releaseDate, certifications, directors);
    }
}