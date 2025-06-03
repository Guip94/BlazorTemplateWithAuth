using Domain.Command.User;
using Domain.Entity;
using Domain.Query;
using Domain.Query.Utilisateurs;
using LocalNuggetTools.Commands;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IUserRepository :
        ICommandHandler<AddUserCommand>,
        ICommandHandler<UpdateUserMailCommand>,
        ICommandHandler<InsertRTokenToUserCommand>,
        IQueryHandler<LoginQuery, User>,
        IQueryHandler<GetAllUser, IEnumerable<User>>,
        IQueryHandler<GetUserRoleQuery, User>,
        IQueryHandler<GetUserByIdWithToken, User>,
        IQueryHandler<GetUserFromTokenQuery, User>,
        IQueryHandler<GetUserById, User>,
        ICommandHandler<UpdateUserFirstname>,
        ICommandHandler<UpdateUserLastname>


    {
    }
}
