using Altidude.Contracts.Types;
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

        private static readonly DateTime NoDate = new DateTime(2000, 01, 01);

        public ChartType GetById(Guid id)
        {
            if (_chartTypes.ContainsKey(id))
                return _chartTypes[id];

            return null;
        }

        public InMemoryChartTypeView()
        {
            Add(new ChartType("33F0009E-ABC8-4CD6-8799-979B7BF5CA6F", "The Forest", "{0} has visited the forest", "{1}", 1, NoDate));
            Add(new ChartType("19930022-DDB3-4CFC-A75E-3E8CC2DEEB04", "Simply Sunshine", "{1}", "{2}", 1, NoDate));
            Add(new ChartType("57B271BD-CA75-42BD-B7FD-A5A0EBEC887F", "Connecting Dots", "{0} has connected the dots", "{1}", 1, NoDate));
            //Add(new ChartType("28D33FB8-BEFC-41B3-B947-A0B9B6A811EB", "Mountain Silhouette", "{1}", "{2}", 2, NoDate));
            //Add(new ChartType("E6C5D286-BF69-4FD0-A6DE-F46ACC53F011", "Giro d'Italia", "{1}", "{2}", 2, NoDate));
            Add(new ChartType("614483F0-5B42-41B3-939E-24C4BD1660F8", "Northern Zodiac", "{1}", "{2}", 1, NoDate));
            Add(new ChartType("507AF72C-5678-4AFC-AB43-AB7DC34D904E", "Southern Zodiac", "{1}", "{2}", 2, NoDate));
        }
        private void Add(ChartType chartType)
        {
            _chartTypes.Add(chartType.Id, chartType);
        }

        public UserChartType[] GetUserChartTypes(int level, DateTime now)
        {
            return _chartTypes.Values.Select(chartType => new UserChartType(chartType, chartType.GetIsUnlocked(level, now))).ToArray();
        }
    }
}
