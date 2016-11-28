﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoLocal.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        [StringLength(12)]
        [RegularExpression("[0-9]{12}")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(1)]
        public string Status { get; set; }
        public virtual List<Feed> MyFeeds { get; set; }
        public virtual List<Comment> MyComments { get; set; }
        public virtual List<Vote> MyVotes { get; set; }

        public User()
        {
            MyFeeds = new List<Feed>();
            MyComments = new List<Comment>();
            MyVotes = new List<Vote>();
        }
    }
}