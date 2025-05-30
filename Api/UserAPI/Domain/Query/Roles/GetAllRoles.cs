using Domain.Entity;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Query.Roles
{
    public class GetAllRoles : IQueryDefinition<IEnumerable<Role>>
    {
    }
}
