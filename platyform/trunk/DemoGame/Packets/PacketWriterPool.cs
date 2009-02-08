using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Platyform;
using Platyform.Extensions;

namespace DemoGame
{
    public class PacketWriterPool : ObjectPool<PacketWriter>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}