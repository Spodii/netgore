using System.Linq;

namespace NetGore.Graphics.GUI
{
    public class PanelSettings : ControlSettings
    {
        static PanelSettings _default = null;

        public PanelSettings(ControlBorder border) : base(border)
        {
        }

        public static PanelSettings Default
        {
            get
            {
                if (_default == null)
                    _default = new PanelSettings(null);
                return _default;
            }
        }
    }
}