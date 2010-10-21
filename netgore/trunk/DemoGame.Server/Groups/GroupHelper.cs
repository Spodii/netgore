using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Properties;
using log4net;
using NetGore;

namespace DemoGame.Server.Groups
{
    public static class GroupHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets if two grouped characters are close enough to one another to share group earnings.
        /// </summary>
        /// <param name="a">The first character.</param>
        /// <param name="b">The second character.</param>
        /// <returns>True if <paramref name="a"/> is close enough to <paramref name="b"/> to share group earnings; false if they
        /// are too far away to share.</returns>
        public static bool IsInShareDistance(Character a, Character b)
        {
            // Check for valid characters
            if (a == null || b == null)
            {
                const string errmsg = "Unexpected null parameter(s).";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return false;
            }

            // Require being on the same map
            if (a.Map != b.Map)
                return false;

            // Check the distance
            var dist = a.Center.QuickDistance(b.Center);
            return dist < ServerSettings.Default.MaxGroupShareDistance;
        }
    }
}