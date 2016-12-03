using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Contracts
{
    public static class Extensions
    {
        public static T MinObject<T>(this IEnumerable<T> objects, Func<T, double> valueFunc)
        {
            T minObject = default(T);
            double minValue = double.MaxValue;

            foreach (var obj in objects)
            {
                var value = valueFunc(obj); 

                if (value < minValue)
                {
                    minValue = value;
                    minObject = obj;
                }
            }

            return minObject;
        }
    }
}
