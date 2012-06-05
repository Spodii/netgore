using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.Docking
{
    static class Win32Helper
    {
        public static Control ControlAtPoint(Point pt)
        {
            return Control.FromChildHandle(NativeMethods.WindowFromPoint(pt));
        }

        public static uint MakeLong(int low, int high)
        {
            return (uint)((high << 16) + low);
        }
    }
}