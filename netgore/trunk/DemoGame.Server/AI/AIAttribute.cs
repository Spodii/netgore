using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public sealed class AIAttribute : Attribute
    {
        public AIAttribute(AIID id)
        {
        }
    }
}
