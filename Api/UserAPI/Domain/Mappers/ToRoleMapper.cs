using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mappers
{
    internal static class ToRoleMapper
    {
        public static Role ToRole(this IDataRecord reader)
        {
            return new Role
                (
                (int)reader["Id"],
                (string)reader["Fonction"]
                );
        }
        public static Role UserToRole(this IDataRecord reader)
        {
            return new Role
                (
                (string)reader["Fonction"]
                );
        }
    }
}
