using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace NetGore.Editor
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
            const BindingFlags gridViewFlags = BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance;
            const BindingFlags moveSplitterToFlags = BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance;

            var propertyGridView = typeof(PropertyGrid).InvokeMember("gridView", gridViewFlags, null, propertyGrid, null);

            var pgvType = propertyGridView.GetType();
            var invokeParams = new object[] { x };
            pgvType.InvokeMember("MoveSplitterTo", moveSplitterToFlags, null, propertyGridView, invokeParams);
        }

        /// <summary>
        /// Shrinks the left column (property names) on the <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="propertyGrid">The <see cref="PropertyGrid"/>.</param>
        /// <param name="padding">The number of extra pixels to pad. Can be negative to shrink the column more.</param>
        public static void ShrinkPropertiesColumn(this PropertyGrid propertyGrid, int padding = 0)
        {
            using (var g = propertyGrid.CreateGraphics())
            {
                var tabs = propertyGrid.PropertyTabs.Cast<PropertyTab>();
                var font = propertyGrid.Font;
                var w = tabs.Max(x => g.MeasureString(x.TabName, font).Width + (x.Bitmap != null ? x.Bitmap.Width : 0));
                MoveSplitter(propertyGrid, (int)w + padding);
            }
        }
    }
}