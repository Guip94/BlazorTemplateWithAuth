using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mappers
{
    internal static class ToAdressMapper
    {

        public static Adress ToAdress(this IDataRecord reader)
        {
            return new Adress
                (
                (int)reader["Id"],
                (string)reader["Country"],
                (int)reader["Zipcode"],
                (string)reader["City"],
                (string)reader["Street"]
                );
        }

    }
}
