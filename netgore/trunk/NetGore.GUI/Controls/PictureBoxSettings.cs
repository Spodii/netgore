using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Graphics.GUI
{
    public class PictureBoxSettings : PictureControlSettings
    {
        static PictureBoxSettings _default = null;

        public static PictureBoxSettings Default
        {
            get
            {
                if (_default == null)
                    _default = new PictureBoxSettings(null);
                return _default;
            }
        }

        public PictureBoxSettings(ControlBorder border) : base(border)
        {
        }
    }
}