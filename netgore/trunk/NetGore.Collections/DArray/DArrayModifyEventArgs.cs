using System;
using System.Linq;

namespace NetGore.Collections
{
    public class DArrayModifyEventArgs<T> : EventArgs
    {
        public readonly int Index;
        public readonly T Item;

        public DArrayModifyEventArgs(T item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}