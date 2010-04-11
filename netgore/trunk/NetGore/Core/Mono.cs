using System;
using System.Linq;

namespace NetGore
{
    public static class Mono
    {
        // TODO: ## Remove
        public static bool IsRunningMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}