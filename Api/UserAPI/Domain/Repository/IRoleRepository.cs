using Domain.Entity;
using Domain.Query.Roles;
using LocalNuggetTools.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IRoleRepository :
        IQueryHandler<GetRoleByIdQuery, Role>,
        IQueryHandler<GetAllRoles, IEnumerable<Role>>
    {
    }
}
