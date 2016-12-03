﻿using Altidude.Contracts.Types;
using Altidude.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Infrastructure
{
    public class InMemoryChartTypeView : IChartTypeView
    {
        private Dictionary<Guid, ChartType> _chartTypes = new Dictionary<Guid, ChartType>();

        public ChartType GetById(Guid id)
        {
            if (_chartTypes.ContainsKey(id))
                return _chartTypes[id];

            return null;
        }

        public InMemoryChartTypeView()
        {
            Add(new ChartType("33F0009E-ABC8-4CD6-8799-979B7BF5CA6F", "The Forest", "{0} has visited the forest", "{1}"));
            Add(new ChartType("19930022-DDB3-4CFC-A75E-3E8CC2DEEB04", "Simply Sunny", "{1}", "{2}"));
            Add(new ChartType("57B271BD-CA75-42BD-B7FD-A5A0EBEC887F", "Connecting Dots", "{0} has connected the dots", "{1}"));
            Add(new ChartType("28D33FB8-BEFC-41B3-B947-A0B9B6A811EB", "Mountain Silhouette", "{1}", "{2}"));
        }

        private void Add(ChartType chartType)
        {
            _chartTypes.Add(chartType.Id, chartType);
        }
    }
}
