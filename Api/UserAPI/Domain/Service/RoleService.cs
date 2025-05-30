using BStorm.Tools.Database;
using Domain.Entity;
using Domain.Mappers;
using Domain.Query.Roles;
using Domain.Repository;
using LocalNuggetTools.Queries;
using LocalNuggetTools.ResultPattern;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class RoleService : IRoleRepository
    {
        private readonly DbConnection _connection;

        public RoleService(DbConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        public QueryResult<Role> Execute(GetRoleByIdQuery query)
        {
            try
            {
                //[dbo] empêchera le fonctionnement de la query

                Role? rslt = _connection.ExecuteReader("[dbo].[GetRoleById]", sp => sp.ToRole(), true, query).SingleOrDefault();

                if (rslt is null) { return QueryResult<Role>.Failure("L'id n'existe pas"); }

                return QueryResult<Role>.Success(rslt);

            }
            catch (Exception ex) { return QueryResult<Role>.Failure(ex.Message, ex); }
        }

        public QueryResult<IEnumerable<Role>> Execute(GetAllRoles query)
        {
            try
            {
                IEnumerable<Role> rslt = _connection.ExecuteReader("[DbUserStandard].[GetAllRoles]", sp => sp.ToRole(), true, query).ToList();

               return QueryResult<IEnumerable<Role>>.Success(rslt);


            }
            catch (Exception ex) { return QueryResult<IEnumerable<Role>>.Failure(ex.Message, ex); }
        }

     
    }
}
