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
        public PacketWriterPool()
            : base(CreatorHandler, InitializeHandler, null, true)
        {
        }

        static PacketWriter CreatorHandler(ObjectPool<PacketWriter> pool)
        {
            return new PacketWriter(pool);
        }

        static void InitializeHandler(PacketWriter x)
        {
            x.Reset();
        }
    }
}