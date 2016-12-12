using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Domain.Aggregates.Profile
{
    public interface IChartTypeService
    {
        bool IsUnlocked();
    }
}
