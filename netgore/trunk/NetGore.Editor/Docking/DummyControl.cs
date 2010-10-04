using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.Docking
{
    class DummyControl : Control
    {
        public DummyControl()
        {
            SetStyle(ControlStyles.Selectable, false);
        }
    }
}