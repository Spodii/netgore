using System.Linq;
using System.Reflection;
using log4net;
using NetGore;

namespace DemoGame.Client
{
    /// <summary>
    /// Main application entry point - does nothing more than start the primary class
    /// </summary>
    static class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            log.Info("Starting client...");

            GameSettings.Initialize();
            ThreadAsserts.IsMainThread();

            using (var game = new DemoGame())
            {
                game.Run();
            }
        }
    }
}