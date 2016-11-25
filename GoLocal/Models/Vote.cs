using System;

namespace GoLocal.Models
{
    public class Vote
    {
        public int ID { get; set; }
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
        public string type { get; set; }
        public virtual int FeedID { get; set; }
        public virtual Feed Feed { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}