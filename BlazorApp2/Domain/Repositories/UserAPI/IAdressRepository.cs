using Entities.Commands.UserAPICommands.AdressCommands;
using Entities.Entities.UserAPI;
using Entities.Queries.UserAPIQueries.AdressQueries;
using LocalNuggetTools.Commands;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.UserAPI
{
    public interface IAdressRepository:
        ICommandAsyncHandler<AddAdressCommand>,
        IQueryAsyncHandler<GetAdressQuery, Adress>,
        ICommandAsyncHandler<UpdateAdressCommand>
    {
    }
}
