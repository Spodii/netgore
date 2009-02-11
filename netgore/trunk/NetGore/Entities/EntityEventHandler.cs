using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Delegate for handling Entity events
    /// </summary>
    /// <param name="entity">Entity this event came from</param>
    public delegate void EntityEventHandler(Entity entity);

    /// <summary>
    /// Delegate for handling Entity events
    /// </summary>
    /// <param name="entity">Entity this event came from</param>
    /// <param name="value">Value related to this event</param>
    public delegate void EntityEventHandler<T>(Entity entity, T value);
}