using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Features.ActionDisplays;
using NetGore.IO;
using NetGore.IO.PropertySync;
using NetGore.World;
using NUnit.Framework;
using SFML.Graphics;

// ReSharper disable UnusedMember.Local

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class PropertySyncTests
    {
        static void TestSync(string propertyName, object value)
        {
            // Get the property
            var property = typeof(TestClass).GetProperty(propertyName);

            // Set the value and ensure it was set correctly
            var cSrc = new TestClass();
            property.SetValue(cSrc, value, null);
            Assert.AreEqual(value, property.GetValue(cSrc, null));

            // Serialize
            var bs = new BitStream();
            cSrc.Serialize(bs);

            // Deserialize
            var cDest = new TestClass();
            bs.PositionBits = 0;
            cDest.Deserialize(bs);

            // Check
            Assert.AreEqual(value, property.GetValue(cDest, null));
        }

        static void TestSyncNullable(string propertyName, object value)
        {
            // Get the property
            var property = typeof(NullableTestClass).GetProperty(propertyName);

            // Set the value and ensure it was set correctly
            var cSrc = new NullableTestClass();
            property.SetValue(cSrc, value, null);
            Assert.AreEqual(value, property.GetValue(cSrc, null));

            // Serialize
            var bs = new BitStream();
            cSrc.Serialize(bs);

            // Deserialize
            var cDest = new NullableTestClass();
            bs.PositionBits = 0;
            cDest.Deserialize(bs);

            // Check
            Assert.AreEqual(value, property.GetValue(cDest, null));
        }

        #region Unit tests

        [Test]
        public void AutomaticNullableTypesTest()
        {
            TestSyncNullable("mBool", true);
            TestSyncNullable("mByte", null);
            TestSyncNullable("mColor", new Color(255, 243, 234, 12));
            TestSyncNullable("mFloat", null);
            TestSyncNullable("mGrhIndex", null);
            TestSyncNullable("mGrhIndex", new GrhIndex(5));
            TestSyncNullable("mInt", null);
            TestSyncNullable("mLong", (long)1032);
            TestSyncNullable("mMapEntityIndex", null);
            TestSyncNullable("mActionDisplayID", null);
            TestSyncNullable("mActionDisplayID", new ActionDisplayID(5));
        }

        [Test]
        public void BoolTest()
        {
            TestSync("mBool", true);
        }

        [Test]
        public void ByteTest()
        {
            TestSync("mByte", (byte)5);
        }

        [Test]
        public void ColorTest()
        {
            TestSync("mColor", new Color(255, 243, 234, 12));
        }

        [Test]
        public void FloatTest()
        {
            TestSync("mFloat", 57.0f);
        }

        [Test]
        public void GrhIndexTest()
        {
            TestSync("mGrhIndex", new GrhIndex(5));
        }

        [Test]
        public void IntTest()
        {
            TestSync("mInt", 5);
        }

        [Test]
        public void LongTest()
        {
            TestSync("mLong", (long)1032);
        }

        [Test]
        public void MapEntityIndexTest()
        {
            TestSyncNullable("mMapEntityIndex", null);
            TestSyncNullable("mMapEntityIndex", new MapEntityIndex(55));
            TestSync("mMapEntityIndex", new MapEntityIndex(55));
        }

        [Test]
        public void ActionDisplayIDTest()
        {
            TestSyncNullable("mActionDisplayID", null);
            TestSyncNullable("mActionDisplayID", new ActionDisplayID(55));
            TestSync("mActionDisplayID", new ActionDisplayID(55));
        }

        [Test]
        public void SByteTest()
        {
            TestSync("mSByte", (sbyte)23);
        }

        [Test]
        public void ShortTest()
        {
            TestSync("mShort", (short)23);
        }

        [Test]
        public void UIntTest()
        {
            TestSync("mUInt", (uint)23);
        }

        [Test]
        public void ULongTest()
        {
            TestSync("mULong", (ulong)23);
        }

        [Test]
        public void UShortTest()
        {
            TestSync("mUShort", (ushort)23);
        }

        [Test]
        public void Vector2Test()
        {
            TestSync("mVector2", new Vector2(23, 32));
        }

        #endregion

        /// <summary>
        /// Test class containing all the nullable values to test the syncing on.
        /// </summary>
        class NullableTestClass
        {
            readonly IPropertySync[] _propertySyncs;

            public NullableTestClass()
            {
                _propertySyncs = PropertySyncHelper.GetPropertySyncs(GetType()).ToArray();
            }

            [SyncValue]
            public bool? mBool { get; set; }

            [SyncValue]
            public byte? mByte { get; set; }

            [SyncValue]
            public Color? mColor { get; set; }

            [SyncValue]
            public double? mDouble { get; set; }

            [SyncValue]
            public float? mFloat { get; set; }

            [SyncValue]
            public GrhIndex? mGrhIndex { get; set; }

            [SyncValue]
            public int? mInt { get; set; }

            [SyncValue]
            public long? mLong { get; set; }

            [SyncValue]
            public MapEntityIndex? mMapEntityIndex { get; set; }

            [SyncValue]
            public sbyte? mSByte { get; set; }

            [SyncValue]
            public short? mShort { get; set; }

            [SyncValue]
            public uint? mUInt { get; set; }

            [SyncValue]
            public ulong? mULong { get; set; }

            [SyncValue]
            public ushort? mUShort { get; set; }

            [SyncValue]
            public Vector2? mVector2 { get; set; }

            [SyncValue]
            public ActionDisplayID? mActionDisplayID { get; set; }

            public void Deserialize(IValueReader reader)
            {
                int count = reader.ReadByte("Count");

                for (var i = 0; i < count; i++)
                {
                    int propIndex = reader.ReadByte("PropertyIndex" + i);
                    _propertySyncs[propIndex].ReadValue(this, reader);
                }
            }

            public void Serialize(IValueWriter writer)
            {
                var changed = new Queue<int>();

                for (var i = 0; i < _propertySyncs.Length; i++)
                {
                    if (_propertySyncs[i].HasValueChanged(this))
                        changed.Enqueue(i);
                }

                writer.Write("Count", (byte)changed.Count);

                var index = 0;
                foreach (var i in changed)
                {
                    writer.Write("PropertyIndex" + index++, (byte)i);
                    _propertySyncs[i].WriteValue(this, writer);
                }
            }
        }

        /// <summary>
        /// Test class containing all the values to test the syncing on.
        /// </summary>
        class TestClass
        {
            readonly IPropertySync[] _propertySyncs;

            public TestClass()
            {
                _propertySyncs = PropertySyncHelper.GetPropertySyncs(GetType()).ToArray();
            }

            [SyncValue]
            public bool mBool { get; set; }

            [SyncValue]
            public byte mByte { get; set; }

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

            [SyncValue]
            public ActionDisplayID mActionDisplayID { get; set; }

            public void Deserialize(IValueReader reader)
            {
                int count = reader.ReadByte("Count");

                for (var i = 0; i < count; i++)
                {
                    int propIndex = reader.ReadByte("PropertyIndex" + i);
                    _propertySyncs[propIndex].ReadValue(this, reader);
                }
            }

            public void Serialize(IValueWriter writer)
            {
                var changed = new Queue<int>();

                for (var i = 0; i < _propertySyncs.Length; i++)
                {
                    if (_propertySyncs[i].HasValueChanged(this))
                        changed.Enqueue(i);
                }

                writer.Write("Count", (byte)changed.Count);

                var index = 0;
                foreach (var i in changed)
                {
                    writer.Write("PropertyIndex" + index++, (byte)i);
                    _propertySyncs[i].WriteValue(this, writer);
                }
            }
        }
    }
}