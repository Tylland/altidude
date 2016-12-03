namespace Altidude.Contracts.Types
{
    public class ProfilePlaceAttribute
    {
        public PlaceAttributeType Type { get; set; }
        public bool Value { get; set; }

        public ProfilePlaceAttribute(PlaceAttributeType type, bool value)
        {
            Type = type;
            Value = value;
        }
        public ProfilePlaceAttribute()
        {

        }
    }
}
