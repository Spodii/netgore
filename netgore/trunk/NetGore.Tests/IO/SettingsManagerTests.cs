using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetGore;
using NetGore.Globalization;
using NUnit.Framework;

namespace NetGore.IO.Tests
{
    class InterfaceTester : IRestorableSettings
    {
        public string A;
        public int B;
        public float C;

        public static bool HaveSameValues(InterfaceTester l, InterfaceTester r)
        {
            return (l.A == r.A) && (l.B == r.B) && (l.C == r.C);
        }

        public bool HaveSameValues(InterfaceTester other)
        {
            return HaveSameValues(this, other);
        }

        #region IRestorableSettings Members

        /// <summary>
        /// Loads the values supplied by the <paramref name="items"/> to reconstruct the settings.
        /// </summary>
        /// <param name="items">NodeItems containing the values to restore.</param>
        public void Load(IDictionary<string, string> items)
        {
            A = items["A"];
            B = Parser.Invariant.ParseInt(items["B"]);
            C = Parser.Invariant.ParseFloat(items["C"]);
        }

        /// <summary>
        /// Returns the key and value pairs needed to restore the settings.
        /// </summary>
        /// <returns>The key and value pairs needed to restore the settings.</returns>
        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[] { new NodeItem("A", A), new NodeItem("B", B), new NodeItem("C", C) };
        }

