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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this.Items);
        }
    }
}