using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    public static class EnumHelper
    {
        public static T[] GetValues<T>()
        {
            if (!typeof(T).IsEnum)
                throw new MethodAccessException("Type parameter T must be an enum.");

            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }
    }
}
