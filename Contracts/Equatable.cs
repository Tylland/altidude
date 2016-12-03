using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Altidude.Contracts
{
    public static class Equatable
    {
        public static bool PropertiesAreEqual<T>(T first, T second)
        {
            if ((first == null && second != null) || (first != null && second == null))
                return false;

            if (ReferenceEquals(first, second))
                return true;

            var firstProperties = first.GetType().GetProperties(BindingFlags.DeclaredOnly);
            var secondProperties = second.GetType().GetProperties(BindingFlags.DeclaredOnly);

            if (firstProperties.Length != secondProperties.Length)
                return false;

            return firstProperties.All(p =>
            {
                var firstValue = p.GetValue(first, null);
                var secondValue = p.GetValue(second, null);

                return (firstValue == null && secondValue == null) ||
                       (firstValue != null && secondValue != null && firstValue.Equals(secondValue));
            });
        }

        public static bool ArraysAreEqual<T>(T[] first, T[] second)
        {
            if ((first == null && second != null) || (first != null && second == null))
                return false;

            if (ReferenceEquals(first, second))
                return true;

            if (first.Length != second.Length)
                return false;

            for (int i = 0; i < first.Length; i++)
            {
                if (!ValuesAreEqual(first[i], second[i]))
                    return false;
            }

            return true;
        }

        public static bool ArraysAreEqual(IList first, IList second)
        {
            if ((first == null && second != null) || (first != null && second == null))
                return false;

            if (ReferenceEquals(first, second))
                return true;

            if (first.Count != second.Count)
                return false;

            for (int i = 0; i < first.Count; i++)
            {
                if (!ValuesAreEqual(first[i], second[i]))
                    return false;
            }

            return true;
        }
        public static bool ValuesAreEqual<T>(T firstValue, T secondValue)
        {
            return (firstValue == null && secondValue == null) ||
                     (firstValue != null && secondValue != null && firstValue.Equals(secondValue));
        }
    }
}
