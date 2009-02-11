using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Platyform.Extensions;

namespace DemoGame.Server
{
    class Program
    {
        static void Main()
        {
            using (new Server())
            {
            }
        }
    }
}