using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;

namespace DemoGame.Server
{
    public interface IUserStat : IStat
    {
        User User { get; }
    }
}