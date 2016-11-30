using System;
using System.ComponentModel.DataAnnotations;

namespace GoLocal.Models
{
    public class Comment
    {
        public int ID { get; set; }
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Timestamp { get; set; }
        [Display(Name = "User ID")]
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual Feed Feed { get; set; }
        [Display(Name = "Feed ID")]
        public virtual int FeedID { get; set; }
    }
}