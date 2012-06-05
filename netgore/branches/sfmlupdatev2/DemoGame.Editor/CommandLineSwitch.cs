using System.Linq;

namespace DemoGame.Editor
{
    /// <summary>
    /// The command-line switches that can be used in this application.
    /// </summary>
    public enum CommandLineSwitch
    {
        /// <summary>
        /// Loads and saves every map one-by-one.
        /// </summary>
        SaveMaps,

        /// <summary>
        /// After the program has loaded and all other switches have been processed, the program will close.
        /// </summary>
        Close,
    }
}