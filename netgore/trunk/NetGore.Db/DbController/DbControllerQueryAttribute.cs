using System;
using System.Linq;
using NetGore;

namespace NetGore.Db
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbControllerQueryAttribute : Attribute
    {
    }
}