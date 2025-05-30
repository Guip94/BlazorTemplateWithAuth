using Domain.Entity;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Query.Utilisateurs
{
    public class GetAllUser : IQueryDefinition<IEnumerable<User>>
    {
    }
}
