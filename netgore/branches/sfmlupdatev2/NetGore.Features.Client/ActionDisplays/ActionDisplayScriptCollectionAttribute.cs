using System;
using System.Linq;

namespace NetGore.Features.ActionDisplays
{
    /// <summary>
    /// Attribute used to denote a class that contains <see cref="ActionDisplayScriptAttribute"/>s. A class must
    /// use this attribute if the contained methods with a <see cref="ActionDisplayScriptAttribute"/> are to be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ActionDisplayScriptCollectionAttribute : Attribute
    {
    }
}