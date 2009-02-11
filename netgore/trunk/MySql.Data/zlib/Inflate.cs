using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace zlib
{
    sealed class Inflate
    {
        internal const int Z_FINISH = 4;
        internal const int Z_FULL_FLUSH = 3;
        internal const int Z_NO_FLUSH = 0;
        internal const int Z_PARTIAL_FLUSH = 1;
        internal const int Z_SYNC_FLUSH = 2;
        const int BAD = 13; // got an error--stay here
        const int BLOCKS = 7; // decompressing blocks
        const int CHECK1 = 11; // one check byte to go
        const int CHECK2 = 10; // two check bytes to go
        const int CHECK3 = 9; // three check bytes to go
        const int CHECK4 = 8; // four check bytes to go
        const int DICT0 = 6; // waiting for inflateSetDictionary
        const int DICT1 = 5; // one dictionary check byte to go
        const int DICT2 = 4; // two dictionary check bytes to go
        const int DICT3 = 3; // three dictionary check bytes to go
        const int DICT4 = 2; // four dictionary check bytes to go
        const int DONE = 12; // finished check, done
        const int FLAG = 1; // waiting for flag byte
        const int MAX_WBITS = 15; // 32K LZ77 window
        const int METHOD = 0; // waiting for method byte
        const int PRESET_DICT = 0x20;
        const int Z_BUF_ERROR = - 5;
        const int Z_DATA_ERROR = - 3;

        const int Z_DEFLATED = 8;
        const int Z_ERRNO = - 1;
        const int Z_MEM_ERROR = - 4;
        const int Z_NEED_DICT = 2;

        const int Z_OK = 0;
        const int Z_STREAM_END = 1;
        const int Z_STREAM_ERROR = - 2;
        const int Z_VERSION_ERROR = - 6;
        static readonly byte[] mark = new byte[] { 0, 0, (byte)SupportClass.Identity(0xff), (byte)SupportClass.Identity(0xff) };
        internal InfBlocks blocks; // current inflate_blocks state
        internal int marker;

        // mode dependent information
        internal int method; // if FLAGS, method byte
        internal int mode; // current inflate mode

        // if CHECK, check values to compare
        internal long need; // stream check value

        // if BAD, inflateSync's marker bytes count

        // mode independent information
        internal int nowrap; // flag for no wrapper
        internal long[] was = new long[1]; // computed check value
        internal int wbits; // log2(window size)  (8..15, defaults to 15)

        internal int inflate(ZStream z, int f)
        {
            int r;
            int b;

            if (z == null || z.istate == null || z.next_in == null)
                return Z_STREAM_ERROR;
            f = f == Z_FINISH ? Z_BUF_ERROR : Z_OK;
            r = Z_BUF_ERROR;
            while (true)
            {
                //System.out.println("mode: "+z.istate.mode);
                switch (z.istate.mode)
                {
                    case METHOD:

                        if (z.avail_in == 0)
                            return r;
                        r = f;

                        z.avail_in--;
                        z.total_in++;
                        if (((z.istate.method = z.next_in[z.next_in_index++]) & 0xf) != Z_DEFLATED)
                        {
                            z.istate.mode = BAD;
                            z.msg = "unknown compression method";
                            z.istate.marker = 5; // can't try inflateSync
                            break;
                        }
                        if ((z.istate.method >> 4) + 8 > z.istate.wbits)
                        {
                            z.istate.mode = BAD;
                            z.msg = "invalid window size";
                            z.istate.marker = 5; // can't try inflateSync
                            break;
                        }
                        z.istate.mode = FLAG;
                        goto case FLAG;

                    case FLAG:

                        if (z.avail_in == 0)
                            return r;
                        r = f;

                        z.avail_in--;
                        z.total_in++;
                        b = (z.next_in[z.next_in_index++]) & 0xff;

                        if ((((z.istate.method << 8) + b) % 31) != 0)
                        {
                            z.istate.mode = BAD;
                            z.msg = "incorrect header check";
                            z.istate.marker = 5; // can't try inflateSync
                            break;
                        }

                        if ((b & PRESET_DICT) == 0)
                        {
                            z.istate.mode = BLOCKS;
                            break;
                        }
                        z.istate.mode = DICT4;
                        goto case DICT4;

                    case DICT4:

                        if (z.avail_in == 0)
                            return r;
                        r = f;

                        z.avail_in--;
                        z.total_in++;
                        z.istate.need = ((z.next_in[z.next_in_index++] & 0xff) << 24) & unchecked((int)0xff000000L);
                        z.istate.mode = DICT3;
                        goto case DICT3;

                    case DICT3:

                        if (z.avail_in == 0)
                            return r;
                        r = f;

                        z.avail_in--;
                        z.total_in++;
                        z.istate.need += (((z.next_in[z.next_in_index++] & 0xff) << 16) & 0xff0000L);
                        z.istate.mode = DICT2;
                        goto case DICT2;

                    case DICT2:

                        if (z.avail_in == 0)
                            return r;
                        r = f;

                        z.avail_in--;
                        z.total_in++;
                        z.istate.need += (((z.next_in[z.next_in_index++] & 0xff) << 8) & 0xff00L);
                        z.istate.mode = DICT1;
                        goto case DICT1;

                    case DICT1:

                        if (z.avail_in == 0)
                            return r;
                        r = f;

                        z.avail_in--;
                        z.total_in++;
                        z.istate.need += (z.next_in[z.next_in_index++] & 0xffL);
                        z.adler = z.istate.need;
                        z.istate.mode = DICT0;
                        return Z_NEED_DICT;

                    case DICT0:
                        z.istate.mode = BAD;
                        z.msg = "need dictionary";
                        z.istate.marker = 0; // can try inflateSync
                        return Z_STREAM_ERROR;

                    case BLOCKS:

                        r = z.istate.blocks.proc(z, r);
                        if (r == Z_DATA_ERROR)
                        {
                            z.istate.mode = BAD;
                            z.istate.marker = 0; // can try inflateSync
                            break;
                        }
                        if (r == Z_OK)
                            r = f;
                        if (r != Z_STREAM_END)
                            return r;
                        r = f;
                        z.istate.blocks.reset(z, z.istate.was);
                        if (z.istate.nowrap != 0)
                        {
                            z.istate.mode = DONE;
                            break;
                        }
                        z.istate.mode = CHECK4;
                        goto case CHECK4;

                    case CHECK4:

                        if (z.avail_in == 0)
                            return r;
                        r = f;

                        z.avail_in--;
                        z.total_in++;
                        z.istate.need = ((z.next_in[z.next_in_index++] & 0xff) << 24) & unchecked((int)0xff000000L);
                        z.istate.mode = CHECK3;
                        goto case CHECK3;

                    case CHECK3:

                        if (z.avail_in == 0)
                            return r;
                        r = f;

                        z.avail_in--;
                        z.total_in++;
                        z.istate.need += (((z.next_in[z.next_in_index++] & 0xff) << 16) & 0xff0000L);
                        z.istate.mode = CHECK2;
                        goto case CHECK2;

                    case CHECK2:

                        if (z.avail_in == 0)
                            return r;
                        r = f;

                        z.avail_in--;
                        z.total_in++;
                        z.istate.need += (((z.next_in[z.next_in_index++] & 0xff) << 8) & 0xff00L);
                        z.istate.mode = CHECK1;
                        goto case CHECK1;

                    case CHECK1:

                        if (z.avail_in == 0)
                            return r;
                        r = f;

                        z.avail_in--;
                        z.total_in++;
                        z.istate.need += (z.next_in[z.next_in_index++] & 0xffL);

                        if (((int)(z.istate.was[0])) != ((int)(z.istate.need)))
                        {
                            z.istate.mode = BAD;
                            z.msg = "incorrect data check";
                            z.istate.marker = 5; // can't try inflateSync
                            break;
                        }

                        z.istate.mode = DONE;
                        goto case DONE;

                    case DONE:
                        return Z_STREAM_END;

                    case BAD:
                        return Z_DATA_ERROR;

                    default:
                        return Z_STREAM_ERROR;
                }
            }
        }

        internal int inflateEnd(ZStream z)
        {
            if (blocks != null)
                blocks.free(z);
            blocks = null;
            //    ZFREE(z, z->state);
            return Z_OK;
        }

        internal int inflateInit(ZStream z, int w)
        {
            z.msg = null;
            blocks = null;

            // handle undocumented nowrap option (no zlib header or check)
            nowrap = 0;
            if (w < 0)
            {
                w = - w;
                nowrap = 1;
            }

            // set window size
            if (w < 8 || w > 15)
            {
                inflateEnd(z);
                return Z_STREAM_ERROR;
            }
            wbits = w;

            z.istate.blocks = new InfBlocks(z, z.istate.nowrap != 0 ? null : this, 1 << w);

            // reset state
            inflateReset(z);
            return Z_OK;
        }

        internal int inflateReset(ZStream z)
        {
            if (z == null || z.istate == null)
                return Z_STREAM_ERROR;

            z.total_in = z.total_out = 0;
            z.msg = null;
            z.istate.mode = z.istate.nowrap != 0 ? BLOCKS : METHOD;
            z.istate.blocks.reset(z, null);
            return Z_OK;
        }

        internal int inflateSetDictionary(ZStream z, byte[] dictionary, int dictLength)
        {
            int index = 0;
            int length = dictLength;
            if (z == null || z.istate == null || z.istate.mode != DICT0)
                return Z_STREAM_ERROR;

            if (z._adler.adler32(1L, dictionary, 0, dictLength) != z.adler)
                return Z_DATA_ERROR;

            z.adler = z._adler.adler32(0, null, 0, 0);

            if (length >= (1 << z.istate.wbits))
            {
                length = (1 << z.istate.wbits) - 1;
                index = dictLength - length;
            }
            z.istate.blocks.set_dictionary(dictionary, index, length);
            z.istate.mode = BLOCKS;
            return Z_OK;
        }

        internal int inflateSync(ZStream z)
        {
            int n; // number of bytes to look at
            int p; // pointer to bytes
            int m; // number of marker bytes found in a row
            long r, w; // temporaries to save total_in and total_out

            // set up
            if (z == null || z.istate == null)
                return Z_STREAM_ERROR;
            if (z.istate.mode != BAD)
            {
                z.istate.mode = BAD;
                z.istate.marker = 0;
            }
            if ((n = z.avail_in) == 0)
                return Z_BUF_ERROR;
            p = z.next_in_index;
            m = z.istate.marker;

            // search
            while (n != 0 && m < 4)
            {
                if (z.next_in[p] == mark[m])
                    m++;
                else if (z.next_in[p] != 0)
                    m = 0;
                else
                    m = 4 - m;
                p++;
                n--;
            }

            // restore
            z.total_in += p - z.next_in_index;
            z.next_in_index = p;
            z.avail_in = n;
            z.istate.marker = m;

            // return no joy or set up to restart on a new block
            if (m != 4)
                return Z_DATA_ERROR;
            r = z.total_in;
            w = z.total_out;
            inflateReset(z);
            z.total_in = r;
            z.total_out = w;
            z.istate.mode = BLOCKS;
            return Z_OK;
        }

        // Returns true if inflate is currently at the end of a block generated
        // by Z_SYNC_FLUSH or Z_FULL_FLUSH. This function is used by one PPP
        // implementation to provide an additional safety check. PPP uses Z_SYNC_FLUSH
        // but removes the length bytes of the resulting empty stored block. When
        // decompressing, PPP checks that at the end of input packet, inflate is
        // waiting for these length bytes.
        internal int inflateSyncPoint(ZStream z)
        {
            if (z == null || z.istate == null || z.istate.blocks == null)
                return Z_STREAM_ERROR;
            return z.istate.blocks.sync_point();
        }
    }
}