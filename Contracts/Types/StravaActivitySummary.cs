namespace Altidude.Contracts.Types
{
    public class StravaActivitySummary
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }

        public StravaActivitySummary(long id, string type, string name, string startDate)
        {
            Id = id;
            Type = type;
            Name = name;
            StartDate = startDate;
        }
        public StravaActivitySummary()
        {

        }
    }
}
