using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRestServer.Models
{
    public class Abonne
    {
        public long ID { get; set; }
        public int Identifiant { get; set; }
        public string Password { get; set; }
    }
}