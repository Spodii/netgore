using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    class GenericListener : TraceListener
    {
        readonly StringBuilder partial;
        readonly StringCollection strings;

        public GenericListener()
        {
            strings = new StringCollection();
            partial = new StringBuilder();
        }

        public void Clear()
        {
            partial.Remove(0, partial.Length);
            strings.Clear();
        }

        public int Find(string sToFind)
        {
            int count = 0;
            foreach (string s in strings)
            {
                if (s.IndexOf(sToFind) != -1)
                    count++;
            }
            return count;
        }

        public override void Write(string message)
        {
            partial.Append(message);
        }

        public override void WriteLine(string message)
        {
            Write(message);
            strings.Add(partial.ToString());
            partial.Remove(0, partial.Length);
        }
    }

    /// <summary>
    /// Summary description for ConnectionTests.
    /// </summary>
    [TestFixture]
    public class ThreadingTests : BaseTest
    {
        void MultipleThreadsWorker(object ev)
        {
            (ev as ManualResetEvent).WaitOne();

            try
            {
                MySqlConnection c = new MySqlConnection(GetConnectionString(true));
                c.Open();
                c.Close();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Bug #17106 MySql.Data.MySqlClient.CharSetMap.GetEncoding thread synchronization issue
        /// </summary>
        [Test]
        public void MultipleThreads()
        {
            GenericListener myListener = new GenericListener();
            ManualResetEvent ev = new ManualResetEvent(false);
            ArrayList threads = new ArrayList();
            Trace.Listeners.Add(myListener);

            for (int i = 0; i < 20; i++)
            {
                ParameterizedThreadStart ts = MultipleThreadsWorker;
                Thread t = new Thread(ts);
                threads.Add(t);
                t.Start(ev);
            }
            // now let the threads go
            ev.Set();

            // wait for the threads to end
            int x = 0;
            while (x < threads.Count)
            {
                while ((threads[x] as Thread).IsAlive)
                {
                    Thread.Sleep(50);
                }
                x++;
            }
        }
    }
}