
using Altidude.Contracts.Types;
using System;

namespace Altidude.Domain
{
    public interface IUserService
    {
        User GetById(Guid id);
    }
}
