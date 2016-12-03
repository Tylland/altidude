using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Contracts.Events
{
    public abstract class EventBase<T> : IEvent, IEquatable<T> where T : EventBase<T>
    {
        public Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            T other = obj as T;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            IEnumerable<FieldInfo> fields = GetFields();

            int startValue = 17;
            int multiplier = 59;
            int hashCode = startValue;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(this);

                if (value != null)
                    hashCode = hashCode * multiplier + value.GetHashCode();
            }

            return hashCode;
        }

        public virtual bool Equals(T other)
        {
            if ((object)other == null)
                return false;

            Type t = GetType();
            Type otherType = other.GetType();

            if (t != otherType)
                return false;

            FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                object value1 = field.GetValue(other);
                object value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                        return false;
                }
                else {
                    if (t.IsArray)
                        if(!Equatable.ArraysAreEqual(value1 as IList, value2 as IList))
                            return false;

                    if (!value1.Equals(value2))
                         return false;
                }
            }

            return true;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            Type t = GetType();

            List<FieldInfo> fields = new List<FieldInfo>();

            while (t != typeof(object))
            {
                fields.AddRange(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));

                t = t.BaseType;
            }

            return fields;
        }

        public static bool operator ==(EventBase<T> x, EventBase<T> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(EventBase<T> x, EventBase<T> y)
        {
            return !(x == y);
        }
    }
}
