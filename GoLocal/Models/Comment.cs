using System;

namespace GoLocal.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual Feed Feed { get; set; }
        public virtual int FeedID { get; set; }
    }
}