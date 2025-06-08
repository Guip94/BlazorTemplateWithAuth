using LocalNuggetTools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Entities.Commands.UserAPICommands.AdressCommands.UpdateUserNamesCommand
{
    public class UpdateUserNamesCommand
        : ICommandDefinition
    {

        public int Id { get; }

        public string Lastname { get; }

        public string Firstname { get; }



        public UpdateUserNamesCommand(int id, string lastname, string firtname)
        {
            Id = id;
            Lastname = lastname;
            Firstname = firtname;
        }
    }
}
