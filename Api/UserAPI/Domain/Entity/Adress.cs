using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Adress
    {
    

        public Adress(string country, int zipcode, string city, string street)
        {
            Country = country;
            Zipcode = zipcode;
            City = city;
            Street = street;
        }

        public Adress(int id, string country, int zipcode, string city, string street)
        {
            Id = id;
            Country = country;
            Zipcode = zipcode;
            City = city;
            Street = street;
        }

        public int Id { get; set; }
        public string Country { get; }

        public int Zipcode { get; }

        public string City { get; }

        public string Street { get; }
    }
}
