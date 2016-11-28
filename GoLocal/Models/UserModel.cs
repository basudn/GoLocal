using System;
using System.Collections.Generic;

namespace GoLocal.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public List<FeedModel> Feeds { get; set; }
        public List<CommentModel> Comments { get; set; }
        public List<VoteModel> Votes { get; set; }
    }
}