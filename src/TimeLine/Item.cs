namespace TimeLine{
    /// <summary>
    /// Holds an action and the times at wich this starts and end.
    /// </summary>
    public class Item{
        public object Action { get; set; }
        public DateTime StartTime = new DateTime();
        public DateTime EndTime = new DateTime();

        /// <summary>
        /// Creates a new TimeLine Item.
        /// </summary>
        /// <param name="action">An object to hold. For example: Consumption, Film and Serie.</param>
        /// <param name="startTime">Contains the starting time of the object.</param>
        /// <param name="endTime">Contains the ending time of the object.</param>
        public Item(object action, DateTime startTime, DateTime endTime){
            this.Action = action;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }
    }
}