using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.IO;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Extension methods for the <see cref="Control"/>.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Gets all of the <see cref="Control"/>s from the given <paramref name="root"/>.
        /// </summary>
        /// <param name="root">The root <see cref="Control"/> to search from.</param>
        /// <param name="filter">A filter that determines what <see cref="Control"/>s to return. If null, it will
        /// be treated as if it were to always return true.</param>
        /// <returns>All of the <see cref="Control"/>s from the given <paramref name="root"/> <see cref="Control"/>,
        /// including the <paramref name="root"/>.</returns>
        public static IEnumerable<Control> GetControls(this Control root, Func<Control, bool> filter)
        {
            if (root != null)
            {
                // Check the filter
                if (filter == null || filter(root))
                    yield return root;

                // Check all children and call this method on them recursively
                foreach (var child in root.Controls.OfType<Control>())
                {
                    foreach (var child2 in GetControls(child, filter))
                    {
                        yield return child2;
                    }
                }
            }
        }

        /// <summary>
        /// Gets all of the <see cref="Control"/>s from the given <paramref name="root"/>.
        /// </summary>
        /// <param name="root">The root <see cref="Control"/> to search from.</param>
        /// <returns>All of the <see cref="Control"/>s from the given <paramref name="root"/> <see cref="Control"/>,
        /// including the <paramref name="root"/>.</returns>
        public static IEnumerable<Control> GetControls(this Control root)
        {
            return GetControls(root, null);
        }

        /// <summary>
        /// Gets all of the <see cref="Control"/>s that implement <see cref="IPersistable"/> from the given
        /// <paramref name="root"/> <see cref="Control"/>.
        /// </summary>
        /// <param name="root">The root <see cref="Control"/> to search from.</param>
        /// <returns>All of the <see cref="Control"/>s that implement <see cref="IPersistable"/> from the given
        /// <paramref name="root"/> <see cref="Control"/>, including the <paramref name="root"/>.</returns>
        public static IEnumerable<Control> GetPersistableControls(this Control root)
        {
            return GetControls(root, x => x is IPersistable);
        }
    }
}