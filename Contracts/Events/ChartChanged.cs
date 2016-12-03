using Newtonsoft.Json;
using System;

namespace Altidude.Contracts.Events
{
    public class ChartChanged: IEvent
    {
        public Guid Id { get; set; }
        public Guid ChartId { get; set; }

        [JsonIgnore]
        public string Base64Image { get; set; }

        public ChartChanged(Guid id, Guid chartId, string base64Image)
        {
            Id = id;
            ChartId = chartId;
            Base64Image = base64Image;
        }
        public ChartChanged()
        {

        }
    }
}
