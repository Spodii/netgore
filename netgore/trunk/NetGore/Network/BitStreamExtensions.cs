using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Lidgren.Network;
using NetGore.IO;

namespace NetGore.Network
{
    public static class BitStreamExtensions
    {
        public static void Write(this BitStream bs, NetIncomingMessage msg)
        {
#if DEBUG
            int startBSPos = bs.PositionBits;
            int startMsgPos = (int)msg.Position;
#endif

            // TODO: !! Make sure this works

            // Work in 32-bit blocks
            int bitsLeft;
            while ((bitsLeft = msg.LengthBits - (int)msg.Position) > 0)
            {
                if (bitsLeft >= 32)
                {
                    // Write a full 32 bits
                    var v = msg.ReadUInt32();
                    bs.Write(v);
                }
                else
                {
                    // Write less than 32 bits
                    var v = msg.ReadUInt32(bitsLeft);
                    bs.Write(v, bitsLeft);
                }
            }

            Debug.Assert(msg.Position == msg.LengthBits);

#if DEBUG
            Debug.Assert(bs.PositionBits - startBSPos == msg.LengthBits - startMsgPos);
#endif
        }
    }
}
