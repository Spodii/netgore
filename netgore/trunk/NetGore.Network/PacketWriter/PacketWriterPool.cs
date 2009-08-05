using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.Network
{
    /// <summary>
    /// Pool of PacketWriters.
    /// </summary>
    public class PacketWriterPool : ObjectPool<PacketWriter>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}