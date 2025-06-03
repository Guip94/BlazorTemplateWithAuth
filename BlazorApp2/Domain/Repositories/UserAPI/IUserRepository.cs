using Entities.Commands.UserAPICommands.UserCommands;
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
        ICommandAsyncHandler<UpdateUserFirstnameCommand>,
        ICommandAsyncHandler<UpdateUserLastnameCommand>,
        IQueryAsyncHandler<GetUserQuery, User>

    {
    }
}
