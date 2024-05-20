using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Klubovi.Models
{
    public class Club
    {
        private string name;
        private string city;
        private bool active;
        private int points;

        public Club(string name, string city, bool active)
        {
            this.name = name;
            this.city = city;
            this.active = active;
            this.points = 0;
        }

        public string Name { get => name; set => name = value; }
        public string City { get => city; set => city = value; }
        public bool Active { get => active; set => active = value; }
        public int Points { get => points; set => points = value; }

        public override bool Equals(object obj)
        {
            return obj.Equals(this.Name);
        }
    }
}