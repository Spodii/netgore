using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Platyform.Extensions;

namespace Platyform.Graphics.GUI
{
    public abstract class PictureControlSettings : ControlSettings
    {
        protected PictureControlSettings(ControlBorder border) : base(border)
        {
        }
    }
}