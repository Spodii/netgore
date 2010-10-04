using System;
using System.Linq;

namespace NetGore.EditorTools.Docking
{
    public class DockContentEventArgs : EventArgs
    {
        readonly IDockContent m_content;

        public DockContentEventArgs(IDockContent content)
        {
            m_content = content;
        }

        public IDockContent Content
        {
            get { return m_content; }
        }
    }
}