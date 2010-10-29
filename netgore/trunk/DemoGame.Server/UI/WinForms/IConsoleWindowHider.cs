using System.Linq;

namespace DemoGame.Server.UI
{
    /// <summary>
    /// Interface for an object that can hide the console window.
    /// </summary>
    public interface IConsoleWindowHider
    {
        /// <summary>
        /// Hides the console window.
        /// </summary>
        void Hide();
    }
}