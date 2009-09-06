using System.Linq;
using NetGore.Collections;

namespace NetGore.Network
{
    /// <summary>
    /// Pool of PacketWriters.
    /// </summary>
    public class PacketWriterPool : ObjectPool<PacketWriter>
    {
    }
}