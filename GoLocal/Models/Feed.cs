using System;
using System.Collections.Generic;

namespace GoLocal.Models
{
    public class Feed
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual int LocationID { get; set; }
        public virtual Location Location { get; set; }
        public virtual List<Comment> FeedComments { get; set; }
        public virtual List<Vote> FeedVotes { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }

        public Feed()
        {
            FeedComments = new List<Comment>();
            FeedVotes = new List<Vote>();
        }
    }
}