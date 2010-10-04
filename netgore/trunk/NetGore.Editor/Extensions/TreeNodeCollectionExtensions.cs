using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor
{
    /// <summary>
    /// Extension methods for the TreeNodeCollection.
    /// </summary>
    public static class TreeNodeCollectionExtensions
    {
        /// <summary>
        /// Gets an array of the TreeNodeCollection.
        /// </summary>
        /// <typeparam name="T">The Type of object.</typeparam>
        /// <param name="c">The TreeNodeCollection to get an array of.</param>
        /// <returns>An array of the TreeNodeCollection with each element casted to type <typeparamref name="T"/>.</returns>
        public static T[] ToArray<T>(this TreeNodeCollection c)
        {
            var ret = new T[c.Count];
            var i = 0;
            foreach (var node in c)
            {
                ret[i] = (T)node;
                i++;
            }

            return ret;
        }

        /// <summary>
        /// Gets an array of the TreeNodeCollection.
        /// </summary>
        /// <param name="c">The TreeNodeCollection to get an array of.</param>
        /// <returns>An array of the TreeNodeCollection.</returns>
        public static object[] ToArray(this TreeNodeCollection c)
        {
            var ret = new object[c.Count];
            var i = 0;
            foreach (var node in c)
            {
                ret[i] = node;
                i++;
            }

            return ret;
        }
    }
}