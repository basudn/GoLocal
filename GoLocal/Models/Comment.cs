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
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TimeStamp { get; set; }
        public virtual int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual Feed Feed { get; set; }
        public virtual int FeedID { get; set; }
    }
}