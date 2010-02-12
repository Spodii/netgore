using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore;

namespace DemoGame.Server.Groups
{
    public static class GroupHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static bool IsInShareDistance(Character a, Character b)
        {
            if (a == null || b == null)
            {
                const string errmsg = "Unexpected null parameter(s).";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return false;
            }

            return a.Map == b.Map && a.Center.QuickDistance(b.Center) < ServerSettings.MaxGroupShareDistance;
        }
    }
}
