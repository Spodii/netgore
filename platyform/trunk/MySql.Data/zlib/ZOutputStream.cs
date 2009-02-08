using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace zlib
{
    class ZOutputStream : Stream
    {
        protected internal byte[] buf, buf1 = new byte[1];
        protected internal int bufsize = 512;
        protected internal bool compress;
        protected internal int flush_Renamed_Field;
        protected internal ZStream z = new ZStream();

        Stream out_Renamed;

        public override Boolean CanRead
        {
            get { return false; }
        }

        public override Boolean CanSeek
        {
            get { return false; }
        }

        public override Boolean CanWrite
        {
            get { return false; }
        }

        public virtual int FlushMode
        {
            get { return (flush_Renamed_Field); }

            set { flush_Renamed_Field = value; }
        }

        public override Int64 Length
        {
            get { return 0; }
        }

        public override Int64 Position
        {
            get { return 0; }

            set { }
        }

        /// <summary> Returns the total number of bytes input so far.</summary>
        public virtual long TotalIn
        {
            get { return z.total_in; }
        }

        /// <summary> Returns the total number of bytes output so far.</summary>
        public virtual long TotalOut
        {
            get { return z.total_out; }
        }

        public ZOutputStream(Stream out_Renamed)
        {
            InitBlock();
            this.out_Renamed = out_Renamed;
            z.inflateInit();
            compress = false;
        }

        public ZOutputStream(Stream out_Renamed, int level)
        {
            InitBlock();
            this.out_Renamed = out_Renamed;
            z.deflateInit(level);
            compress = true;
        }

        public override void Close()
        {
            try
            {
                try
                {
                    finish();
                }
                catch
                {
                }
            }
            finally
            {
                end();
                out_Renamed.Close();
                out_Renamed = null;
            }
        }

        public virtual void end()
        {
            if (compress)
                z.deflateEnd();
            else
                z.inflateEnd();
            z.free();
            z = null;
        }

        public virtual void finish()
        {
            do
            {
                int err;
                z.next_out = buf;
                z.next_out_index = 0;
                z.avail_out = bufsize;
                if (compress)
                    err = z.deflate(zlibConst.Z_FINISH);
                else
                    err = z.inflate(zlibConst.Z_FINISH);
                if (err != zlibConst.Z_STREAM_END && err != zlibConst.Z_OK)
                    throw new ZStreamException((compress ? "de" : "in") + "flating: " + z.msg);
                if (bufsize - z.avail_out > 0)
                    out_Renamed.Write(buf, 0, bufsize - z.avail_out);
            }
            while (z.avail_in > 0 || z.avail_out == 0);
            try
            {
                Flush();
            }
            catch
            {
            }
        }

        public override void Flush()
        {
            out_Renamed.Flush();
        }

        void InitBlock()
        {
            flush_Renamed_Field = zlibConst.Z_NO_FLUSH;
            buf = new byte[bufsize];
        }

        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            return 0;
        }

        public override Int64 Seek(Int64 offset, SeekOrigin origin)
        {
            return 0;
        }

        public override void SetLength(Int64 value)
        {
        }

        public override void Write(Byte[] b1, int off, int len)
        {
            if (len == 0)
                return;
            var b = new byte[b1.Length];
            Array.Copy(b1, b, b1.Length);
            z.next_in = b;
            z.next_in_index = off;
            z.avail_in = len;
            do
            {
                int err;
                z.next_out = buf;
                z.next_out_index = 0;
                z.avail_out = bufsize;
                if (compress)
                    err = z.deflate(flush_Renamed_Field);
                else
                    err = z.inflate(flush_Renamed_Field);
                if (err != zlibConst.Z_OK)
                    throw new ZStreamException((compress ? "de" : "in") + "flating: " + z.msg);
                out_Renamed.Write(buf, 0, bufsize - z.avail_out);
            }
            while (z.avail_in > 0 || z.avail_out == 0);
        }

        public void WriteByte(int b)
        {
            buf1[0] = (byte)b;
            Write(buf1, 0, 1);
        }

        public override void WriteByte(byte b)
        {
            WriteByte(b);
        }
    }
}