using System;
using System.ComponentModel.DataAnnotations;

namespace GoLocal.Models
{
    public class Vote
    {
        public int ID { get; set; }
        [Display(Name = "User ID")]
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
        [Required]
        [StringLength(1)]
        public string Type { get; set; }
        [Display(Name = "Feed ID")]
        public virtual int FeedID { get; set; }
        public virtual Feed Feed { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Timestamp { get; set; }
    }
}