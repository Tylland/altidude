namespace Altidude.Contracts.Types
{
    public class PlaceAttribute
    {
        public PlaceAttributeType Type { get; set; }
        public bool DefaultValue { get; set; }

        public PlaceAttribute(PlaceAttributeType type, bool defaultValue)
        {
            Type = type;
            DefaultValue = defaultValue;
        }
        public PlaceAttribute()
        {

        }

        public static PlaceAttribute[] CreateDefault()
        {
            return new PlaceAttribute[] { new PlaceAttribute(PlaceAttributeType.EatStation, false), new PlaceAttribute(PlaceAttributeType.Coffee, false) };
        }
    }
}
