using System;

namespace GoLocal.Models
{
    public class VoteModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public int FeedID { get; set; }
        public string FeedTitle { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}