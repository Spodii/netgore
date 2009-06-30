using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.SkeletonEditor
{
    public class ListItem<T>
    {
        public readonly T Item;
        public string Name;

        public ListItem(T item, string name)
        {
            Item = item;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}