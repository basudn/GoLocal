using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoLocal.Models
{
    public class Location
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public virtual List<Feed> FeedsList { get; set; }

        public Location()
        {
            FeedsList = new List<Feed>();
        }
    }
}