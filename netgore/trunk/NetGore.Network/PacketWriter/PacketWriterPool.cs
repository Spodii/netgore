using System.Linq;
using NetGore.Collections;

namespace NetGore.Network
{
    /// <summary>
    /// Pool of <see cref="PacketWriter"/>s.
    /// </summary>
    public class PacketWriterPool : ObjectPool<PacketWriter>
    {
    }
}