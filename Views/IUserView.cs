using Altidude.Contracts.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Views
{
    public interface IUserView
    {
        User GetById(Guid id);
        List<User> GetAll();
    }
}
