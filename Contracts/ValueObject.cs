using System.Linq;

namespace Altidude.Contracts
{
    public abstract class ValueObject
    {
        public override bool Equals(object obj)
        {
            if (!(obj is ValueObject))
                return false;

            if (obj.GetType() != GetType())
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            var publicProperties =
                GetType()
                    .GetProperties()
                    .Where(p => p.GetCustomAttributes(typeof(EqualsIgnoreAttribute), false).Length == 0);

            if (publicProperties == null || !publicProperties.Any())
                return true;

            return publicProperties.All(p =>
            {
                var value1 = p.GetValue(this, null);
                var value2 = p.GetValue(obj, null);
                return (value1 == null && value2 == null) || (value1 != null && value2 != null && value1.Equals(value2));
            });
        }

        public override int GetHashCode()
        {
            int hashCode = 31;
            bool changeMultiplier = false;
            int index = 1;

            var publicProperties = this.GetType().GetProperties();

            if ((object)publicProperties != null && publicProperties.Any())
            {
                foreach (var item in publicProperties)
                {
                    if (item.GetCustomAttributes(typeof(EqualsIgnoreAttribute), false).Any())
                        continue;

                    object value = item.GetValue(this, null);

                    if ((object)value != null)
                    {
                        hashCode = hashCode * ((changeMultiplier) ? 59 : 114) + value.GetHashCode();

                        changeMultiplier = !changeMultiplier;
                    }
                    else
                        hashCode = hashCode ^ (index * 13);
                }
            }

            return hashCode;
        }
    }

}
