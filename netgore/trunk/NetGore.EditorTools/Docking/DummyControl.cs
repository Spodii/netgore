using System.Linq;
using System.Windows.Forms;

namespace NetGore.EditorTools.Docking
{
    class DummyControl : Control
    {
        public DummyControl()
        {
            SetStyle(ControlStyles.Selectable, false);
        }
    }
}