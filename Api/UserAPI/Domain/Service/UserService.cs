using BStorm.Tools.Database;
using Domain.Command.User;
using Domain.Entity;
using Domain.Mappers;
using Domain.Query;
using Domain.Query.Utilisateurs;
using Domain.Repository;
using LocalNuggetTools.ResultPattern;
using System.Data.Common;


namespace Domain.Service
{
    public class UserService : IUserRepository
    {
        private readonly DbConnection _connection;

        public UserService(DbConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        public CommandResult Execute(AddUserCommand command)
        {
            try
            {
            int rslt = _connection.ExecuteNonQuery("[DbUserStandard].[AddUser]", true, command);
            if (rslt != 1) { return CommandResult.Failure("Erreur dans l'encodage de la command"); }
            return CommandResult.Success();


            }
            catch (Exception ex) { return CommandResult.Failure(ex.Message , ex); }

        }

        public QueryResult<User> Execute(LoginQuery query)
        {
            try
            {

                User? rslt = _connection.ExecuteReader("[DbUserStandard].[GetUser]", sp => sp.ToUser(), true, query).SingleOrDefault();
                if (rslt is null) { return QueryResult<User>.Failure("l'adresse ou le mail ne correspondent pas"); }


                return QueryResult<User>.Success(rslt);



            }
            catch (Exception ex) {  return QueryResult<User>.Failure (ex.Message , ex); }

        }

        public QueryResult<IEnumerable<User>> Execute(GetAllUser query)
        {

            try
            {
                IEnumerable<User> rslt = _connection.ExecuteReader("[DbUserStandard].[GetAllUser]", sp => sp.ToAllUser(), true, query).ToList();

                return QueryResult<IEnumerable<User>>.Success(rslt);


            }
            catch (Exception ex) { return QueryResult<IEnumerable<User>>.Failure(ex.Message, ex); }
        }

        public QueryResult<User> Execute(GetUserRoleQuery query)
        {
            try
            {

                User? rslt = _connection.ExecuteReader("[DbUserStandard].[GetUserRole]", sp => sp.ToUserRole(), true, query).SingleOrDefault();

                if (rslt is null) { return QueryResult<User>.Failure("Id non existant"); }
                else 
                {
                    return QueryResult<User>.Success(rslt);
                }
            }
            catch (Exception ex) { return QueryResult<User>.Failure(ex.Message, ex); }

            }

        public CommandResult Execute(UpdateUserMailCommand command)
        {
            try
            {

                int rslt = _connection.ExecuteNonQuery("[DbUserStandard].[UpdateUserMail]", true, command);
                if (rslt != 1) { return CommandResult.Failure("Echec de la command"); }

                return CommandResult.Success();

            }
            catch (Exception ex) { return CommandResult.Failure(ex.Message, ex); }
        }

        public QueryResult<User> Execute(GetUserByIdWithToken query)
        {
            try
            {
                User? rslt = _connection.ExecuteReader("[DbUserStandard].[GetUserByIdWithToken]", sp => sp.ToUserToken(), true, query).SingleOrDefault();
                if (rslt is null) { QueryResult<User>.Failure("Erreur de requête"); }

                return QueryResult<User>.Success(rslt);


            }
            catch (Exception ex) { return QueryResult<User>.Failure(ex.Message, ex); }
        }

        public QueryResult<User> Execute(GetUserFromTokenQuery query)
        {
            try
            {
            User rslt = _connection.ExecuteReader("[DbUserStandard].[GetUserFromRToken]", sp=>sp.ToUserRole(), true, query).SingleOrDefault()!;
                if (rslt is null) { return QueryResult<User>.Failure("User non trouvé"); }

                return QueryResult<User>.Success(rslt);
            }
            catch (Exception ex) { return QueryResult<User>.Failure(ex.Message, ex); }
        }

        public CommandResult Execute(InsertRTokenToUserCommand command)
        {
            try
            {
                int rslt = _connection.ExecuteNonQuery("[DbUserStandard].[InsertRTokenToUser]", true, command);
                if (rslt != 1) { return CommandResult.Failure("command failure"); }
                return CommandResult.Success();

            }
            catch (Exception ex)
            {
                return CommandResult.Failure(ex.Message, ex);
            }
        }

        public QueryResult<User> Execute(GetUserById query)
        {
            try
            {
                User? rslt = _connection.ExecuteReader("[DbUserStandard].[GetUserById]", sp => sp.ToUserById(), true, query).SingleOrDefault();
                if (rslt is null) { QueryResult<User>.Failure("Erreur de requête"); }

                return QueryResult<User>.Success(rslt!);


            }
            catch (Exception ex) { return QueryResult<User>.Failure(ex.Message, ex); }
        }

        public CommandResult Execute(UpdateUserNamesCommand command)
        {
            try
            {
                int rslt = _connection.ExecuteNonQuery("[DbUserStandard].[UpdateUserNames]", true, command);
                if (rslt is not 1) { CommandResult.Failure(" Update command failure"); }

                return CommandResult.Success();

            }
            catch (Exception ex)
            {
                return CommandResult.Failure(ex.Message, ex);
            }
        }

      
    }
}
