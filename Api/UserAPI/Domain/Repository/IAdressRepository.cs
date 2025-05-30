using Domain.Command.Adress;
using Domain.Entity;
using Domain.Query.Adresses;
using LocalNuggetTools.Commands;
using LocalNuggetTools.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IAdressRepository :
        ICommandHandler<AddAdressCommand>,
        ICommandHandler<UpdateAdressCommand>,
        IQueryHandler<GetAdressByUserId, Adress>
    {
    }
}
