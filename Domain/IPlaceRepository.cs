using Altidude.Contracts.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Domain
{
    public interface IPlaceRepository 
    {
        Place GetById(Guid id);
        void Add(Place place);
        void Remove(Guid id);
        void Clear();
    }
}
