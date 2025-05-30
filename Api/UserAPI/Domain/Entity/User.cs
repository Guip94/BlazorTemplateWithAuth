using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class User
    {
        public User(string role)
        {
            Role = role;
        }

        internal User(int id, string mail, string lastname, string firstname, string refreshToken)
        {
            Id = id;
            Mail = mail;
            Lastname = lastname;
            Firstname = firstname;
            RefreshToken = refreshToken;
        }

        internal User(int id, string refreshToken)
        {
            Id = id;
            RefreshToken = refreshToken;
        }

        internal User(int id, string lastname, string firstname)
        {
            Id = id;
            Lastname = lastname;
            Firstname = firstname;
        }

        internal User(int id, string mail, string lastname, string firstname )
        {
            Id = id;
            Mail = mail;
            Lastname = lastname;
            Firstname = firstname;
        }
        internal User(int id , string lastname, string firstname, int? adressid)
        {
            Id = id;
            Lastname = lastname;
            Firstname = firstname;
            AdressId = adressid;
        }

        internal User(string mail, string lastname, string firstname, int? adressid)
        {
            
            Mail = mail;
            Lastname = lastname;
            Firstname = firstname;
            AdressId = adressid;

        }





        public int Id { get; }
        public string Mail { get; }
        public string Lastname { get; }
        public string Firstname { get; }
        public string? RefreshToken { get; }
        public int? AdressId { get; }
        public string Role { get; }
        
    }
}
