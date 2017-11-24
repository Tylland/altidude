using System;
using System.Collections.Generic;
using Altidude.Contracts.Types;

namespace Altidude.Views
{
    public interface  IChartTypeView
    {
        ChartType GetById(Guid id);
        UserChartType[] GetForUser(int level, DateTime now);
        List<ChartType> GetAll();
    }
}
