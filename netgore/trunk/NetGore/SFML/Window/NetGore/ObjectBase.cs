using System;
using System.Linq;

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
        IntPtr myThis = IntPtr.Zero;

        /// <summary>
        /// Construct the object from a pointer to the C library object
        /// </summary>
        /// <param name="thisPtr">Internal pointer to the object in the C libraries</param>
        ////////////////////////////////////////////////////////////
        protected ObjectBase(IntPtr thisPtr)
        {
            myThis = thisPtr;
        }

        ////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Access to the internal pointer of the object.
        /// For internal use only
        /// </summary>
        ////////////////////////////////////////////////////////////
        public virtual IntPtr This
        {
            get { return myThis; }
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

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Destroy the object (implementation is left to each derived class)
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        ////////////////////////////////////////////////////////////
        protected abstract void Destroy(bool disposing);

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
        /// Set the pointer to the internal object. For internal use only
        /// </summary>
        /// <param name="thisPtr">Pointer to the internal object in C library</param>
        ////////////////////////////////////////////////////////////
        protected void SetThis(IntPtr thisPtr)
        {
            myThis = thisPtr;
        }

        #region IDisposable Members

        /// <summary>
        /// Explicitely dispose the object
        /// </summary>
        ////////////////////////////////////////////////////////////
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}