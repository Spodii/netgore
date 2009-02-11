using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using MySql.Data.MySqlClient;

namespace MySql.Data.Common
{
    /// <summary>
    /// Summary description for SharedMemoryStream.
    /// </summary>
    class SharedMemoryStream : Stream
    {
        const int BUFFERLENGTH = 16004;
        const uint EVENT_MODIFY_STATE = 0x2;
//		private const uint EVENT_ALL_ACCESS = 0x001F0003;
        const uint FILE_MAP_WRITE = 0x2;
        const uint SYNCHRONIZE = 0x00100000;
        readonly string memoryName;
        int bytesLeft;
        AutoResetEvent clientRead;
        AutoResetEvent clientWrote;
        int connectNumber;
        IntPtr dataMap;
        IntPtr dataView;
        int position;
        AutoResetEvent serverRead;
        AutoResetEvent serverWrote;

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { throw new NotSupportedException("SharedMemoryStream does not support seeking - length"); }
        }

        public override long Position
        {
            get { throw new NotSupportedException("SharedMemoryStream does not support seeking - postition"); }
            set { }
        }

        public SharedMemoryStream(string memName)
        {
            memoryName = memName;
        }

        public override void Close()
        {
            UnmapViewOfFile(dataView);
            CloseHandle(dataMap);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int CloseHandle(IntPtr hObject);

        public override void Flush()
        {
            FlushViewOfFile(dataView, 0);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int FlushViewOfFile(IntPtr address, uint numBytes);

        void GetConnectNumber(uint timeOut)
        {
            AutoResetEvent connectRequest = new AutoResetEvent(false);
            IntPtr handle = OpenEvent(SYNCHRONIZE | EVENT_MODIFY_STATE, false, memoryName + "_" + "CONNECT_REQUEST");
            connectRequest.SafeWaitHandle = new SafeWaitHandle(handle, true);

            AutoResetEvent connectAnswer = new AutoResetEvent(false);
            handle = OpenEvent(SYNCHRONIZE | EVENT_MODIFY_STATE, false, memoryName + "_" + "CONNECT_ANSWER");
            connectAnswer.SafeWaitHandle = new SafeWaitHandle(handle, true);

            IntPtr connectFileMap = OpenFileMapping(FILE_MAP_WRITE, false, memoryName + "_" + "CONNECT_DATA");
            IntPtr connectView = MapViewOfFile(connectFileMap, FILE_MAP_WRITE, 0, 0, (IntPtr)4);

            // now start the connection
            if (!connectRequest.Set())
                throw new MySqlException("Failed to open shared memory connection");

            connectAnswer.WaitOne((int)(timeOut * 1000), false);

            connectNumber = Marshal.ReadInt32(connectView);
        }

        public bool IsClosed()
        {
            try
            {
                dataView = MapViewOfFile(dataMap, FILE_MAP_WRITE, 0, 0, (IntPtr)BUFFERLENGTH);
                if (dataView == IntPtr.Zero)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh,
                                           uint dwFileOffsetLow, IntPtr dwNumberOfBytesToMap);

        public void Open(uint timeOut)
        {
            GetConnectNumber(timeOut);
            SetupEvents();
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenEvent(uint dwDesiredAccess, bool bInheritHandle, string lpName);

        //		[DllImport("kernel32.dll")]
        //		static extern bool SetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenFileMapping(uint dwDesiredAccess, bool bInheritHandle, string lpName);

        public override int Read(byte[] buffer, int offset, int count)
        {
            while (bytesLeft == 0)
            {
                while (!serverWrote.WaitOne(500, false))
                {
                    if (IsClosed())
                        return 0;
                }

                bytesLeft = Marshal.ReadInt32(dataView);
                position = 4;
            }

            int len = Math.Min(count, bytesLeft);
            long baseMem = dataView.ToInt64() + position;

            for (int i = 0; i < len; i++, position++)
            {
                buffer[offset + i] = Marshal.ReadByte((IntPtr)(baseMem + i));
            }

            bytesLeft -= len;

            if (bytesLeft == 0)
                clientRead.Set();

            return len;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("SharedMemoryStream does not support seeking");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("SharedMemoryStream does not support seeking");
        }

        void SetupEvents()
        {
            string dataMemoryName = memoryName + "_" + connectNumber;
            dataMap = OpenFileMapping(FILE_MAP_WRITE, false, dataMemoryName + "_DATA");
            dataView = MapViewOfFile(dataMap, FILE_MAP_WRITE, 0, 0, (IntPtr)BUFFERLENGTH);

            serverWrote = new AutoResetEvent(false);
            IntPtr handle = OpenEvent(SYNCHRONIZE | EVENT_MODIFY_STATE, false, dataMemoryName + "_SERVER_WROTE");
            Debug.Assert(handle != IntPtr.Zero);
            serverWrote.SafeWaitHandle = new SafeWaitHandle(handle, true);

            serverRead = new AutoResetEvent(false);
            handle = OpenEvent(SYNCHRONIZE | EVENT_MODIFY_STATE, false, dataMemoryName + "_SERVER_READ");
            Debug.Assert(handle != IntPtr.Zero);
            serverRead.SafeWaitHandle = new SafeWaitHandle(handle, true);

            clientWrote = new AutoResetEvent(false);
            handle = OpenEvent(SYNCHRONIZE | EVENT_MODIFY_STATE, false, dataMemoryName + "_CLIENT_WROTE");
            Debug.Assert(handle != IntPtr.Zero);
            clientWrote.SafeWaitHandle = new SafeWaitHandle(handle, true);

            clientRead = new AutoResetEvent(false);
            handle = OpenEvent(SYNCHRONIZE | EVENT_MODIFY_STATE, false, dataMemoryName + "_CLIENT_READ");
            Debug.Assert(handle != IntPtr.Zero);
            clientRead.SafeWaitHandle = new SafeWaitHandle(handle, true);

            // tell the server we are ready
            serverRead.Set();
        }

        [DllImport("kernel32.dll")]
        static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        public override void Write(byte[] buffer, int offset, int count)
        {
            int leftToDo = count;
            int buffPos = offset;

            while (leftToDo > 0)
            {
                if (!serverRead.WaitOne())
                    throw new MySqlException("Writing to shared memory failed");

                int bytesToDo = Math.Min(leftToDo, BUFFERLENGTH);

                long baseMem = dataView.ToInt64() + 4;
                Marshal.WriteInt32(dataView, bytesToDo);
                for (int i = 0; i < bytesToDo; i++, buffPos++)
                {
                    Marshal.WriteByte((IntPtr)(baseMem + i), buffer[buffPos]);
                }
                leftToDo -= bytesToDo;
                if (!clientWrote.Set())
                    throw new MySqlException("Writing to shared memory failed");
            }
        }
    }
}