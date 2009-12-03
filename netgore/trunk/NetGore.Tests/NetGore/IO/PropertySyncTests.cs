using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class PropertySyncTests
    {
        /// <summary>
        /// Test class containing all the values to test the syncing on
        /// </summary>
        class TestClass
        {
            [SyncValue]
            public bool mBool { get; set; }

            [SyncValue]
            public byte mByte { get; set; }

            [SyncValue]
            public CollisionType mCollisionType { get; set; }

            [SyncValue]
            public Color mColor { get; set; }

            [SyncValue]
            public double mDouble { get; set; }

            [SyncValue]
            public float mFloat { get; set; }

            [SyncValue]
            public GrhIndex mGrhIndex { get; set; }

            [SyncValue]
            public int mInt { get; set; }

            [SyncValue]
            public long mLong { get; set; }

            [SyncValue]
            public MapEntityIndex mMapEntityIndex { get; set; }

            [SyncValue]
            public sbyte mSByte { get; set; }

            [SyncValue]
            public short mShort { get; set; }

            [SyncValue]
            public string mString { get; set; }

            [SyncValue]
            public uint mUInt { get; set; }

            [SyncValue]
            public ulong mULong { get; set; }

            [SyncValue]
            public ushort mUShort { get; set; }

            [SyncValue]
            public Vector2 mVector2 { get; set; }

            readonly PropertySyncBase[] _propertySyncs;

            public TestClass()
            {
                _propertySyncs = PropertySyncBase.GetPropertySyncs(this).ToArray();
            }

            public void Deserialize(IValueReader reader)
            {
                int count = reader.ReadByte("Count");

                for (int i = 0; i < count; i++)
                {
                    int propIndex = reader.ReadByte("PropertyIndex" + i);
                    _propertySyncs[propIndex].ReadValue(reader);
                }
            }

            public void Serialize(IValueWriter writer)
            {
                Queue<int> changed = new Queue<int>();

                for (int i = 0; i < _propertySyncs.Length; i++)
                {
                    if (_propertySyncs[i].HasValueChanged())
                        changed.Enqueue(i);
                }

                writer.Write("Count", (byte)changed.Count);

                int index = 0;
                foreach (var i in changed)
                {
                    writer.Write("PropertyIndex" + index++, (byte)i);
                    _propertySyncs[i].WriteValue(writer);
                }
            }
        }

        static void TestSync(string propertyName, object value)
        {
            // Get the property
            PropertyInfo property = typeof(TestClass).GetProperty(propertyName);

            // Set the value and ensure it was set correctly
            TestClass cSrc = new TestClass();
            property.SetValue(cSrc, value, null);
            Assert.AreEqual(value, property.GetValue(cSrc, null));

            // Serialize
            BitStream bs = new BitStream(BitStreamMode.Write, 64);
            cSrc.Serialize(bs);

            // Deserialize
            TestClass cDest = new TestClass();
            bs.Mode = BitStreamMode.Read;
            cDest.Deserialize(bs);

            // Check
            Assert.AreEqual(value, property.GetValue(cDest, null));
        }

        [Test]
        public void FloatTest() { TestSync("mFloat", 57.0f); }

        [Test]
        public void ByteTest() { TestSync("mByte", (byte)5); }

        [Test]
        public void BoolTest() { TestSync("mBool", true); }

        [Test]
        public void CollisionTypeTest() { TestSync("mCollisionType", CollisionType.Full); }

        [Test]
        public void ColorTest() { TestSync("mColor", new Color(255, 243, 234, 12)); }

        [Test]
        public void GrhIndexTest() { TestSync("mGrhIndex", new GrhIndex(5)); }

        [Test]
        public void IntTest() { TestSync("mInt", 5); }

        [Test]
        public void LongTest() { TestSync("mLong", (long)1032); }

        [Test]
        public void MapEntityIndexTest() { TestSync("mMapEntityIndex", new MapEntityIndex(55)); }

        [Test]
        public void SByteTest() { TestSync("mSByte", (sbyte)23); }

        [Test]
        public void ShortTest() { TestSync("mShort", (short)23); }

        [Test]
        public void UIntTest() { TestSync("mUInt", (uint)23); }

        [Test]
        public void ULongTest() { TestSync("mULong", (ulong)23); }

        [Test]
        public void UShortTest() { TestSync("mUShort", (ushort)23); }

        [Test]
        public void Vector2Test() { TestSync("mVector2", new Vector2(23,32)); }
    }
}
