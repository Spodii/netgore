// Copyright (C) 2004-2007 MySQL AB
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License version 2 as published by
// the Free Software Foundation
//
// There are special exceptions to the terms and conditions of the GPL 
// as it is applied to this software. View the full text of the 
// exception in file EXCEPTIONS in the directory of this software 
// distribution.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace MySql.Data.Common
{
	/// <summary>
	/// Summary description for MySqlSocket.
	/// </summary>
	internal sealed class SocketStream : Stream
	{
		private Socket	socket;

		public SocketStream(AddressFamily addressFamily, SocketType socketType, ProtocolType protocol)
		{
			socket = new Socket(addressFamily, socketType, protocol);
		}

		#region Properties

		public Socket Socket 
		{
			get { return socket; }
		}

		public override bool CanRead
		{
			get	{ return true;	}
		}

		public override bool CanSeek
		{
			get	{ return false;	}
		}

		public override bool CanWrite
		{
			get	{ return true; }
		}

		public override long Length
		{
			get	{ return 0;	}
		}

		public override long Position
		{
			get	{ return 0;	}
			set	{ throw new NotSupportedException("SocketStream does not support seek"); }
		}

		#endregion

		#region Stream Implementation

		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return socket.Receive(buffer, offset, count, SocketFlags.None);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("SocketStream does not support seek");
		}

		public override void SetLength(long value)
		{
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			socket.Send(buffer, offset, count, SocketFlags.None);
		}


		#endregion

		public bool Connect(EndPoint remoteEP, int timeout)
		{
            // set the socket to non blocking
            socket.Blocking = false;

			// then we star the connect
			SocketAddress addr = remoteEP.Serialize();
			byte[] buff = new byte[addr.Size];
			for (int i=0; i<addr.Size; i++)
				buff[i] = addr[i];

			NativeMethods.connect(socket.Handle, buff, addr.Size);
            int wsaerror = NativeMethods.WSAGetLastError();
            if (wsaerror != 10035)
            {
                //  this is probably an IPV6 address
                if (wsaerror == 10047)
                    return false;
                throw new SocketException(wsaerror);
            }

			// next we wait for our connect timeout or until the socket is connected
			ArrayList write = new ArrayList();
			write.Add(socket);
			ArrayList error = new ArrayList();
			error.Add(socket);

			Socket.Select(null, write, error, timeout*1000*1000);

			if (write.Count == 0) return false;

			// set socket back to blocking mode
            socket.Blocking = true;
			return true;
		}


	}
}
