using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Editor
{
    /// <summary>
    /// Describes the different places that a <see cref="Tool"/> is visible in a <see cref="ToolBar"/>.
    /// </summary>
    public enum ToolBarVisibility : byte
    {
        /// <summary>
        /// The <see cref="Tool"/> is visible from every part of the editor.
        /// </summary>
        Global,

        /// <summary>
        /// The <see cref="Tool"/> is only displayed when editing maps.
        /// </summary>
        Map,
    }
}
