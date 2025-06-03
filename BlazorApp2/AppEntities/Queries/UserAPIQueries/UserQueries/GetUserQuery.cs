using Entities.Entities.UserAPI;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Queries.UserAPIQueries.UserQueries
{
    public class GetUserQuery : IQueryDefinition<User>
    {
        public int Id { get; }
        public GetUserQuery(int id)
        {
            Id = id;
        }
    }
}
