using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Editor.Docking
{
    public class DockContentCollection : ReadOnlyCollection<IDockContent>
    {
        static readonly List<IDockContent> _emptyList = new List<IDockContent>(0);
        readonly DockPane m_dockPane = null;

        internal DockContentCollection() : base(new List<IDockContent>())
        {
        }

        internal DockContentCollection(DockPane pane) : base(_emptyList)
        {
            m_dockPane = pane;
        }

        public new IDockContent this[int index]
        {
            get
            {
                if (DockPane == null)
                    return Items[index];
                else
                    return GetVisibleContent(index);
            }
        }

        public new int Count
        {
            get
            {
                if (DockPane == null)
                    return base.Count;
                else
                    return CountOfVisibleContents;
            }
        }

        int CountOfVisibleContents
        {
            get
            {
                AssertDockPaneNotNull();

                var count = 0;
                foreach (var content in DockPane.Contents)
                {
                    if (content.DockHandler.DockState == DockPane.DockState)
                        count++;
                }
                return count;
            }
        }

        DockPane DockPane
        {
            get { return m_dockPane; }
        }

        internal int Add(IDockContent content)
        {
            AssertDockPaneNull();

            if (Contains(content))
                return IndexOf(content);

            Items.Add(content);
            return Count - 1;
        }

        internal void AddAt(IDockContent content, int index)
        {
            AssertDockPaneNull();

            if (index < 0 || index > Items.Count - 1)
                return;

            if (Contains(content))
                return;

            Items.Insert(index, content);
        }

        [Conditional("DEBUG")]
        void AssertDockPaneNotNull()
        {
            if (DockPane == null)
                throw new InvalidOperationException();
        }

        [Conditional("DEBUG")]
        void AssertDockPaneNull()
        {
            if (DockPane != null)
                throw new InvalidOperationException();
        }

        public new bool Contains(IDockContent content)
        {
            if (DockPane == null)
                return Items.Contains(content);
            else
                return (GetIndexOfVisibleContents(content) != -1);
        }

        int GetIndexOfVisibleContents(IDockContent content)
        {
            AssertDockPaneNotNull();

            if (content == null)
                return -1;

            var index = -1;
            foreach (var c in DockPane.Contents)
            {
                if (c.DockHandler.DockState == DockPane.DockState)
                {
                    index++;

                    if (c == content)
                        return index;
                }
            }
            return -1;
        }

        IDockContent GetVisibleContent(int index)
        {
            AssertDockPaneNotNull();

            var currentIndex = -1;
            foreach (var content in DockPane.Contents)
            {
                if (content.DockHandler.DockState == DockPane.DockState)
                    currentIndex++;

                if (currentIndex == index)
                    return content;
            }

            throw new ArgumentOutOfRangeException("index");
        }

        public new int IndexOf(IDockContent content)
        {
            if (DockPane == null)
            {
                if (!Contains(content))
                    return -1;
                else
                    return Items.IndexOf(content);
            }
            else
                return GetIndexOfVisibleContents(content);
        }

        internal void Remove(IDockContent content)
        {
            if (DockPane != null)
                throw new InvalidOperationException();

            if (!Contains(content))
                return;

            Items.Remove(content);
        }
    }
}