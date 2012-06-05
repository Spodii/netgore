using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;

namespace NetGore.Editor.Docking
{
    [Flags]
    [Serializable]
    [Editor(typeof(DockAreasEditor), typeof(UITypeEditor))]
    public enum DockAreas
    {
        Float = 1,
        DockLeft = 2,
        DockRight = 4,
        DockTop = 8,
        DockBottom = 16,
        Document = 32
    }
}