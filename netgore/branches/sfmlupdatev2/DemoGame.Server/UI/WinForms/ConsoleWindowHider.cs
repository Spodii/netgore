using System.Linq;

namespace DemoGame.Server.UI
{
    public abstract partial class ConsoleWindowHider : IConsoleWindowHider
    {
        /// <summary>
        /// When overridden in the derived class, hides the console window.
        /// </summary>
        protected abstract void DoHide();

        /// <summary>
        /// Creates a <see cref="ConsoleWindowHider"/> instance.
        /// </summary>
        /// <returns>The <see cref="ConsoleWindowHider"/> instance, or null if the no implementation exist for the given
        /// environment (either due to build settings or your OS).</returns>
        public static ConsoleWindowHider TryCreate()
        {
            // Create the appropriate instance for the OS

#if !MONO
            // Windows
            return new WindowsConsoleWindowHider();
#else
    // Other OSes don't to actually hide the console right now (but can add them in the future if they do)
            return null;
#endif
        }

        #region IConsoleWindowHider Members

        /// <summary>
        /// Hides the console window.
        /// </summary>
        public void Hide()
        {
            DoHide();
        }

        #endregion
    }
}