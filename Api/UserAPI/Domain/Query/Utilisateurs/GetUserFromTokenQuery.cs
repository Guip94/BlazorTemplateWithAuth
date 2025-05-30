using Domain.Entity;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Query.Utilisateurs
{
    public class GetUserFromTokenQuery : IQueryDefinition<User>
    {

        public string RefreshToken { get; }

        public GetUserFromTokenQuery(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
