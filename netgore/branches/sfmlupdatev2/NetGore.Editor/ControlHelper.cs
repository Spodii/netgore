using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor
{
    /// <summary>
    /// Contains helper methods for <see cref="Control"/>s.
    /// </summary>
    public static class ControlHelper
    {
        /// <summary>
        /// Handles drawing a string for a <see cref="Control"/>.
        /// </summary>
        /// <param name="e">The <see cref="DrawItemEventArgs"/>.</param>
        /// <param name="value">The string to draw.</param>
        public static void DrawItem(DrawItemEventArgs e, string value)
        {
            // Draw the background
            e.DrawBackground();

            // Draw the text
            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(value, e.Font, brush, e.Bounds);
            }

            // Draw the focus region
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Handles drawing a string for a <see cref="Control"/>.
        /// </summary>
        /// <param name="e">The <see cref="DrawItemEventArgs"/>.</param>
        /// <param name="kvp">The string to draw.</param>
        public static void DrawItem(DrawItemEventArgs e, KeyValuePair<string, string> kvp)
        {
            // Draw the background
            e.DrawBackground();

            var keyWidth = string.IsNullOrEmpty(kvp.Key) ? 0 : (int)e.Graphics.MeasureString(kvp.Key, e.Font).Width;
            var valueBounds = new Rectangle(e.Bounds.X + keyWidth, e.Bounds.Y, e.Bounds.Width - keyWidth, e.Bounds.Height);

            // Draw the text
            if (!string.IsNullOrEmpty(kvp.Key))
            {
                using (var brush = new SolidBrush((e.State & DrawItemState.Selected) != 0 ? Color.LightGreen : Color.Green))
                {
                    e.Graphics.DrawString(kvp.Key, e.Font, brush, e.Bounds);
                }
            }

            if (!string.IsNullOrEmpty(kvp.Value))
            {
                using (var brush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(kvp.Value, e.Font, brush, valueBounds);
                }
            }

            // Draw the focus region
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Implements the basic <see cref="ListBox.OnDrawItem"/> logic.
        /// </summary>
        /// <typeparam name="T">The expected type of item.</typeparam>
        /// <param name="items">The <see cref="Control"/>'s item collection.</param>
        /// <param name="e">The <see cref="DrawItemEventArgs"/>.</param>
        /// <param name="toString">The <see cref="Func{T,TResult}"/> to turn the item into a string.</param>
        /// <returns>True if the item was successfully drawn; false if the drawing failed. When false,
        /// it is highly recommended you call the base <see cref="ListBox.OnDrawItem"/>.</returns>
        public static bool DrawListItem<T>(IList items, DrawItemEventArgs e, Func<T, string> toString)
        {
            // Check for a valid index
            if (e.Index < 0 || e.Index >= items.Count)
                return false;

            // Check for a valid item type
            if (!(items[e.Index] is T))
                return false;

            // Get the item
            var item = (T)items[e.Index];

            // Get the text and draw
            DrawItem(e, toString(item));

            return true;
        }

        /// <summary>
        /// Implements the basic <see cref="ComboBox.OnDrawItem"/> logic.
        /// </summary>
        /// <typeparam name="T">The expected type of item.</typeparam>
        /// <param name="items">The <see cref="Control"/>'s items collection.</param>
        /// <param name="e">The <see cref="DrawItemEventArgs"/>.</param>
        /// <param name="toString">The <see cref="Func{T,U}"/> to turn the item into a string.</param>
        /// <returns>True if the item was successfully drawn; false if the drawing failed. When false,
        /// it is highly recommended you call the base <see cref="ComboBox.OnDrawItem"/>.</returns>
        public static bool DrawListItem<T>(IList items, DrawItemEventArgs e, Func<T, KeyValuePair<string, string>> toString)
        {
            // Check for a valid index
            if (e.Index < 0 || e.Index >= items.Count)
                return false;

            // Check for a valid item type
            if (!(items[e.Index] is T))
                return false;

            // Get the item
            var item = (T)items[e.Index];

            // Get the text and draw
            DrawItem(e, toString(item));

            return true;
        }
    }
}