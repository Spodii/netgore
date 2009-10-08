using System;
using System.Linq;
using NetGore;

namespace NetGore.Db
{
    /// <summary>
    /// Attribute for a class containing a query that is to be managed and invoked by the
    /// <see cref="DbControllerBase"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbControllerQueryAttribute : Attribute
    {
    }
}