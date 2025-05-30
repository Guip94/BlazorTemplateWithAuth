using BStorm.Tools.Database;
using Domain.Command.Adress;
using Domain.Entity;
using Domain.Mappers;
using Domain.Query.Adresses;
using Domain.Repository;
using LocalNuggetTools.ResultPattern;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service
{

    public class AdressService : IAdressRepository
    {
         private readonly DbConnection _connection;

        public AdressService(DbConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        public CommandResult Execute(AddAdressCommand command)
        {

            try
            {



                int rslt = _connection.ExecuteNonQuery("[DbUserStandard].[AddAdress]", true, command);
                if (rslt != 2) { return CommandResult.Failure("Echec de la create command"); }
                return CommandResult.Success();

            }
            catch (Exception ex) {  return CommandResult.Failure(ex.Message, ex); }
        }

        public CommandResult Execute(UpdateAdressCommand command)
        {
            try
            {

                int rslt = _connection.ExecuteNonQuery("[DbUserStandard].[UpdateAdress]", true, command);
                if (rslt != 1) { return CommandResult.Failure("Echec de l'update command"); }
                return CommandResult.Success();

            }
            catch (Exception ex) { return CommandResult.Failure(ex.Message, ex); }
        }

     

        public QueryResult<Adress> Execute(GetAdressByUserId query)
        {
            try
            {
                Adress? rslt = _connection.ExecuteReader("[DbUserStandard].[GetAdressByUserId]", sp => sp.ToAdress(), true, query).SingleOrDefault();
                if(rslt is null) { return QueryResult<Adress>.Failure("Echec de la query"); }
                return QueryResult<Adress>.Success(rslt);

            }
            catch (Exception ex)
            {
                return QueryResult<Adress>.Failure(ex.Message, ex);
            }
        }
    }
}
