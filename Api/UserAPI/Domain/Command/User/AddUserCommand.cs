using LocalNuggetTools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Command.User
{
    public class AddUserCommand : ICommandDefinition
    {
        public AddUserCommand(string mail, string pwd, string lastname, string firstname)
        {
            Mail = mail;
            Pwd = pwd;
            Lastname = lastname;
            Firstname = firstname;
        }

        public string Lastname { get; }
        public string Firstname { get; }
        public string Mail { get; }

        public string Pwd { get; }
    }
}
