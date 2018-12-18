using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFML
{
    public abstract partial class ObjectBase
    {
        /// <summary>
        /// Access to the internal pointer of the object. Returns the pointer, never anything more. Cannot override.
        /// For internal use only
        /// </summary>
        protected IntPtr CPointerRaw
        {
            get { return myCPointer; }
        }

        /// <summary>
        /// Gets if this object has been disposed. For objects that will automatically reload after being disposed (such as content
        /// loaded through a ContentManager), this will always return true since, even if the object is disposed at the time the
        /// call is made to the method, it will reload automatically when needed. Such objects are often variations of existing
        /// objects, prefixed with the word "Lazy" (e.g. Image and LazyImage).
        /// </summary>
        public virtual bool IsDisposed
        {
            get { return CPointer == IntPtr.Zero; }
        }
    }
}
