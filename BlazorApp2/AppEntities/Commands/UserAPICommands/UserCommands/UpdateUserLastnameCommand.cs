using LocalNuggetTools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Entities.Commands.UserAPICommands.UserCommands
{
    public class UpdateUserLastnameCommand : ICommandDefinition
    {

        public int Id { get; }

        public string Lastname { get; }

        public UpdateUserLastnameCommand(int id, string lastname)
        {
            Id = id;
            Lastname = lastname;
        }
    }
}
