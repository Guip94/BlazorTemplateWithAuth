using Domain.Entity;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Query.Adresses
{
    public class GetAdressByUserId : IQueryDefinition<Adress>
    {
        public GetAdressByUserId(int id)
        {
            userId = id;
        }

        public int userId { get; }
    }
}