        #endregion
    }

    [TestFixture]
    public class SettingsManagerTests
    {
        static readonly Random r = new Random();

        [Test]
        public void AddNewItemTest()
        {
            InterfaceTester t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            InterfaceTester t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            InterfaceTester t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            InterfaceTester retT1 = new InterfaceTester();
            InterfaceTester retT2 = new InterfaceTester();
            InterfaceTester retT3;

            string filePath = Path.GetTempFileName();

            try
            {
                using (SettingsManager settingsWriter = new SettingsManager("TestSettings", filePath))
                {
                    settingsWriter.Add("t1", t1);
                    settingsWriter.Add("t2", t2);
                }

                using (SettingsManager settingsReader = new SettingsManager("TestSettings", filePath))
                {
                    settingsReader.Add("t3", t3);
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                    retT3 = t3;
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        [Test]
        public void CaseInsensitiveLowerTest()
        {
            InterfaceTester t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            InterfaceTester t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            InterfaceTester t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            InterfaceTester retT1 = new InterfaceTester();
            InterfaceTester retT2 = new InterfaceTester();
            InterfaceTester retT3 = new InterfaceTester();

            string filePath = Path.GetTempFileName();

            try
            {
                using (SettingsManager settingsWriter = new SettingsManager("TestSettings", filePath))
                {
                    settingsWriter.Add("T2", t2);
                    settingsWriter.Add("T3", t3);
                    settingsWriter.Add("T1", t1);
                }

                using (SettingsManager settingsReader = new SettingsManager("TestSettings", filePath))
                {
                    settingsReader.Add("t3", retT3);
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        [Test]
        public void CaseInsensitiveUpperTest()
        {
            InterfaceTester t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            InterfaceTester t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            InterfaceTester t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            InterfaceTester retT1 = new InterfaceTester();
            InterfaceTester retT2 = new InterfaceTester();
            InterfaceTester retT3 = new InterfaceTester();

            string filePath = Path.GetTempFileName();

            try
            {
                using (SettingsManager settingsWriter = new SettingsManager("TestSettings", filePath))
                {
                    settingsWriter.Add("t2", t2);
                    settingsWriter.Add("t3", t3);
                    settingsWriter.Add("t1", t1);
                }

                using (SettingsManager settingsReader = new SettingsManager("TestSettings", filePath))
                {
                    settingsReader.Add("T3", retT3);
                    settingsReader.Add("T1", retT1);
                    settingsReader.Add("T2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        static string CreateRandomString()
        {
            int length = r.Next(2, 10);
            var ret = new char[length];

            for (int i = 0; i < ret.Length; i++)
            {
                int j;
                if (i == 0)
                    j = r.Next(1, 3); // Don't let the first character be numeric
                else
                    j = r.Next(0, 3);

                int min;
                int max;

                // ReSharper disable RedundantCast
                switch (j)
                {
                    case 0:
                        min = (int)'0';
                        max = (int)'9';
                        break;

                    case 1:
                        min = (int)'a';
                        max = (int)'z';
                        break;

                    default:
                        min = (int)'A';
                        max = (int)'Z';
                        break;
                }
                // ReSharper restore RedundantCast

                ret[i] = (char)r.Next(min, max + 1);
            }

            return new string(ret);
        }

        [Test]
        public void DuplicateKeyTest()
        {
            InterfaceTester t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            InterfaceTester t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };

            string filePath = Path.GetTempFileName();

            try
            {
                using (SettingsManager settingsWriter = new SettingsManager("TestSettings", filePath))
                {
                    settingsWriter.Add("t1", t1);
                    Assert.Throws<ArgumentException>(() => settingsWriter.Add("t1", t2));
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        static InterfaceTester GetRandomInterfaceTester()
        {
            return new InterfaceTester
            { A = CreateRandomString(), B = r.Next(int.MinValue, int.MaxValue), C = (r.Next(-1000, 1000)) };
        }

        [Test]
        public void MissingItemTest()
        {
            InterfaceTester t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            InterfaceTester t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            InterfaceTester t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            InterfaceTester retT1 = new InterfaceTester();
            InterfaceTester retT2 = new InterfaceTester();
            InterfaceTester retT3 = new InterfaceTester();

            string filePath = Path.GetTempFileName();

            try
            {
                using (SettingsManager settingsWriter = new SettingsManager("TestSettings", filePath))
                {
                    settingsWriter.Add("t2", t2);
                    settingsWriter.Add("t3", t3);
                }

                using (SettingsManager settingsReader = new SettingsManager("TestSettings", filePath))
                {
                    settingsReader.Add("t3", retT3);
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        [Test]
        public void ReadWriteTonsOfCrapTest()
        {
            const int iterations = 10000;

            var items = new List<InterfaceTester>(iterations);

            for (int i = 0; i < iterations; i++)
            {
                items.Add(GetRandomInterfaceTester());
            }

            string filePath = Path.GetTempFileName();

            try
            {
                using (SettingsManager settingsWriter = new SettingsManager("TestSettings", filePath))
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        settingsWriter.Add("item" + Parser.Invariant.ToString(i), items[i]);
                    }
                }

                using (SettingsManager settingsReader = new SettingsManager("TestSettings", filePath))
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        InterfaceTester newItem = new InterfaceTester();
                        settingsReader.Add("item" + Parser.Invariant.ToString(i), newItem);
                        Assert.IsTrue(items[i].HaveSameValues(newItem), "Index: {0}", i);
                    }
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        [Test]
        public void VariableLoadOrderTest()
        {
            InterfaceTester t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            InterfaceTester t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            InterfaceTester t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            InterfaceTester retT1 = new InterfaceTester();
            InterfaceTester retT2 = new InterfaceTester();
            InterfaceTester retT3 = new InterfaceTester();

            string filePath = Path.GetTempFileName();

            try
            {
                using (SettingsManager settingsWriter = new SettingsManager("TestSettings", filePath))
                {
                    settingsWriter.Add("t1", t1);
                    settingsWriter.Add("t2", t2);
                    settingsWriter.Add("t3", t3);
                }

                using (SettingsManager settingsReader = new SettingsManager("TestSettings", filePath))
                {
                    settingsReader.Add("t3", retT3);
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        [Test]
        public void VariableSaveOrderTest()
        {
            InterfaceTester t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            InterfaceTester t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            InterfaceTester t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            InterfaceTester retT1 = new InterfaceTester();
            InterfaceTester retT2 = new InterfaceTester();
            InterfaceTester retT3 = new InterfaceTester();

            string filePath = Path.GetTempFileName();

            try
            {
                using (SettingsManager settingsWriter = new SettingsManager("TestSettings", filePath))
                {
                    settingsWriter.Add("t2", t2);
                    settingsWriter.Add("t3", t3);
                    settingsWriter.Add("t1", t1);
                }

                using (SettingsManager settingsReader = new SettingsManager("TestSettings", filePath))
                {
                    settingsReader.Add("t3", retT3);
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        [Test]
        public void WriteReadTest()
        {
            InterfaceTester t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            InterfaceTester t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };

            InterfaceTester retT1 = new InterfaceTester();
            InterfaceTester retT2 = new InterfaceTester();

            string filePath = Path.GetTempFileName();

            try
            {
                using (SettingsManager settingsWriter = new SettingsManager("TestSettings", filePath))
                {
                    settingsWriter.Add("t1", t1);
                    settingsWriter.Add("t2", t2);
                }

                using (SettingsManager settingsReader = new SettingsManager("TestSettings", filePath))
                {
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
        }

        [Test]
        public void WriteTonsOfCrapReadOnlySomeTest()
        {
            const int iterations = 10000;

            var items = new List<InterfaceTester>(iterations);

            for (int i = 0; i < iterations; i++)
            {
                items.Add(GetRandomInterfaceTester());
            }

            string filePath = Path.GetTempFileName();

            try
            {
                using (SettingsManager settingsWriter = new SettingsManager("TestSettings", filePath))
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        settingsWriter.Add("item" + Parser.Invariant.ToString(i), items[i]);
                    }
                }

                using (SettingsManager settingsReader = new SettingsManager("TestSettings", filePath))
                {
                    for (int i = items.Count - 1; i > 0; i -= 22)
                    {
                        InterfaceTester newItem = new InterfaceTester();
                        settingsReader.Add("item" + Parser.Invariant.ToString(i), newItem);
                        Assert.IsTrue(items[i].HaveSameValues(newItem), "Index: {0}", i);
                    }
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
    }
}