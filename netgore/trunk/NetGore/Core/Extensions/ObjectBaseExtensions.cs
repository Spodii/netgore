using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML;

namespace NetGore
{
    public static class ObjectBaseExtensions
    {
        /// <summary>
        /// Gets if an <see cref="ObjectBase"/> has been disposed.
        /// </summary>
        /// <param name="obj">The <see cref="ObjectBase"/> to check if disposed.</param>
        /// <returns>True if the <paramref name="obj"/> has been disposed; otherwise false.</returns>
        public static bool IsDisposed(this ObjectBase obj)
        {
            try
            {
                return obj.This == IntPtr.Zero;
            }
            catch (LoadingFailedException)
            {
                return true;
            }
        }
    }
}
