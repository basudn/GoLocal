using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoLocal.Models
{
    public class Feed
    {
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Timestamp { get; set; }
        [Display(Name = "User ID")]
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
        [Display(Name = "Location Name")]
        [StringLength(100)]
        public string LocationName { get; set; }
        [Required]
        [Range(-90,90)]
        public double Lat { get; set; }
        [Required]
        [Range(-180,180)]
        public double Long { get; set; }
        [StringLength(1)]
        public string Status { get; set; }
        public virtual List<Comment> FeedComments { get; set; }
        public virtual List<Vote> FeedVotes { get; set; }

        public Feed()
        {
            FeedComments = new List<Comment>();
            FeedVotes = new List<Vote>();
        }

        public int Upvote()
        {
            int upvote = 0;
            foreach (Vote vote in FeedVotes)
            {
                if (vote.Type == "U")
                {
                    upvote++;
                }
            }
            return upvote;
        }

        public int Downvote()
        {
            int downvote = 0;
            foreach (Vote vote in FeedVotes)
            {
                if (vote.Type == "D")
                {
                    downvote++;
                }
            }
            return downvote;
        }
    }
}