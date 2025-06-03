using LocalNuggetTools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Command.User
{
    public class UpdateUserLastname : ICommandDefinition
    {
        public UpdateUserLastname(int id, string lastname)
        {
            Id = id;
            Lastname = lastname;
        }

        public int Id { get; }
        public string Lastname { get; }
    }
}
