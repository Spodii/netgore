using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Extension methods for the <see cref="PropertyGrid"/>.
    /// </summary>
    public static class PropertyGridExtensions
    {
        /// <summary>
        /// Sets the position of the horizontal splitter on the <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="propertyGrid">The <see cref="PropertyGrid"/>.</param>
        /// <param name="x">The new position of the horizontal splitter.</param>
        public static void MoveSplitter(this PropertyGrid propertyGrid, int x)
        {
            object propertyGridView = typeof(PropertyGrid).InvokeMember("gridView",
                                                                        BindingFlags.GetField | BindingFlags.NonPublic |
                                                                        BindingFlags.Instance, null, propertyGrid, null);
            propertyGridView.GetType().InvokeMember("MoveSplitterTo",
                                                    BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                                                    null, propertyGridView, new object[] { x });
        }

        /// <summary>
        /// Shrinks the left column (property names) on the <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="propertyGrid">The <see cref="PropertyGrid"/>.</param>
        public static void ShrinkPropertiesColumn(this PropertyGrid propertyGrid)
        {
            ShrinkPropertiesColumn(propertyGrid, 0);
        }

        /// <summary>
        /// Shrinks the left column (property names) on the <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="propertyGrid">The <see cref="PropertyGrid"/>.</param>
        /// <param name="padding">The number of extra pixels to pad. Can be negative to shrink the column more.</param>
        public static void ShrinkPropertiesColumn(this PropertyGrid propertyGrid, int padding)
        {
            using (var g = propertyGrid.CreateGraphics())
            {
                var tabs = propertyGrid.PropertyTabs.Cast<PropertyTab>();
                var font = propertyGrid.Font;
                var w = tabs.Max(x => g.MeasureString(x.TabName, font).Width + (x.Bitmap != null ? x.Bitmap.Width : 0));
                MoveSplitter(propertyGrid, (int)w + 20);
            }
        }
    }
}