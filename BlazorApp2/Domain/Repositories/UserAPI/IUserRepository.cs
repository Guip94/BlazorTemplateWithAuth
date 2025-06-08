using Entities.Commands.UserAPICommands.AdressCommands.UpdateUserNamesCommand;
using Entities.Entities.UserAPI;
using Entities.Queries.UserAPIQueries.UserQueries;
using LocalNuggetTools.Commands;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.UserAPI
{
    public interface IUserRepository :
        ICommandAsyncHandler<UpdateUserNamesCommand>,
        IQueryAsyncHandler<GetUserQuery, User>

    {
    }
}
