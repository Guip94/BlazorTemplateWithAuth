using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
namespace Domain.Query.Utilisateurs
{
    public class GetUserRoleQuery : IQueryDefinition<User>
    {
        public GetUserRoleQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }

    }
}
