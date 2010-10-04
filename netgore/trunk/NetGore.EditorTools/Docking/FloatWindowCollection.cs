using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NetGore.EditorTools.Docking
{
    public class FloatWindowCollection : ReadOnlyCollection<FloatWindow>
    {
        internal FloatWindowCollection() : base(new List<FloatWindow>())
        {
        }

        internal int Add(FloatWindow fw)
        {
            if (Items.Contains(fw))
                return Items.IndexOf(fw);

            Items.Add(fw);
            return Count - 1;
        }

        internal void BringWindowToFront(FloatWindow fw)
        {
            Items.Remove(fw);
            Items.Add(fw);
        }

        internal void Dispose()
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                this[i].Close();
            }
        }

        internal void Remove(FloatWindow fw)
        {
            Items.Remove(fw);
        }
    }
}