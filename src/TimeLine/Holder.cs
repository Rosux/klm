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
        /// <returns>True if there is a conflict, otherwise false.</returns>
        public bool HasConflict(DateTime startTime, DateTime endTime)
        {
            foreach (var item in Items)
            {
                if (item.Action is Film)
                {
                    // Check if the new time range is also within any already added film 
                    if (startTime < item.EndTime && endTime > item.StartTime)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this.Items);
        }
    }
}