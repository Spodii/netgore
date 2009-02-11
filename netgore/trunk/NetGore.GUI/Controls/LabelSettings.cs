using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public class LabelSettings : TextControlSettings
    {
        static LabelSettings _default = null;

        public static LabelSettings Default
        {
            get
            {
                if (_default == null)
                    _default = new LabelSettings(null);
                return _default;
            }
        }

        public LabelSettings(ControlBorder border) : base(Color.Black, border)
        {
        }
    }
}