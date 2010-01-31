using System.Linq;
using NetGore.IO;
using NetGore.Stats;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class StatTests
    {
        [Test]
        public void ChangeStatValueTest()
        {
            var stat = CreateStat(TestStatType.C, 5);
            Assert.AreEqual(5, stat.Value);
            Assert.AreEqual(TestStatType.C, stat.StatType);

            stat.Value = 199;
            Assert.AreEqual(199, stat.Value);
        }

        static IStat<TestStatType> CreateStat(TestStatType statType, int value)
        {
            return CreateStat<StatValueShort>(statType, value);
        }

        static IStat<TestStatType> CreateStat<T>(TestStatType statType, int value) where T : IStatValueType, new()
        {
            return new Stat<TestStatType, T>(statType, value);
        }

        [Test]
        public void CreateStatTest()
        {
            var stat = CreateStat(TestStatType.C, 5);
            Assert.AreEqual(5, stat.Value);
            Assert.AreEqual(TestStatType.C, stat.StatType);

            stat = CreateStat(TestStatType.D, 1);
            Assert.AreEqual(1, stat.Value);
            Assert.AreEqual(TestStatType.D, stat.StatType);

            stat = CreateStat(TestStatType.F, 62);
            Assert.AreEqual(62, stat.Value);
            Assert.AreEqual(TestStatType.F, stat.StatType);
        }

        [Test]
        public void DeepCopyIsNewReferenceTest()
        {
            var stat = CreateStat(TestStatType.C, 5);
            var copy = stat.DeepCopy();
            Assert.AreEqual(stat.Value, copy.Value);
            Assert.AreEqual(stat.StatType, copy.StatType);

            stat.Value = 123;

            Assert.AreEqual(123, stat.Value);
            Assert.AreEqual(5, copy.Value);
        }

        [Test]
        public void DeepCopyTest()
        {
            var stat = CreateStat(TestStatType.C, 5);
            var copy = stat.DeepCopy();
            Assert.AreEqual(stat.Value, copy.Value);
            Assert.AreEqual(stat.StatType, copy.StatType);
        }

        [Test]
        public void DeepCopyValueTypeIsNewReferenceTest()
        {
            var stat = CreateStat(TestStatType.C, 5);
            var copy = stat.DeepCopyValueType();
            Assert.AreEqual(stat.Value, copy.GetValue());

            stat.Value = 123;

            Assert.AreEqual(123, stat.Value);
            Assert.AreEqual(5, copy.GetValue());
        }

        [Test]
        public void DeepCopyValueTypeTest()
        {
            var stat = CreateStat(TestStatType.C, 5);
            var copy = stat.DeepCopyValueType();
            Assert.AreEqual(stat.Value, copy.GetValue());
        }

        [Test]
        public void OnChangeStatValueTest()
        {
            bool b = false;

            var stat = CreateStat(TestStatType.C, 5);
            stat.Changed += delegate { b = true; };
            Assert.IsFalse(b);

            stat.Value = 199;

            Assert.AreEqual(199, stat.Value);
            Assert.IsTrue(b);

            b = false;
            Assert.IsFalse(b);
            stat.Value = 199;
            Assert.IsFalse(b);

            stat.Value++;
            Assert.IsTrue(b);
        }

        [Test]
        public void ReadWriteTest()
        {
            var stat = CreateStat(TestStatType.C, 5);
            var stat2 = CreateStat(TestStatType.C, 0);
            BitStream bs = new BitStream(BitStreamMode.Write, 128);

            Assert.AreEqual(5, stat.Value);
            Assert.AreEqual(0, stat2.Value);

            stat.Write(bs);

            Assert.AreEqual(5, stat.Value);
            Assert.AreEqual(0, stat2.Value);

            bs.Mode = BitStreamMode.Read;
            stat2.Read(bs);

            Assert.AreEqual(5, stat.Value);
            Assert.AreEqual(5, stat2.Value);

            bs.Mode = BitStreamMode.Write;
            stat.Value = 321;
            stat2.Value = 123;

            Assert.AreEqual(321, stat.Value);
            Assert.AreEqual(123, stat2.Value);

            stat2.Write(bs);

            Assert.AreEqual(321, stat.Value);
            Assert.AreEqual(123, stat2.Value);

            bs.Mode = BitStreamMode.Read;
            stat.Read(bs);

            Assert.AreEqual(123, stat.Value);
            Assert.AreEqual(123, stat2.Value);
        }

        enum TestStatType
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G
        }
    }
}