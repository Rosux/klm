using Newtonsoft.Json;

namespace TimeLine{
    /// <summary>
    /// Holds a list of TimeLine items and has methods to add to this list.
    /// </summary>
    public class Holder
    {
        public List<Item> Items = new List<Item>();

        /// <summary>
        /// Creates a new holder.
        /// </summary>
        public Holder(){
            this.Items = new List<Item>();
        }

        /// <summary>
        /// Adds a given item to this TimeLine.
        /// </summary>
        /// <param name="item">The Item to add.</param>
        public void Add(Item item){
            this.Items.Add(item);
            this.Items = this.Items.OrderBy(item => item.StartTime).ToList();
        }

        /// <summary>
        /// Adds a given item to this TimeLine.
        /// </summary>
        /// <param name="action">Object to add.</param>
        /// <param name="startTime">Contains the starting time of the object.</param>
        /// <param name="endTime">Contains the ending time of the object.</param>
        public void Add(object action, DateTime startTime, DateTime endTime){
            Add(new Item(action, startTime, endTime));
        }

        /// <summary>
        /// Checks if there is a conflict with the given time range.
        /// </summary>
        /// <param name="startTime">Start time of the new item.</param>
        /// <param name="endTime">End time of the new item.</param>
        /// <returns>bool Conflict, string conflicting title (for testing purposes), string conflicting time ("From ... to ...)</returns>
        public (bool Conflict, string ConflictingTitle, string ConflictingString) HasConflict(DateTime startTime, DateTime endTime)
        {
            foreach (var item in Items)
            {
                if (item.Action is Film film )
                {
                    // Check if the new time range is also within any already added film 
                    if (startTime < item.EndTime && endTime > item.StartTime)
                    {
                        string ConflictingString = $"Movie {film.Title} is already scheduled from {item.StartTime.ToString("HH:mm")} to {item.EndTime.ToString("HH:mm")}";
                        return (true, film.Title, ConflictingString);
                    }
                }
                if (item.Action is Episode episode)
                {
                    if (startTime < item.EndTime && endTime > item.StartTime)
                    {
                        string ConflictingString = $"Episode {episode.Title} is already scheduled from {item.StartTime.ToString("HH:mm")} to {item.EndTime.ToString("HH:mm")}";
                        return (true, episode.Title, ConflictingString);
                    }
                }
            }
            return (false, null, null);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this.Items);
        }
    }
}