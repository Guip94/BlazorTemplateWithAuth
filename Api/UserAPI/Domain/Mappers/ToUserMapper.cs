using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mappers
{
    internal static class ToUserMapper
    {

        public static User ToUser(this IDataRecord reader)
        {
            return new User
                (
                (int)reader["Id"],
                (string)reader["Mail"],
                (string)reader["Lastname"],
                (string)reader["Firstname"]
                );
           
        }
        public static User ToAllUser(this IDataRecord reader)
        {
            return new User
                (
                (int)reader["Id"],
                (string)reader["Lastname"],
                (string)reader["Firstname"],
                 reader["AdressId"] is DBNull? null : (int)reader["AdressId"]
                );
        }
        public static User ToUserRole (this IDataRecord reader)
        {
            return new User
                (
                    (string)reader["Fonction"]
                );
        }

        public static User ToUserToken(this IDataRecord reader)
        {
            return new User
            (
                (int)reader["Id"],
                (string)reader["Mail"],
                (string)reader["Lastname"],
                (string)reader["Firstname"],
                (string)reader["RefreshToken"]
            );
        }

        public static User ToUserById(this IDataRecord reader)
        {
            return new User
                (
                    (string)reader["Mail"],
                    (string)reader["Lastname"],
                    (string)reader["Firstname"]
                );
        }
     
       
    }
}
