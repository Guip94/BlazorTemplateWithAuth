using LocalNuggetTools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Command.Adress
{
    public class AddAdressCommand : ICommandDefinition
    {
        public AddAdressCommand(string country, int zipcode, string city, string street, int userId)
        {
            Country = country;
            Zipcode = zipcode;
            City = city;
            Street = street;
            UserId = userId;
        }
        public int UserId { get; }
        public string Country { get; }

        public int Zipcode { get; }

        public string City { get; }

        public string Street { get; }
    }
}
