using LocalNuggetTools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Commands.UserAPICommands.UserCommands
{
    public class UpdateUserFirstname : ICommandDefinition
    {
        public int Id { get; }

        public string Firstname { get; }

        public UpdateUserFirstname(int id, string firstname)
        {
            Id = id;
            Firstname = firstname;
        }
    }
}
