using System.Reflection;
using log4net;

namespace DemoGame.Server
{
    class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            if (log.IsInfoEnabled)
                log.Info("Starting server...");

            using (new Server())
            {
            }
        }
    }
}