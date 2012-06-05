using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.IO;

namespace NetGore.Editor
{
    /// <summary>
    /// Extension methods for the <see cref="Control"/> class.
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
        public static IEnumerable<Control> GetControls(this Control root, Func<Control, bool> filter = null)
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

        /// <summary>
        /// Resizes a <see cref="Control"/>'s ClientArea to a given aspect ratio.
        /// </summary>
        /// <param name="c">The <see cref="Control"/> to resize.</param>
        /// <param name="targetRatio">The target aspect ratio.</param>
        /// <param name="expand">If true, the <paramref name="c"/> will expand to fit the target ratio. If false,
        /// it will shrink to the target ratio.</param>
        public static void ResizeToAspectRatio(this Control c, float targetRatio, bool expand)
        {
            // Get the ratio of the control's client size
            var realRatio = (float)c.ClientSize.Width / c.ClientSize.Height;

            // Check that the ratio difference is large enough to bother with
            if (targetRatio - realRatio <= float.Epsilon)
                return;

            // Are we changing the width, or height?
            var willChangeWidth = (realRatio < targetRatio);

            // Flip what one to change if not expanding
            if (!expand)
                willChangeWidth = !willChangeWidth;

            // Resize
            if (willChangeWidth)
                c.ClientSize = new Size(Math.Max(1, (int)Math.Round(c.ClientSize.Height * targetRatio)), c.ClientSize.Height);
            else
                c.ClientSize = new Size(c.ClientSize.Width, Math.Max(1, (int)Math.Round(c.ClientSize.Width * (1f / targetRatio))));
        }
    }
}