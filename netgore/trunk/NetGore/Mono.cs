using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    public static class Mono
    {
        public static bool IsRunningMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}
