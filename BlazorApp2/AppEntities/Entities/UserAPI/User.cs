using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Entities.UserAPI
{
    public class User
    {
        [JsonConstructor]
        internal User(int id, string mail, string lastname, string firstname)
        {
            Id = id;
            Mail = mail;
            Lastname = lastname;
            Firstname = firstname;
        }

        public int Id { get; }
        public string Mail { get; }
        public string Lastname { get; }
        public string Firstname { get; }

        public int AdressId { get; }

        public string refreshToken { get; }

        public string? Role {  get; }

    }
}
