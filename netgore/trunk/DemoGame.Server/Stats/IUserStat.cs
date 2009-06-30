using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.Server
{
    public interface IUserStat : IStat
    {
        User User { get; }
    }
}