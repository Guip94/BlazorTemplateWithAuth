using Domain.Entity;
using LocalNuggetTools.Commands;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Query.Utilisateurs
{
    public class GetUserByIdWithToken : IQueryDefinition<User>
    {
        public GetUserByIdWithToken(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
