using System;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Handles events from the <see cref="DbControllerQueryAttributeChecker"/>.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="type">The type.</param>
    public delegate void DbControllerQueryAttributeCheckerEventHandler(DbControllerQueryAttributeChecker sender, Type type);
}