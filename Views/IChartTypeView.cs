using System;
using Altidude.Contracts.Types;

namespace Altidude.Views
{
    public interface  IChartTypeView
    {
        ChartType GetById(Guid id);
    }
}
