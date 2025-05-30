using LocalNuggetTools.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Command.User
{
    public class UpdateUserMailCommand : ICommandDefinition
    {
        public UpdateUserMailCommand(int id, string pwd, string mail)
        {
            Id = id;
            Mail = mail;
            
            Pwd = pwd;
        }
        public int Id { get;}
        public string Pwd { get; }

        public string Mail { get; }
    }
}
