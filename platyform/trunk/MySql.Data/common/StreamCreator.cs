using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace MySql.Data.Common
{
    /// <summary>
    /// Summary description for StreamCreator.
    /// </summary>
    class StreamCreator
    {
        readonly string hostList;
        readonly string pipeName;
        readonly uint port;
        uint timeOut;

        public StreamCreator(string hosts, uint port, string pipeName)
        {
            hostList = hosts;
            if (string.IsNullOrEmpty(hostList))
                hostList = "localhost";
            this.port = port;
            this.pipeName = pipeName;
        }

        Stream CreateNamedPipeStream(string hostname)
        {
            string pipePath;
            if (0 == String.Compare(hostname, "localhost", true))
                pipePath = @"\\.\pipe\" + pipeName;
            else
                pipePath = String.Format(@"\\{0}\pipe\{1}", hostname, pipeName);
            return new NamedPipeStream(pipePath, FileAccess.ReadWrite);
        }

        Stream CreateSocketStream(IPAddress ip, bool unix)
        {
            EndPoint endPoint;

            if (!Platform.IsWindows() && unix)
                endPoint = CreateUnixEndPoint(hostList);
            else
                endPoint = new IPEndPoint(ip, (int)port);

            Socket socket = unix
                                ? new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP)
                                : new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult ias = socket.BeginConnect(endPoint, null, null);
            if (!ias.AsyncWaitHandle.WaitOne((int)timeOut * 1000, false))
            {
                socket.Close();
                return null;
            }
            try
            {
                socket.EndConnect(ias);
            }
            catch (Exception)
            {
                socket.Close();
                return null;
            }
            NetworkStream stream = new NetworkStream(socket, true);
            GC.SuppressFinalize(socket);
            GC.SuppressFinalize(stream);
            return stream;
        }

        static EndPoint CreateUnixEndPoint(string host)
        {
            // first we need to load the Mono.posix assembly
            Assembly a = Assembly.Load("Mono.Posix");

            // then we need to construct a UnixEndPoint object
            EndPoint ep =
                (EndPoint)
                a.CreateInstance("Mono.Posix.UnixEndPoint", false, BindingFlags.CreateInstance, null, new object[] { host }, null,
                                 null);
            return ep;
        }

        static IPHostEntry GetHostEntry(string hostname)
        {
            IPHostEntry ipHE;

            IPAddress addr;
            if (IPAddress.TryParse(hostname, out addr))
            {
                ipHE = new IPHostEntry { AddressList = new IPAddress[1] };
                ipHE.AddressList[0] = addr;
            }
            else
                ipHE = Dns.GetHostEntry(hostname);
            return ipHE;
        }

        public Stream GetStream(uint timeout)
        {
            timeOut = timeout;

            if (hostList.StartsWith("/"))
                return CreateSocketStream(null, true);

            var dnsHosts = hostList.Split('&');

            Random random = new Random((int)DateTime.Now.Ticks);
            int index = random.Next(dnsHosts.Length);
            int pos = 0;
            bool usePipe = (!string.IsNullOrEmpty(pipeName));
            Stream stream = null;

            while (pos < dnsHosts.Length)
            {
                if (usePipe)
                    stream = CreateNamedPipeStream(dnsHosts[index]);
                else
                {
                    IPHostEntry ipHE = GetHostEntry(dnsHosts[index]);
                    foreach (IPAddress address in ipHE.AddressList)
                    {
                        // MySQL doesn't currently support IPv6 addresses
                        if (address.AddressFamily == AddressFamily.InterNetworkV6)
                            continue;

                        stream = CreateSocketStream(address, false);
                        if (stream != null)
                            break;
                    }
                }
                if (stream != null)
                    break;
                index++;
                if (index == dnsHosts.Length)
                    index = 0;
                pos++;
            }

            return stream;
        }
    }
}