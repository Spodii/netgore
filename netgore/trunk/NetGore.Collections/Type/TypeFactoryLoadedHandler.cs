using System;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Handles when a new type has been loaded into a <see cref="TypeFactory"/>.
    /// </summary>
    /// <param name="typeFactory"><see cref="TypeFactory"/> that the event occured on.</param>
    /// <param name="loadedType">Type that was loaded.</param>
    /// <param name="name">Name of the Type.</param>
    public delegate void TypeFactoryLoadedHandler(TypeFactory typeFactory, Type loadedType, string name);
}