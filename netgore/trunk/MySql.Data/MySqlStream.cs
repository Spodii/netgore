using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using MySql.Data.Common;
using MySql.Data.MySqlClient.Properties;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Summary description for MySqlStream.
    /// </summary>
    class MySqlStream
    {
        readonly MemoryStream bufferStream;
        readonly byte[] byteBuffer;

        readonly Stream inStream;
        readonly Stream outStream;
        Encoding encoding;
        ulong inLength;
        ulong inPos;
        bool isLastPacket;
        int maxBlockSize;
        ulong maxPacketSize;

        ulong outLength;
        ulong outPos;
        int peekByte;
        byte sequenceByte;
        DBVersion version;

        public Encoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        public bool HasMoreData
        {
            get { return inLength > 0 && (inLength == (ulong)maxBlockSize || inPos < inLength); }
        }

        public MemoryStream InternalBuffer
        {
            get { return bufferStream; }
        }

        public bool IsLastPacket
        {
            get { return isLastPacket; }
        }

        public int MaxBlockSize
        {
            get { return maxBlockSize; }
            set { maxBlockSize = value; }
        }

        public ulong MaxPacketSize
        {
            get { return maxPacketSize; }
            set { maxPacketSize = value; }
        }

        public byte SequenceByte
        {
            get { return sequenceByte; }
            set { sequenceByte = value; }
        }

        public DBVersion Version
        {
            get { return version; }
            set { version = value; }
        }

        public MySqlStream(Encoding encoding)
        {
            // we have no idea what the real value is so we start off with the max value
            // The real value will be set in NativeDriver.Configure()
            maxPacketSize = ulong.MaxValue;

            // we default maxBlockSize to MaxValue since we will get the 'real' value in 
            // the authentication handshake and we know that value will not exceed 
            // true maxBlockSize prior to that.
            maxBlockSize = Int32.MaxValue;

            this.encoding = encoding;
            bufferStream = new MemoryStream();
            byteBuffer = new byte[1];
            peekByte = -1;
        }

        public MySqlStream(Stream baseStream, Encoding encoding, bool compress) : this(encoding)
        {
            inStream = new BufferedStream(baseStream);
            outStream = new BufferedStream(baseStream);
            if (compress)
            {
                inStream = new CompressedStream(inStream);
                outStream = new CompressedStream(outStream);
            }
        }

        public void Close()
        {
            inStream.Close();
            // no need to close outStream because closing
            // inStream closes the underlying network stream
            // for us.
        }

        public void Flush()
        {
            if (outLength == 0)
            {
                if (bufferStream.Length > 0)
                {
                    var bytes = bufferStream.GetBuffer();
                    StartOutput((ulong)bufferStream.Length, false);
                    Write(bytes, 0, (int)bufferStream.Length);
                }
                bufferStream.SetLength(0);
                bufferStream.Position = 0;
            }

            try
            {
                outStream.Flush();
            }
            catch (IOException ioex)
            {
                throw new MySqlException(Resources.WriteToStreamFailed, true, ioex);
            }
        }

        /// <summary>
        /// LoadPacket loads up and decodes the header of the incoming packet.
        /// </summary>
        public void LoadPacket()
        {
            try
            {
                int b1 = inStream.ReadByte();
                int b2 = inStream.ReadByte();
                int b3 = inStream.ReadByte();
                int seqByte = inStream.ReadByte();

                if (b1 == -1 || b2 == -1 || b3 == -1 || seqByte == -1)
                    throw new MySqlException(Resources.ConnectionBroken, true, null);

                sequenceByte = (byte)++seqByte;
                inLength = (ulong)(b1 + (b2 << 8) + (b3 << 16));

                inPos = 0;
            }
            catch (IOException ioex)
            {
                throw new MySqlException(Resources.ReadFromStreamFailed, true, ioex);
            }
        }

        /// <summary>
        /// OpenPacket is called by NativeDriver to start reading the next
        /// packet on the stream.
        /// </summary>
        public void OpenPacket()
        {
            if (HasMoreData)
                SkipBytes((int)(inLength - inPos));
            // make sure we have read all the data from the previous packet
            //Debug.Assert(HasMoreData == false, "HasMoreData is true in OpenPacket");

            LoadPacket();

            int peek = PeekByte();
            if (peek == 0xff)
            {
                ReadByte(); // read off the 0xff

                int code = ReadInteger(2);
                string msg = ReadString();
                if (msg.StartsWith("#"))
                {
                    msg.Substring(1, 5); /* state code */
                    msg = msg.Substring(6);
                }
                throw new MySqlException(msg, code);
            }
            isLastPacket = (peek == 0xfe && (inLength < 9));
        }

        /// <summary>
        /// Peek at the next byte off the stream
        /// </summary>
        /// <returns>The next byte off the stream</returns>
        public int PeekByte()
        {
            if (peekByte == -1)
            {
                peekByte = ReadByte();
                // ReadByte will advance inPos so we need to back it up since
                // we are not really reading the byte
                inPos--;
            }
            return peekByte;
        }

        /// <summary>
        /// Reads a block of bytes from the input stream into the given buffer.
        /// </summary>
        /// <returns>The number of bytes read.</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            // we use asserts here because this is internal code
            // and we should be calling it correctly in all cases
            Debug.Assert(buffer != null);
            Debug.Assert(offset >= 0 && (offset < buffer.Length || (offset == 0 && buffer.Length == 0)));
            Debug.Assert(count >= 0);
            Debug.Assert((offset + count) <= buffer.Length);

            int totalRead = 0;

            while (count > 0)
            {
                // if we have peeked at a byte, then read it off first.
                if (peekByte != -1)
                {
                    buffer[offset++] = (byte)ReadByte();
                    count--;
                    totalRead++;
                    continue;
                }

                // check if we are done reading the current packet
                if (inPos == inLength)
                {
                    // if yes and this block is not max size, then we are done
                    if (inLength < (ulong)maxBlockSize)
                        return 0;

                    // the current block is maxBlockSize so we need to read
                    // in another block to continue
                    LoadPacket();
                }

                int lenToRead = Math.Min(count, (int)(inLength - inPos));
                try
                {
                    int read = inStream.Read(buffer, offset, lenToRead);

                    // we don't throw an exception here even though this probably
                    // indicates a broken connection.  We leave that to the 
                    // caller.
                    if (read == 0)
                        break;

                    count -= read;
                    offset += read;
                    totalRead += read;
                    inPos += (ulong)read;
                }
                catch (IOException ioex)
                {
                    throw new MySqlException(Resources.ReadFromStreamFailed, true, ioex);
                }
            }

            return totalRead;
        }

        /// <summary>
        /// Reads the next byte from the incoming stream
        /// </summary>
        /// <returns></returns>
        public int ReadByte()
        {
            int b;
            if (peekByte != -1)
            {
                b = PeekByte();
                peekByte = -1;
                inPos++; // we only do this here since Read will also do it
            }
            else
            {
                // we read the byte this way because we might cross over a 
                // multipacket boundary
                int cnt = Read(byteBuffer, 0, 1);
                if (cnt <= 0)
                    return -1;
                b = byteBuffer[0];
            }
            return b;
        }

        public long ReadFieldLength()
        {
            byte c = (byte)ReadByte();

            switch (c)
            {
                case 251:
                    return -1;
                case 252:
                    return ReadInteger(2);
                case 253:
                    return ReadInteger(3);
                case 254:
                    return ReadInteger(8);
                default:
                    return c;
            }
        }

        public int ReadInteger(int numbytes)
        {
            return (int)ReadLong(numbytes);
        }

        public string ReadLenString()
        {
            long len = ReadPackedInteger();
            return ReadString(len);
        }

        public ulong ReadLong(int numbytes)
        {
            ulong val = 0;
            int raise = 1;
            for (int x = 0; x < numbytes; x++)
            {
                int b = ReadByte();
                val += (ulong)(b * raise);
                raise *= 256;
            }
            return val;
        }

        public int ReadNBytes()
        {
            byte c = (byte)ReadByte();
            if (c < 1 || c > 4)
                throw new MySqlException(Resources.IncorrectTransmission);
            return ReadInteger(c);
        }

        public int ReadPackedInteger()
        {
            byte c = (byte)ReadByte();

            switch (c)
            {
                case 251:
                    return -1;
                case 252:
                    return ReadInteger(2);
                case 253:
                    return ReadInteger(3);
                case 254:
                    return ReadInteger(4);
                default:
                    return c;
            }
        }

        public string ReadString(long length)
        {
            if (length == 0)
                return String.Empty;
            var buf = new byte[length];
            Read(buf, 0, (int)length);
            return encoding.GetString(buf, 0, buf.Length);
        }

        public string ReadString()
        {
            MemoryStream ms = new MemoryStream();

            int b = ReadByte();
            while (b != 0 && b != -1)
            {
                ms.WriteByte((byte)b);
                b = ReadByte();
            }

            return encoding.GetString(ms.GetBuffer(), 0, (int)ms.Length);
        }

        public void SendEmptyPacket()
        {
            outLength = 0;
            outPos = 0;
            WriteHeader();
            outStream.Flush();
        }

        public void SendEntirePacketDirectly(byte[] buffer, int count)
        {
            buffer[0] = (byte)(count & 0xff);
            buffer[1] = (byte)((count >> 8) & 0xff);
            buffer[2] = (byte)((count >> 16) & 0xff);
            buffer[3] = sequenceByte++;
            outStream.Write(buffer, 0, count + 4);
            outStream.Flush();
        }

        public void SkipBytes(int len)
        {
            while (len-- > 0)
            {
                ReadByte();
            }
        }

        /// <summary>
        /// SkipPacket will read the remaining bytes of a packet into a small
        /// local buffer and discard them.
        /// </summary>
        public void SkipPacket()
        {
            var tempBuf = new byte[1024];
            while (inPos < inLength)
            {
                int toRead = (int)Math.Min((ulong)tempBuf.Length, (inLength - inPos));
                Read(tempBuf, 0, toRead);
            }
        }

        /// <summary>
        /// StartOutput is used to reset the write state of the stream.
        /// </summary>
        public void StartOutput(ulong length, bool resetSequence)
        {
            outLength = outPos = 0;
            if (length > 0)
            {
                if (length > maxPacketSize)
                    throw new MySqlException(Resources.QueryTooLarge, (int)MySqlErrorCode.PacketTooLarge);
                outLength = length;
            }

            if (resetSequence)
                sequenceByte = 0;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            Debug.Assert(buffer != null && offset >= 0 && count >= 0);

            // if we are buffering, then just write it to the buffer
            if (outLength == 0)
            {
                bufferStream.Write(buffer, offset, count);
                return;
            }

            // make sure the inputs to the method make sense
            Debug.Assert(outLength > 0 && (outPos + (ulong)count) <= outLength);

            int pos = 0;
            // if we get here, we are not buffering.  
            // outLength is the total amount of data we are going to send
            // This means that multiple calls to write could be combined.
            while (count > 0)
            {
                int cntToWrite = (int)Math.Min((outLength - outPos), (ulong)count);
                cntToWrite = Math.Min(maxBlockSize - (int)(outPos % (ulong)maxBlockSize), cntToWrite);

                // if we are at a block border, then we need to send a new header
                if ((outPos % (ulong)maxBlockSize) == 0)
                    WriteHeader();

                try
                {
                    outStream.Write(buffer, pos, cntToWrite);
                }
                catch (IOException ioex)
                {
                    throw new MySqlException(Resources.WriteToStreamFailed, true, ioex);
                }

                outPos += (ulong)cntToWrite;
                pos += cntToWrite;
                count -= cntToWrite;
            }
        }

        public void Write(byte[] buffer)
        {
            Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes a single byte to the output stream.
        /// </summary>
        public void WriteByte(byte value)
        {
            byteBuffer[0] = value;
            Write(byteBuffer, 0, 1);
        }

        /// <summary>
        /// Writes out the header that is used at the start of a transmission
        /// and at the beginning of every packet when multipacket is used.
        /// </summary>
        void WriteHeader()
        {
            int len = (int)Math.Min((outLength - outPos), (ulong)maxBlockSize);

            outStream.WriteByte((byte)(len & 0xff));
            outStream.WriteByte((byte)((len >> 8) & 0xff));
            outStream.WriteByte((byte)((len >> 16) & 0xff));
            outStream.WriteByte(sequenceByte++);
        }

        /// <summary>
        /// WriteInteger
        /// </summary>
        /// <param name="v"></param>
        /// <param name="numbytes"></param>
        public void WriteInteger(long v, int numbytes)
        {
            long val = v;

            Debug.Assert(numbytes > 0 && numbytes < 5);

            for (int x = 0; x < numbytes; x++)
            {
                WriteByte((byte)(val & 0xff));
                val >>= 8;
            }
        }

        public void WriteLength(long length)
        {
            if (length < 251)
                WriteByte((byte)length);
            else if (length < 65536L)
            {
                WriteByte(252);
                WriteInteger(length, 2);
            }
            else if (length < 16777216L)
            {
                WriteByte(253);
                WriteInteger(length, 3);
            }
            else
            {
                WriteByte(254);
                WriteInteger(length, 4);
            }
        }

        public void WriteLenString(string s)
        {
            var bytes = encoding.GetBytes(s);
            WriteLength(bytes.Length);
            Write(bytes, 0, bytes.Length);
        }

        public void WriteString(string v)
        {
            WriteStringNoNull(v);
            WriteByte(0);
        }

        public void WriteStringNoNull(string v)
        {
            var bytes = encoding.GetBytes(v);
            Write(bytes, 0, bytes.Length);
        }
    }
}