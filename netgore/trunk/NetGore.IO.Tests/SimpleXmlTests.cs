using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        #region IRestorableSettings Members

        public void Load(IDictionary<string, string> items)
        {
            A = items["A"];
            B = int.Parse(items["B"]);
            C = float.Parse(items["C"]);
        }

        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[] { new NodeItem("A", A), new NodeItem("B", B), new NodeItem("C", C) };
        }

        #endregion
    }

    [TestFixture]
    public class SimpleXmlTests
    {
        static void AreNodeValuesEqual(NodeItem[] expected, NodeItem[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Name, actual[i].Name);
                Assert.AreEqual(expected[i].Value, actual[i].Value);
            }
        }

        [Test]
        public void IRestorableSettingsTest()
        {
            string path = Path.GetTempFileName();
            try
            {
                InterfaceTester t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
                InterfaceTester t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };

                using (SimpleXmlWriter w = new SimpleXmlWriter(path, "Tester"))
                {
                    w.Write("t1", t1);
                    w.Write("t2", t2);
                }

                InterfaceTester newT1 = new InterfaceTester();
                InterfaceTester newT2 = new InterfaceTester();

                SimpleXmlReader r = new SimpleXmlReader(path);
                foreach (NodeItems item in r.Items)
                {
                    if (item.Name == "t1")
                        newT1.Load(item.ToDictionary());
                    else if (item.Name == "t2")
                        newT2.Load(item.ToDictionary());
                    else
                        Assert.Fail("Invalid node name.");
                }

                Assert.IsTrue(InterfaceTester.HaveSameValues(t1, newT1));
                Assert.IsTrue(InterfaceTester.HaveSameValues(t2, newT2));
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Test]
        public void SimpleXmlReadWriteTest()
        {
            string path = Path.GetTempFileName();
            try
            {
                // Create lookup dictionary
                var dict = new Dictionary<string, IEnumerable<NodeItem>>
                           {
                               {
                                   "Form1",
                                   new NodeItem[]
                                   { new NodeItem("Name", "MyForm1"), new NodeItem("X", 100), new NodeItem("Y", 244) }
                                   },
                               {
                                   "Form2",
                                   new NodeItem[]
                                   { new NodeItem("Name", "MyForm2"), new NodeItem("X", 140), new NodeItem("Y", 224) }
                                   },
                               {
                                   "QuickBar1",
                                   new NodeItem[]
                                   { new NodeItem("Slot", 1), new NodeItem("Type", "Item"), new NodeItem("Value", 4) }
                                   },
                               {
                                   "QuickBar2",
                                   new NodeItem[]
                                   { new NodeItem("Slot", 1), new NodeItem("Type", "Item"), new NodeItem("Value", 10) }
                                   }
                           };

                // Write values
                using (SimpleXmlWriter w = new SimpleXmlWriter(path, "RootTestNode"))
                {
                    foreach (var pair in dict)
                    {
                        w.Write(pair.Key, pair.Value);
                    }
                }

                // Read values
                SimpleXmlReader r = new SimpleXmlReader(path);
                Assert.AreEqual("RootTestNode", r.RootNodeName);

                var items = r.Items.ToArray();

                int i = 0;
                foreach (var pair in dict)
                {
                    NodeItems item = items[i];
                    Assert.AreEqual(pair.Key, item.Name);
                    AreNodeValuesEqual(pair.Value.ToArray(), item.Items.ToArray());
                    i++;
                }
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
}