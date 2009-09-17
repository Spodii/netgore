using System;
using System.Linq;

namespace NetGore.Db
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbControllerQueryAttribute : Attribute
    {
    }
}