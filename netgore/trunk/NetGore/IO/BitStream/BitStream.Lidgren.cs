using System;
using System.Diagnostics;
using System.Linq;
using Lidgren.Network;

namespace NetGore.IO
{
    public partial class BitStream
    {
        /// <summary>
        /// Copies the contents of the BitStream to a <see cref="NetOutgoingMessage"/>.
        /// </summary>
        /// <param name="target">The <see cref="NetOutgoingMessage"/> to copy the contents of this <see cref="BitStream"/> to.</param>
        public void CopyTo(NetOutgoingMessage target)
        {
#if DEBUG
            var startMsgLen = target.LengthBits;
#endif
            var i = 0;

            var fullBytes = (int)Math.Floor(LengthBits / (float)_bitsByte);

            // Write full 32-bit integers
            while (i + 3 < fullBytes)
            {
                var v = (_buffer[i] << (_bitsByte * 3)) | (_buffer[i + 1] << (_bitsByte * 2)) |
                        (_buffer[i + 2] << (_bitsByte * 1)) | (_buffer[i + 3]);
                i += 4;
                target.Write((uint)v);
            }

            // Write full 8-bit integers
            while (i < fullBytes)
            {
                target.Write(_buffer[i]);
                i++;
            }

            // Write the remaining bits that don't make up a full byte
            var remainingBits = LengthBits % _bitsByte;
            if (remainingBits > 0)
            {
                var remainingData = _buffer[i];
                for (var j = _highBit; j > _highBit - remainingBits; j--)
                {
                    target.Write((remainingData & (1 << j)) != 0);
                }
            }

#if DEBUG
            Debug.Assert(target.LengthBits - startMsgLen == LengthBits);
#endif
        }

        /// <summary>
        /// Writes a <see cref="NetIncomingMessage"/> buffer into the BitStream.
        /// </summary>
        /// <param name="source">The <see cref="NetIncomingMessage"/> who's contents will be copied to this BitStream.</param>
        public void Write(NetIncomingMessage source)
        {
#if DEBUG
            var startBSPos = PositionBits;
            var startMsgPos = (int)source.Position;
#endif

            // Read full 32-bit integers
            while (source.LengthBits - source.Position >= _bitsInt)
            {
                var v = source.ReadUInt32();
                Write(v);
            }

            // Read full 8-bit integers
            while (source.LengthBits - source.Position >= _bitsByte)
            {
                var v = source.ReadByte();
                Write(v);
            }

            // Read the remaining bits
            while (source.LengthBits > source.Position)
            {
                var v = source.ReadBoolean();
                Write(v);
            }

            Debug.Assert(source.Position == source.LengthBits);

#if DEBUG
            Debug.Assert(PositionBits - startBSPos == source.LengthBits - startMsgPos);
#endif
        }
    }
}