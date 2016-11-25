using System.Collections.Generic;

namespace GoLocal.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
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