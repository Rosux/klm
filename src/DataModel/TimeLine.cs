namespace TimeLine{
    public class Holder
    {
        private List<Item> t = new List<Item>();

        public Holder(){
            this.t = new List<Item>();
        }

        public void Add(Item item){
            this.t.Add(item);
        }
        public void Add(object action, DateTime startTime, DateTime endTime){
            this.t.Add(new Item(action, startTime, endTime));
        }
    }

    public class Item{
        public object Action { get; set; }
        public DateTime StartTime = new DateTime();
        public DateTime EndTime = new DateTime();

        public Item(object action, DateTime startTime, DateTime endTime){
            this.Action = action;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }
    }
}