using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Platyform.Extensions;

namespace Platyform.Graphics.GUI
{
    public class FormSettings : TextControlSettings
    {
        static FormSettings _default = null;

        public static FormSettings Default
        {
            get
            {
                if (_default == null)
                    new FormSettings(Color.Black, null);
                return _default;
            }
        }

        public FormSettings(Color foreColor, ControlBorder border) : base(foreColor, border)
        {
        }
    }
}