using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace PngOptimizer
{
    class Optimizer
    {
        readonly Queue<string> _fileQueue = new Queue<string>();
        readonly object _fileQueueSync = new object();
        readonly int _rDirLen;
        readonly int _startingSize;

        public Optimizer(IEnumerable<string> files, int rDirLen)
        {
            _rDirLen = rDirLen;
            _fileQueue = new Queue<string>(files.Where(x => !string.IsNullOrEmpty(x)));
            _startingSize = _fileQueue.Count;
        }

        static void Cmd(Process proc, string app, string args, params string[] p)
        {
            if (p != null && p.Length > 0)
                args = string.Format(args, p);

            var psi = new ProcessStartInfo(app, args) { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden };
            proc.StartInfo = psi;

            try
            {
                proc.PriorityClass = ProcessPriorityClass.Idle;
            }
            catch
            {
            }

            proc.Start();

            try
            {
                proc.WaitForExit();
            }
            catch
            {
            }
        }

        Thread CreateWorkerThread()
        {
            var t = new Thread(WorkerLoop) { Priority = ThreadPriority.Lowest, IsBackground = false };
            return t;
        }

        static int GetThreadsCount()
        {
            return Environment.ProcessorCount;
        }

        static void ProcessFile(Process proc, string file)
        {
            try
            {
                if (!File.Exists(file))
                {
                    Debug.Fail(string.Format("File `{0}` does not exist.", file));
                    return;
                }

                Cmd(proc, "optipng.exe", "-quiet -fix -i0 -o7 \"{0}\" ", file);
            }
            catch (Exception ex)
            {
                Program.Print("Failed to process file `{0}`. Exception: {1}", file, ex);
            }
        }

        public void Run()
        {
            lock (_fileQueueSync)
            {
                if (_fileQueue.Count == 0)
                    return;
            }

            var threads = new List<Thread>();

            for (var i = 0; i < GetThreadsCount(); i++)
            {
                var t = CreateWorkerThread();
                t.Name = "Worker thread " + i;
                threads.Add(t);
            }

            foreach (var t in threads)
            {
                t.Start();
            }

            foreach (var t in threads)
            {
                try
                {
                    if (t.IsAlive)
                        t.Join();
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }
        }

        void WorkerLoop()
        {
            var proc = new Process();

            while (true)
            {
                string file = null;
                int remCount;

                lock (_fileQueueSync)
                {
                    remCount = _fileQueue.Count;
                    if (remCount > 0)
                        file = _fileQueue.Dequeue();
                }

                if (file == null)
                    return;

                var curr = _startingSize - remCount;
                var percent = Math.Round((curr / (float)_startingSize) * 100f, 0);

                string subPath;
                try
                {
                    subPath = file.Substring(_rDirLen);
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.ToString());
                    subPath = file;
                }

                Program.Print("[{0}%] {1}", percent, subPath);

                ProcessFile(proc, file);
            }
        }
    }
}