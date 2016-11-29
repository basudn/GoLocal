using System;
using System.Collections.Generic;

namespace GoLocal.Models
{
    public class FeedModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LocationName { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public List<CommentModel> Comments { get; set; }
        public List<VoteModel> Votes { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }
    }
}