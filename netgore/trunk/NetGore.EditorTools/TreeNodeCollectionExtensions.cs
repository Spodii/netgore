using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    public static class TreeNodeCollectionExtensions
    {
        public static T[] ToArray<T>(this TreeNodeCollection c)
        {
            T[] ret = new T[c.Count];
            int i = 0;
            foreach (var node in c)
            {
                ret[i] = (T)node;
                i++;
            }

            return ret;
        }
    }
}
