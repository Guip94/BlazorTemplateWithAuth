using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Role
    {
        public Role(string fonction)
        {
            Fonction = fonction;
        }

        public Role(int id, string fonction)
        {
            Id = id;
            Fonction = fonction;
        }

        public int Id { get;  }
        public string Fonction { get;  }
    }
}
