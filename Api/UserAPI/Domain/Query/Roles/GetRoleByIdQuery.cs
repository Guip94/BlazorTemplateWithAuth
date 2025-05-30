using Domain.Entity;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Query.Roles
{
    public class GetRoleByIdQuery : IQueryDefinition<Role>
    {
        public GetRoleByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get;  }
    }
}
