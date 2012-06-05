using System;
using System.Linq;

namespace NetGore.Graphics.GUI
{
    [Flags]
    public enum MessageBoxButton : byte
    {
        /// <summary>
        /// An unknown or invalid button.
        /// </summary>
        None,

        /// <summary>
        /// The "Yes" button on a <see cref="MessageBox"/>.
        /// </summary>
        Yes = 1 << 0,

        /// <summary>
        /// The "No" button on a <see cref="MessageBox"/>.
        /// </summary>
        No = 1 << 1,

        /// <summary>
        /// The "Cancel" button on a <see cref="MessageBox"/>.
        /// </summary>
        Cancel = 1 << 2,

        /// <summary>
        /// The "Retry" button on a <see cref="MessageBox"/>.
        /// </summary>
        Retry = 1 << 3,

        /// <summary>
        /// The "Ok" button on a <see cref="MessageBox"/>.
        /// </summary>
        Ok = 1 << 4,

        /// <summary>
        /// The "Abort" button on a <see cref="MessageBox"/>.
        /// </summary>
        Abort = 1 << 5,

        /// <summary>
        /// Both the "Yes" and "No" buttons on a <see cref="MessageBox"/>.
        /// </summary>
        YesNo = Yes | No,

        /// <summary>
        /// The "Yes", "No", and "Cancel" buttons on a <see cref="MessageBox"/>.
        /// </summary>
        YesNoCancel = Yes | No | Cancel,

        /// <summary>
        /// The "Ok" and "Cancel" buttons on a <see cref="MessageBox"/>.
        /// </summary>
        OkCancel = Ok | Cancel
    }
}