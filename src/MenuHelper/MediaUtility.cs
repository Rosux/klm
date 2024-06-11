namespace MenuHelper
{

    public static class MediaUtility
    {
        private static SearchAccess searchAccess = new SearchAccess();
        /// <summary>
        /// Ask the user to select a movie or series episodes.
        /// </summary>
        /// <returns>null if the user cancels the search. If a movie is selected it will return a Movie object. if a series is selected it will return a Dictionary<Serie, List<Episode>></returns>
        public static object? SelectMovieOrEpisode(){
            Media selectedMedia;
            int longestWord;
            string searchString = "";
            int cursorPosition = searchString.Length;
            bool typing = true;
            int selectedResult = 0;
            ConsoleKey key;
            ConsoleKeyInfo keyInfo;
            do{
                // calculate longest word
                List<Media> results = searchAccess.Search(searchString);
                longestWord = "Start typing to search".Length + 2;
                foreach(Media m in results){
                    if (m is Film && ((Film)m).Title.Length+3 > longestWord){
                        longestWord = ((Film)m).Title.Length + 3;
                    }else if(m is Serie && ((Serie)m).Title.Length+3 > longestWord){
                        longestWord = ((Serie)m).Title.Length + 3;
                    }
                }
                if (searchString.Length + 2 > longestWord){
                    longestWord = searchString.Length + 2;
                }

                // print the search box thing
                Console.CursorVisible = false;
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"Search by Titles, Genres or Directors seperated by commas.\nPress escape to cancel.\n\n┌─Start typing to search{new string('─', Math.Max(0, longestWord-22))}─┐\n");
                Console.Write($"│ > ");
                string printedText = searchString.Substring(Math.Max(0, searchString.Length-longestWord));
                for(int i = 0; i < printedText.Length; i++){
                    Console.BackgroundColor = (i == cursorPosition && typing) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    Console.Write($"{printedText[i]}");
                }
                Console.BackgroundColor = (cursorPosition == printedText.Length && typing) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                Console.Write(" ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"{new string(' ', Math.Max(0, longestWord-printedText.Length-2))}");
                Console.Write($"│\n");

                if(searchString == "" || results.Count() == 0){
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"└─{new string('─', Math.Max(0, longestWord))}─┘");
                }else{
                    for(int i=0;i<Math.Min(results.Count(), 5);i++)
                    {
                        if (i == 0) {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("└── ");
                            string title = results[i] is Film ? ((Film)results[i]).Title : ((Serie)results[i]).Title;
                            Console.BackgroundColor = (!typing && selectedResult == i) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                            Console.Write($"{title}{new string(' ', Math.Max(0, longestWord-title.Length-3))}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write($" ─┘\n");
                        } else {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("    ");
                            Console.BackgroundColor = (!typing && selectedResult == i) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                            string title = results[i] is Film ? ((Film)results[i]).Title : ((Serie)results[i]).Title;
                            Console.Write($"{title}{new string(' ', Math.Max(0, longestWord-title.Length-3))}\n");
                        }
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }

                keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;

                if(typing && (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)) {
                    if (key == ConsoleKey.LeftArrow && cursorPosition > 0) {
                        cursorPosition--; // Move cursor left if not at the beginning
                    } else if (key == ConsoleKey.RightArrow && cursorPosition < searchString.Length) {
                        cursorPosition++; // Move cursor right if not at the end
                    }
                }
                if (typing && !char.IsControl(keyInfo.KeyChar)) {
                    searchString = searchString.Insert(cursorPosition, keyInfo.KeyChar.ToString());
                    cursorPosition++;
                }
                if (typing && cursorPosition > 0 && searchString.Length > 0 && key == ConsoleKey.Backspace) {
                    searchString = searchString.Remove(cursorPosition - 1, 1);
                    cursorPosition--;
                }
                if (!typing && (key == ConsoleKey.DownArrow || key == ConsoleKey.UpArrow)) {
                    // move selection up/down
                    if (key == ConsoleKey.UpArrow && selectedResult == 0) {
                        typing = true;
                        continue;
                    }
                    selectedResult += (key == ConsoleKey.DownArrow) ? 1 : -1;
                }
                if (typing && key == ConsoleKey.DownArrow && results.Count() > 0) {
                    typing = false;
                    continue;
                }
                if (!typing && key == ConsoleKey.Enter) {
                    selectedMedia = results[selectedResult];
                    if (selectedMedia is Serie) {
                        List<string>? allSelectedEpisodes = SelectEpisodes((Serie)selectedMedia);
                        if (allSelectedEpisodes == null) {
                            continue;
                        }else{
                            Dictionary<Serie, List<Episode>> uwu = new Dictionary<Serie, List<Episode>>();
                            uwu.Add((Serie)selectedMedia, new List<Episode>());
                            for(int i=0;i<allSelectedEpisodes.Count;i++){
                                int season = Convert.ToInt32(allSelectedEpisodes[i].Split('.')[0]);
                                int episode = Convert.ToInt32(allSelectedEpisodes[i].Split('.')[1]);
                                uwu[(Serie)selectedMedia].Add(((Serie)selectedMedia).Seasons[season].Episodes[episode]);
                            }
                            Console.CursorVisible = false;
                            Console.Clear();
                            return uwu;
                        }
                    } else if (selectedMedia is Film) {
                        break;
                    }
                }
                if(key == ConsoleKey.Escape){
                    Console.CursorVisible = false;
                    Console.Clear();
                    return null;
                }
                selectedResult = Math.Clamp(selectedResult, 0, Math.Max(0, Math.Min(results.Count(), 5)-1));
                cursorPosition = Math.Clamp(cursorPosition, 0, Math.Max(0, searchString.Length));
            }while(true);
            Console.CursorVisible = false;
            Console.Clear();
            return selectedMedia;
        }

        /// <summary>
        /// Asks the user to select episodes and returns a list of the episodes the user selected.
        /// </summary>
        /// <param name="serie">A Serie object to select episodes from</param>
        /// <returns>null if the user doesnt select anything. A list of strings containing the user selected episodes formatted as: "1.1" being season 1 episode 1.</returns>
        public static List<string>? SelectEpisodes(Serie serie){
            if(serie.Seasons.Count == 0 || serie.Seasons[0].Episodes.Count == 0)
            {
                Console.CursorVisible = false;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"The series {serie.Title} has no seasons or episodes.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"\n\nPress any key to return...");
                Console.ReadKey(true);
                return new List<string>();
            }
            bool selectSeason = true;
            bool saving = false;
            int selectedSeason = 0;
            int selectedEpisode = 0;
            List<Season> allSeasons = serie.Seasons;
            List<Episode> selectedEpisodes = new List<Episode>();
            ConsoleKey key;
            do{
                List<Episode> allEpisodes = allSeasons[selectedSeason].Episodes;

                int longestSeasonName = "Select season".Length;
                int longestEpisodeName = "Select Episode".Length;

                // calculate season/episode name length
                foreach(Season s in allSeasons){
                    if (s.Title.Length+3 > longestSeasonName){
                        longestSeasonName = s.Title.Length+3;
                    }
                }
                foreach(Episode e in allEpisodes){
                    if (e.Title.Length+3 > longestEpisodeName){
                        longestEpisodeName = e.Title.Length+3;
                    }
                }

                List<string> seasons = new List<string>();
                List<string> episodes = new List<string>();
                for(int i=0;i<allSeasons.Count;i++){
                    Season s = allSeasons[i];
                    for(int j=0;j<s.Episodes.Count;j++){
                        Episode e = s.Episodes[j];
                        bool add = false;
                        foreach(Episode se in selectedEpisodes){
                            if (se == e){ add = true; }
                        }
                        if(add){episodes.Add($"{i+1}.{j+1}");}
                        if(add && !seasons.Contains($"{i+1}")){seasons.Add($"{i+1}");}
                    }
                }
                Console.CursorVisible = false;
                Console.Clear();
                Console.Write($"Series: {serie.Title}\nSeasons: ");
                for(int i=0;i<seasons.Count;i++){
                    if(i == seasons.Count-1){
                        Console.Write($"{seasons[i]}\n");
                    }else{
                        Console.Write($"{seasons[i]}, ");
                    }
                }
                Console.Write($"Episodes: ");
                for(int i=0;i<episodes.Count;i++){
                    if(i == episodes.Count-1){
                        Console.Write($"{episodes[i]}\n");
                    }else{
                        Console.Write($"{episodes[i]}, ");
                    }
                }
                Console.Write($"\n\n┌─Select season{new string('─', Math.Max(0, longestSeasonName-13))}─┐ -> ┌─Select Episode{new string('─', Math.Max(0, longestEpisodeName-14))}─┐\n");
                for(int i=0;i<Math.Max(allSeasons.Count+2, allEpisodes.Count+1);i++){
                    if(i == allSeasons.Count+1){
                        Console.Write($"└─{new string('─', Math.Max(0, longestSeasonName))}─┘    ");
                    }else if(i == allSeasons.Count){
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"│ ");
                        Console.BackgroundColor = (saving) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                        Console.Write($"{i+1}. Select");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"{new string(' ', Math.Max(0, longestSeasonName-(i+1).ToString().Length-8))} │    ");
                    }else if(i < allSeasons.Count){
                        Season s = allSeasons[i];
                        int seasonTextLength = (i+1).ToString().Length + 2 + s.Title.Length;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"│ ");
                        Console.BackgroundColor = (!saving && s == allSeasons[selectedSeason]) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                        bool hasAllSeasons = allSeasons[i].Episodes.All(e => selectedEpisodes.Contains(e));
                        if(hasAllSeasons){
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }
                        Console.Write($"{i+1}. {s.Title}");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"{new string(' ', Math.Max(0, longestSeasonName-seasonTextLength))} │    ");
                    }else{
                        Console.Write($"{new string(' ', longestSeasonName+8)}");
                    }
                    if(i < allEpisodes.Count){
                        Episode e = allEpisodes[i];
                        int episodeTextLength = (i+1).ToString().Length + 2 + e.Title.Length;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"│ ");
                        Console.BackgroundColor = (!selectSeason && e == allSeasons[selectedSeason].Episodes[selectedEpisode]) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                        if(selectedEpisodes.Contains(e)){
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }
                        Console.Write($"{i+1}. {e.Title}");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"{new string(' ', Math.Max(0, longestEpisodeName-episodeTextLength))} │\n");
                    }else if(i == allEpisodes.Count){
                        Console.Write($"└─{new string('─', Math.Max(0, longestEpisodeName))}─┘\n");
                    }else{
                        Console.Write("\n");
                    }
                }
                Console.Write("\nPress escape to cancel.\n");
                key = Console.ReadKey(true).Key;
                if(key == ConsoleKey.Escape){
                    return null;
                }
                if(key == ConsoleKey.LeftArrow){
                    selectSeason = true;
                }else if(key == ConsoleKey.RightArrow && !saving){
                    selectSeason = false;
                }
                if(key == ConsoleKey.UpArrow){
                    if(selectSeason && saving){
                        saving = false;
                        continue;
                    }
                    if (selectSeason) {
                        selectedEpisode = 0;
                        selectedSeason--;
                    }else{
                        selectedEpisode--;
                    }
                }else if(key == ConsoleKey.DownArrow){
                    if(!saving && selectedSeason == allSeasons.Count-1 && selectSeason){
                        saving = true;
                        continue;
                    }
                    if (selectSeason) {
                        selectedEpisode = 0;
                        selectedSeason++;
                    }else{
                        selectedEpisode++;
                    }
                }
                if(key == ConsoleKey.Enter){
                    if(saving){
                        break;
                    }else if(!selectSeason){
                        // add specific episode
                        if (selectedEpisodes.Contains(allSeasons[selectedSeason].Episodes[selectedEpisode])){
                            selectedEpisodes.Remove(allSeasons[selectedSeason].Episodes[selectedEpisode]);
                        }else{
                            selectedEpisodes.Add(allSeasons[selectedSeason].Episodes[selectedEpisode]);
                        }
                    }else{
                        bool hasAllSeasons = allSeasons[selectedSeason].Episodes.All(e => selectedEpisodes.Contains(e));
                        // add/remove whole season
                        if(!hasAllSeasons){
                            foreach (Episode e in allSeasons[selectedSeason].Episodes){
                                selectedEpisodes.Remove(e);
                            }
                            foreach (Episode e in allSeasons[selectedSeason].Episodes){
                                selectedEpisodes.Add(e);
                            }
                        }else{
                            foreach (Episode e in allSeasons[selectedSeason].Episodes){
                                selectedEpisodes.Remove(e);
                            }
                        }
                    }
                }
                selectedSeason = Math.Clamp(selectedSeason, 0, Math.Max(0, allSeasons.Count-1));
                selectedEpisode = Math.Clamp(selectedEpisode, 0, Math.Max(0, allSeasons[selectedSeason].Episodes.Count-1));
            } while(true);

            if(selectedEpisodes.Count == 0){
                return null;
            }else{
                List<string> episodes = new List<string>();
                for(int i=0;i<allSeasons.Count;i++){
                    Season s = allSeasons[i];
                    for(int j=0;j<s.Episodes.Count;j++){
                        Episode e = s.Episodes[j];
                        bool add = false;
                        foreach(Episode se in selectedEpisodes){
                            if (se == e){ add = true; }
                        }
                        if(add){episodes.Add($"{i}.{j}");}
                    }
                }
                return episodes;
            }
        }
    }
}