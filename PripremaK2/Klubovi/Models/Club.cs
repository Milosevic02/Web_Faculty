using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klubovi.Models
{
    public class Club
    {
        public string Name { get; set; }
        public string City { get; set; }
        public bool Active { get; set; }
        public int Points { get; set; }

        public Club() {
            Name = "";
            City = "";
            Active = false;
            Points = 0;
        }

        public Club(string name, string city, bool active)
        {
            Name = name;
            City = city;
            Active = active;
            Points = 0;
        }

        public override bool Equals(object obj)
        {
            return obj.Equals(this.Name);
        }
    }
}