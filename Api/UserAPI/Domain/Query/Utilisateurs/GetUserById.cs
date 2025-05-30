using Domain.Entity;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Query.Utilisateurs
{
    public class GetUserById : IQueryDefinition<User>
    {
        public GetUserById(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
