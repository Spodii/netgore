using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Graphics.GUI
{
    public class PanelSettings : ControlSettings
    {
        static PanelSettings _default = null;

        public static PanelSettings Default
        {
            get
            {
                if (_default == null)
                    _default = new PanelSettings(null);
                return _default;
            }
        }

        public PanelSettings(ControlBorder border) : base(border)
        {
        }
    }
}