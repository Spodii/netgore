using System.Linq;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// A pool of <see cref="PacketWriter"/>s.
    /// </summary>
    public class PacketWriterPool : ObjectPool<PacketWriter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketWriterPool"/> class.
        /// </summary>
        public PacketWriterPool() : base(x => new PacketWriter(x), x => x.Reset(BitStreamMode.Write), null, true)
        {
        }
    }
}