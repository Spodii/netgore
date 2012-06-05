using System;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Attribute for a class containing a query that is to be managed and invoked by the
    /// <see cref="DbControllerBase"/>. This is required to be placed on a class if you want
    /// to invoke it with the <see cref="DbControllerBase"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DbControllerQueryAttribute : Attribute
    {
    }
}