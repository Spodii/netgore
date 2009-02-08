using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MySql.Data.MySqlClient.Properties;
using zlib;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Summary description for CompressedStream.
    /// </summary>
    class CompressedStream : Stream
    {
        // writing fields
        readonly Stream baseStream;
        readonly MemoryStream cache;
        readonly WeakReference inBufferRef;

        // reading fields
        readonly byte[] localByte;
        byte[] inBuffer;
        int inPos;
        int maxInPos;
        ZInputStream zInStream;

        public override bool CanRead
        {
            get { return baseStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return baseStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return baseStream.CanWrite; }
        }

        public override long Length
        {
            get { return baseStream.Length; }
        }

        public override long Position
        {
            get { return baseStream.Position; }
            set { baseStream.Position = value; }
        }

        public CompressedStream(Stream baseStream)
        {
            this.baseStream = baseStream;
            localByte = new byte[1];
            cache = new MemoryStream();
            inBufferRef = new WeakReference(inBuffer, false);
        }

        public override void Close()
        {
            baseStream.Close();
            base.Close();
        }

        void CompressAndSendCache()
        {
            long compressedLength, uncompressedLength;

            // we need to save the sequence byte that is written
            var cacheBuffer = cache.GetBuffer();
            byte seq = cacheBuffer[3];
            cacheBuffer[3] = 0;

            // first we compress our current cache
            MemoryStream compressedBuffer = CompressCache();

            // now we set our compressed and uncompressed lengths
            // based on if our compression is going to help or not
            if (compressedBuffer == null)
            {
                compressedLength = cache.Length;
                uncompressedLength = 0;
            }
            else
            {
                compressedLength = compressedBuffer.Length;
                uncompressedLength = cache.Length;
            }

            baseStream.WriteByte((byte)(compressedLength & 0xff));
            baseStream.WriteByte((byte)((compressedLength >> 8) & 0xff));
            baseStream.WriteByte((byte)((compressedLength >> 16) & 0Xff));
            baseStream.WriteByte(seq);
            baseStream.WriteByte((byte)(uncompressedLength & 0xff));
            baseStream.WriteByte((byte)((uncompressedLength >> 8) & 0xff));
            baseStream.WriteByte((byte)((uncompressedLength >> 16) & 0Xff));

            if (compressedBuffer == null)
                baseStream.Write(cacheBuffer, 0, (int)cache.Length);
            else
            {
                var compressedBytes = compressedBuffer.GetBuffer();
                baseStream.Write(compressedBytes, 0, (int)compressedBuffer.Length);
            }

            baseStream.Flush();

            cache.SetLength(0);
        }

        MemoryStream CompressCache()
        {
            // small arrays almost never yeild a benefit from compressing
            if (cache.Length < 50)
                return null;

            var cacheBytes = cache.GetBuffer();
            MemoryStream compressedBuffer = new MemoryStream();
            ZOutputStream zos = new ZOutputStream(compressedBuffer, zlibConst.Z_DEFAULT_COMPRESSION);
            zos.Write(cacheBytes, 0, (int)cache.Length);
            zos.finish();

            // if the compression hasn't helped, then just return null
            if (compressedBuffer.Length >= cache.Length)
                return null;
            return compressedBuffer;
        }

        public override void Flush()
        {
            if (!InputDone())
                return;

            CompressAndSendCache();
        }

        bool InputDone()
        {
            // if we have not done so yet, see if we can calculate how many bytes we are expecting
            if (cache.Length < 4)
                return false;
            var buf = cache.GetBuffer();
            int expectedLen = buf[0] + (buf[1] << 8) + (buf[2] << 16);
            if (cache.Length < (expectedLen + 4))
                return false;
            return true;
        }

        void PrepareNextPacket()
        {
            // read off the uncompressed and compressed lengths
            byte b1 = (byte)baseStream.ReadByte();
            byte b2 = (byte)baseStream.ReadByte();
            byte b3 = (byte)baseStream.ReadByte();
            int compressedLength = b1 + (b2 << 8) + (b3 << 16);

            baseStream.ReadByte(); // seq
            int unCompressedLength = baseStream.ReadByte() + (baseStream.ReadByte() << 8) + (baseStream.ReadByte() << 16);

            if (unCompressedLength == 0)
            {
                unCompressedLength = compressedLength;
                zInStream = null;
            }
            else
            {
                ReadNextPacket(compressedLength);
                MemoryStream ms = new MemoryStream(inBuffer);
                zInStream = new ZInputStream(ms) { maxInput = compressedLength };
            }

            inPos = 0;
            maxInPos = unCompressedLength;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", Resources.BufferCannotBeNull);
            if (offset < 0 || offset >= buffer.Length)
                throw new ArgumentOutOfRangeException("offset", Resources.OffsetMustBeValid);
            if ((offset + count) > buffer.Length)
                throw new ArgumentException(Resources.BufferNotLargeEnough, "buffer");

            if (inPos == maxInPos)
                PrepareNextPacket();

            int countToRead = Math.Min(count, maxInPos - inPos);
            int countRead;
            if (zInStream != null)
                countRead = zInStream.read(buffer, offset, countToRead);
            else
                countRead = baseStream.Read(buffer, offset, countToRead);
            inPos += countRead;

            // release the weak reference
            if (inPos == maxInPos)
            {
                zInStream = null;
                inBufferRef.Target = inBuffer;
                inBuffer = null;
            }

            return countRead;
        }

        public override int ReadByte()
        {
            Read(localByte, 0, 1);
            return localByte[0];
        }

        void ReadNextPacket(int len)
        {
            inBuffer = (byte[])inBufferRef.Target;
            if (inBuffer == null || inBuffer.Length < len)
                inBuffer = new byte[len];
            int numRead = 0;
            int numToRead = len;
            while (numToRead > 0)
            {
                int read = baseStream.Read(inBuffer, numRead, numToRead);
                numRead += read;
                numToRead -= read;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException(Resources.CSNoSetLength);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            cache.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            cache.WriteByte(value);
        }
    }
}