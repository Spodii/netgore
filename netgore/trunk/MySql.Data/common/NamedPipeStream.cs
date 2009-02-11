using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient.Properties;

namespace MySql.Data.Common
{
    /// <summary>
    /// Summary description for API.
    /// </summary>
    class NamedPipeStream : Stream
    {
        FileAccess _mode;
        int pipeHandle;

        public override bool CanRead
        {
            get { return (_mode & FileAccess.Read) > 0; }
        }

        public override bool CanSeek
        {
            get { throw new NotSupportedException(Resources.NamedPipeNoSeek); }
        }

        public override bool CanWrite
        {
            get { return (_mode & FileAccess.Write) > 0; }
        }

        public override long Length
        {
            get { throw new NotSupportedException(Resources.NamedPipeNoSeek); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(Resources.NamedPipeNoSeek); }
            set { }
        }

        public NamedPipeStream(string host, FileAccess mode)
        {
            Open(host, mode);
        }

        public override void Close()
        {
            if (pipeHandle != 0)
            {
                NativeMethods.CloseHandle((IntPtr)pipeHandle);
                pipeHandle = 0;
            }
        }

        public override void Flush()
        {
            if (pipeHandle != 0)
                NativeMethods.FlushFileBuffers((IntPtr)pipeHandle);
        }

        public void Open(string host, FileAccess mode)
        {
            _mode = mode;
            uint pipemode = 0;

            if ((mode & FileAccess.Read) > 0)
                pipemode |= NativeMethods.GENERIC_READ;
            if ((mode & FileAccess.Write) > 0)
                pipemode |= NativeMethods.GENERIC_WRITE;

            pipeHandle = NativeMethods.CreateFile(host, pipemode, 0, null, NativeMethods.OPEN_EXISTING, 0, 0);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", Resources.BufferCannotBeNull);
            if (buffer.Length < (offset + count))
                throw new ArgumentException(Resources.BufferNotLargeEnough);
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", offset, Resources.OffsetCannotBeNegative);
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count, Resources.CountCannotBeNegative);
            if (! CanRead)
                throw new NotSupportedException(Resources.StreamNoRead);
            if (pipeHandle == 0)
                throw new ObjectDisposedException("NamedPipeStream", Resources.StreamAlreadyClosed);

            // first read the data into an internal buffer since ReadFile cannot read into a buf at
            // a specified offset
            uint read;
            var buf = new Byte[count];
            bool result = NativeMethods.ReadFile((IntPtr)pipeHandle, buf, (uint)count, out read, IntPtr.Zero);

            if (! result)
            {
                Close();
                throw new MySqlException(Resources.ReadFromStreamFailed, true, null);
            }

            Array.Copy(buf, 0, buffer, offset, (int)read);
            return (int)read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException(Resources.NamedPipeNoSeek);
        }

        public override void SetLength(long length)
        {
            throw new NotSupportedException(Resources.NamedPipeNoSetLength);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", Resources.BufferCannotBeNull);
            if (buffer.Length < (offset + count))
                throw new ArgumentException(Resources.BufferNotLargeEnough, "buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", offset, Resources.OffsetCannotBeNegative);
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count, Resources.CountCannotBeNegative);
            if (! CanWrite)
                throw new NotSupportedException(Resources.StreamNoWrite);
            if (pipeHandle == 0)
                throw new ObjectDisposedException("NamedPipeStream", Resources.StreamAlreadyClosed);

            // copy data to internal buffer to allow writing from a specified offset
            uint bytesWritten = 0;
            bool result;

            if (offset == 0 && count <= 65535)
                result = NativeMethods.WriteFile((IntPtr)pipeHandle, buffer, (uint)count, out bytesWritten, IntPtr.Zero);
            else
            {
                var localBuf = new byte[65535];

                result = true;
                while (count != 0 && result)
                {
                    uint thisWritten;
                    int cnt = Math.Min(count, 65535);
                    Array.Copy(buffer, offset, localBuf, 0, cnt);
                    result = NativeMethods.WriteFile((IntPtr)pipeHandle, localBuf, (uint)cnt, out thisWritten, IntPtr.Zero);
                    bytesWritten += thisWritten;
                    count -= cnt;
                    offset += cnt;
                }
            }

            if (! result)
            {
                Close();
                throw new MySqlException(Resources.WriteToStreamFailed, true, null);
            }
            if (bytesWritten < count)
                throw new IOException("Unable to write entire buffer to stream");
        }
    }
}