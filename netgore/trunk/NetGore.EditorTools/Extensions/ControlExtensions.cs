using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// Gets all of the <see cref="Control"/>s that implement <see cref="IPersistable"/> from the given
        /// <paramref name="root"/> <see cref="Control"/>.
        /// </summary>
        /// <param name="root">The root <see cref="Control"/> to search from.</param>
        /// <returns>All of the <see cref="Control"/>s that implement <see cref="IPersistable"/> from the given
        /// <paramref name="root"/> <see cref="Control"/>, including the <paramref name="root"/>.</returns>
        public static IEnumerable<Control> GetPersistableControls(this Control root)
        {
            if (root != null)
            {
                // Return this Control if it implements IPersistable
                if (root is IPersistable)
                    yield return root;

                // Check all children and call this method on them recursively
                foreach (var child in root.Controls.OfType<Control>())
                {
                    foreach (var persistableChild in GetPersistableControls(child))
                        yield return persistableChild;
                }
            }
        }
    }
}
