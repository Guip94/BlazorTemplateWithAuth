using Entities.Entities.UserAPI;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Queries.UserAPIQueries.AdressQueries
{
    public class GetAdressQuery : IQueryDefinition<Adress>
    {
        public int? UserId { get; }
        public GetAdressQuery(int userId)
        {
            UserId = userId;
        }

    }
}
