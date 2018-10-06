using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRestServer.Models
{
    public class Book
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Editor { get; set; }
        public int Isbn { get; set; }
        public int NbrExemplaires { get; set; }
        //public List<Comment> mComments { get; set; } Pour plus tard ! 
    }
}
