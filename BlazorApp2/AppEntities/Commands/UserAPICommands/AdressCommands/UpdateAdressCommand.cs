using LocalNuggetTools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Entities.Commands.UserAPICommands.AdressCommands
{
    public class UpdateAdressCommand : ICommandDefinition
    {
        public UpdateAdressCommand(int id, string country, int zipcode, string city, string street)
        {
            Id = id;
            Country = country;
            Zipcode = zipcode;
            City = city;
            Street = street;
        }

        public int Id { get; }
        public string Country { get; }
        public int Zipcode { get; }
        public string City { get; }
        public string Street { get; }


 
    }
}
