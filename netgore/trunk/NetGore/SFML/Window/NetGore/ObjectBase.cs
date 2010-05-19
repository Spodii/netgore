using System;
using System.Runtime.InteropServices;

namespace SFML
{
    ////////////////////////////////////////////////////////////
    /// <summary>
    /// The ObjectBase class is an abstract base for every
    /// SFML object. It's meant for internal use only
    /// </summary>
    ////////////////////////////////////////////////////////////
    public abstract class ObjectBase : IDisposable
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct the object from a pointer to the C library object
        /// </summary>
        /// <param name="thisPtr">Internal pointer to the object in the C libraries</param>
        ////////////////////////////////////////////////////////////
        protected ObjectBase(IntPtr thisPtr)
        {
            myThis = thisPtr;
        }

        /// <summary>
        /// Gets if this object has been disposed. For objects that will automatically reload after being disposed (such as content
        /// loaded through a ContentManager), this will always return true since, even if the object is disposed at the time the
        /// call is made to the method, it will reload automatically when needed. Such objects are often variations of existing
        /// objects, prefixed with the word "Lazy" (e.g. Image and LazyImage).
        /// </summary>
        public virtual bool IsDisposed
        {
            get { return This == IntPtr.Zero; }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Dispose the object
        /// </summary>
        ////////////////////////////////////////////////////////////
        ~ObjectBase()
        {
            Dispose(false);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Access to the internal pointer of the object.
        /// For internal use only
        /// </summary>
        ////////////////////////////////////////////////////////////
        public virtual IntPtr This
        {
            get {return myThis;}
        }

        /// <summary>
        /// Access to the internal pointer of the object. Returns the pointer, never anything more. Cannot override.
        /// For internal use only
        /// </summary>
        protected IntPtr ThisRaw
        {
            get { return myThis; }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Explicitely dispose the object
        /// </summary>
        ////////////////////////////////////////////////////////////
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Destroy the object
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        ////////////////////////////////////////////////////////////
        protected void Dispose(bool disposing)
        {
            if (myThis != IntPtr.Zero)
            {
                Destroy(disposing);
                myThis = IntPtr.Zero;
            }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Destroy the object (implementation is left to each derived class)
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        ////////////////////////////////////////////////////////////
        protected abstract void Destroy(bool disposing);

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the pointer to the internal object. For internal use only
        /// </summary>
        /// <param name="thisPtr">Pointer to the internal object in C library</param>
        ////////////////////////////////////////////////////////////
        protected void SetThis(IntPtr thisPtr)
        {
            myThis = thisPtr;
        }

        private IntPtr myThis = IntPtr.Zero;
    }
}
