using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lidgren.Network
{
    class ReceivedFragmentGroup
    {
        public byte[] Data;
        public float LastReceived;
        public NetBitVector ReceivedChunks;
    }

    public partial class NetPeer
    {
        readonly Dictionary<int, ReceivedFragmentGroup> m_receivedFragmentGroups;
        int m_lastUsedFragmentGroup;

        // on user thread

        void HandleReleasedFragment(NetIncomingMessage im)
        {
            //
            // read fragmentation header and combine fragments
            //
            int group;
            int totalBits;
            int chunkByteSize;
            int chunkNumber;
            var ptr = NetFragmentationHelper.ReadHeader(im.m_data, 0, out group, out totalBits, out chunkByteSize, out chunkNumber);

            NetException.Assert(im.LengthBytes > ptr);

            NetException.Assert(group > 0);
            NetException.Assert(totalBits > 0);
            NetException.Assert(chunkByteSize > 0);

            var totalBytes = NetUtility.BytesToHoldBits(totalBits);
            var totalNumChunks = totalBytes / chunkByteSize;
            if (totalNumChunks * chunkByteSize < totalBytes)
                totalNumChunks++;

            NetException.Assert(chunkNumber < totalNumChunks);

            if (chunkNumber >= totalNumChunks)
            {
                LogWarning("Index out of bounds for chunk " + chunkNumber + " (total chunks " + totalNumChunks + ")");
                return;
            }

            ReceivedFragmentGroup info;
            if (!m_receivedFragmentGroups.TryGetValue(group, out info))
            {
                info = new ReceivedFragmentGroup();
                info.Data = new byte[totalBytes];
                info.ReceivedChunks = new NetBitVector(totalNumChunks);
                m_receivedFragmentGroups[group] = info;
            }

            info.ReceivedChunks[chunkNumber] = true;
            info.LastReceived = (float)NetTime.Now;

            // copy to data
            var offset = (chunkNumber * chunkByteSize);
            Buffer.BlockCopy(im.m_data, ptr, info.Data, offset, im.LengthBytes - ptr);

            var cnt = info.ReceivedChunks.Count();
            //Console.WriteLine("Found fragment #" + chunkNumber + " in group " + group + " offset " + offset + " of total bits " + totalBits + " (total chunks done " + cnt + ")");

            LogVerbose("Received fragment " + chunkNumber + " of " + totalNumChunks + " (" + cnt + " chunks received)");

            if (info.ReceivedChunks.Count() == totalNumChunks)
            {
                // Done! Transform this incoming message
                im.m_data = info.Data;
                im.m_bitLength = totalBits;
                im.m_isFragment = false;

                LogVerbose("Fragment group #" + group + " fully received in " + totalNumChunks + " chunks (" + totalBits +
                           " bits)");

                ReleaseMessage(im);
            }
            else
            {
                // data has been copied; recycle this incoming message
                Recycle(im);
            }

            return;
        }

        void SendFragmentedMessage(NetOutgoingMessage msg, IList<NetConnection> recipients, NetDeliveryMethod method,
                                   int sequenceChannel)
        {
            var group = Interlocked.Increment(ref m_lastUsedFragmentGroup);
            if (group >= NetConstants.MaxFragmentationGroups)
            {
                // TODO: not thread safe; but in practice probably not an issue
                m_lastUsedFragmentGroup = 1;
                group = 1;
            }
            msg.m_fragmentGroup = group;

            // do not send msg; but set fragmentgroup in case user tries to recycle it immediately

            // create fragmentation specifics
            var totalBytes = msg.LengthBytes;
            var mtu = m_configuration.MaximumTransmissionUnit;

            var bytesPerChunk = NetFragmentationHelper.GetBestChunkSize(group, totalBytes, mtu);

            var numChunks = totalBytes / bytesPerChunk;
            if (numChunks * bytesPerChunk < totalBytes)
                numChunks++;

            var bitsPerChunk = bytesPerChunk * 8;
            var bitsLeft = msg.LengthBits;
            for (var i = 0; i < numChunks; i++)
            {
                var chunk = CreateMessage(mtu);

                chunk.m_bitLength = (bitsLeft > bitsPerChunk ? bitsPerChunk : bitsLeft);
                chunk.m_data = msg.m_data;
                chunk.m_fragmentGroup = group;
                chunk.m_fragmentGroupTotalBits = totalBytes * 8;
                chunk.m_fragmentChunkByteSize = bytesPerChunk;
                chunk.m_fragmentChunkNumber = i;

                NetException.Assert(chunk.m_bitLength != 0);
                NetException.Assert(chunk.GetEncodedSize() < mtu);

                Interlocked.Add(ref chunk.m_recyclingCount, recipients.Count);

                foreach (var recipient in recipients)
                {
                    recipient.EnqueueMessage(chunk, method, sequenceChannel);
                }

                bitsLeft -= bitsPerChunk;
            }

            return;
        }
    }
}