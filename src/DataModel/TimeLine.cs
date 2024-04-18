using Newtonsoft.Json;

namespace TimeLine{
    public class Holder
    {
        public List<Item> t = new List<Item>();

        public Holder(){
            this.t = new List<Item>();
        }

        public void Add(Item item){
            this.t.Add(item);
            this.t = this.t.OrderBy(item => item.StartTime).ToList();
        }
        public void Add(object action, DateTime startTime, DateTime endTime){
            Add(new Item(action, startTime, endTime));
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this.t);
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