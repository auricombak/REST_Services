using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRestServer.Models
{
    public class Commentaire
    {
        public long ID { get; set; }
        public int Isbn { get; set; }
        public int Id_abonne { get; set; }
        public string Content { get; set; }
    }
}