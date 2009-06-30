using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.Server
{
    public interface IUpdateableStat : IStat
    {
        int LastUpdatedValue { get; }
        bool NeedsUpdate { get; }
        StatUpdateHandler UpdateHandler { get; }

        void Update();
    }
}