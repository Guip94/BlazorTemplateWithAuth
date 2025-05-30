using Domain.Entity;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Query.Utilisateurs
{
    public class LoginQuery : IQueryDefinition<User>
    {
        public LoginQuery(string mail, string pwd)
        {
            Mail = mail;
            Pwd = pwd;
        }

        public string Mail { get; }

        public string Pwd { get; }


    }
}
