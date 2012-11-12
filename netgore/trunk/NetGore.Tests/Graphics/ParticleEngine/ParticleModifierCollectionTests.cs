using System;
using System.Linq;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace NetGore.Tests.Graphics.ParticleEngine
{
    [TestFixture]
    public class ParticleModifierCollectionTests
    {
        static void ConsistencyAsserts(ParticleModifierCollection c)
        {
            Assert.AreEqual(c.Count(x => x.ProcessOnRelease), c.ReleaseModifiers.Count(), "Sub-list has too many or too few elements.");
            Assert.AreEqual(c.Count(x => x.ProcessOnUpdate), c.UpdateModifiers.Count(), "Sub-list has too many or too few elements.");

            Assert.IsTrue(c.Where(x => x.ProcessOnRelease).ContainSameElements(c.ReleaseModifiers), "Sub-list does not match main list for given value.");
            Assert.IsTrue(c.Where(x => x.ProcessOnUpdate).ContainSameElements(c.UpdateModifiers), "Sub-list does not match main list for given value.");

            Assert.IsFalse(c.Any(x => x == null), "Shouldn't be able to add null items.");

            Assert.AreEqual(c.HasReleaseModifiers, !c.ReleaseModifiers.IsEmpty());
            Assert.AreEqual(c.HasUpdateModifiers, !c.UpdateModifiers.IsEmpty());

            var concatDistinct = c.ReleaseModifiers.Concat(c.UpdateModifiers).Distinct();
            Assert.IsTrue(c.ContainSameElements(concatDistinct), "Sub-collections don't contain same items as main collection.");
        }

        #region Unit tests

        [Test]
        public void AddNullTest()
        {
            var c = new ParticleModifierCollection
            { new TestModifierA(false, true), new TestModifierA(true, true), new TestModifierA(true, false) };
            var initial = c.ToArray();
            Assert.IsTrue(c.ContainSameElements(initial));
            ConsistencyAsserts(c);
            c.Add(null);
            Assert.IsTrue(c.ContainSameElements(initial));
            ConsistencyAsserts(c);
        }

        [Test]
        public void AddReleaseModifierTest()
        {
            var c = new ParticleModifierCollection { new TestModifierA(true, false) };

            Assert.AreEqual(1, c.ReleaseModifiers.Count());
            Assert.AreEqual(0, c.UpdateModifiers.Count());

            ConsistencyAsserts(c);
        }

        [Test]
        public void AddReleaseUpdateModifierTest()
        {
            var c = new ParticleModifierCollection { new TestModifierA(true, true) };

            Assert.AreEqual(1, c.ReleaseModifiers.Count());
            Assert.AreEqual(1, c.UpdateModifiers.Count());

            ConsistencyAsserts(c);
        }

        [Test]
        public void AddUpdateModifierTest()
        {
            var c = new ParticleModifierCollection { new TestModifierA(false, true) };

            Assert.AreEqual(0, c.ReleaseModifiers.Count());
            Assert.AreEqual(1, c.UpdateModifiers.Count());

            ConsistencyAsserts(c);
        }

        [Test]
        public void DeepCopyTest()
        {
            var c = new ParticleModifierCollection
            { new TestModifierA(false, true), new TestModifierA(true, true), new TestModifierA(true, false) };
            ConsistencyAsserts(c);
            var copy = c.DeepCopy();
            ConsistencyAsserts(copy);
            Assert.IsTrue(c.ContainSameElements(copy));
            Assert.IsTrue(c.ReleaseModifiers.ContainSameElements(copy.ReleaseModifiers));
            Assert.IsTrue(c.UpdateModifiers.ContainSameElements(copy.UpdateModifiers));
        }

        [Test]
        public void InsertNullTest()
        {
            var c = new ParticleModifierCollection
            { new TestModifierA(false, true), new TestModifierA(true, true), new TestModifierA(true, false) };
            var initial = c.ToArray();
            Assert.IsTrue(c.ContainSameElements(initial));
            ConsistencyAsserts(c);
            c.Insert(0, null);
            Assert.IsTrue(c.ContainSameElements(initial));
            ConsistencyAsserts(c);
        }

        [Test]
        public void RemoveModifierThatDoesNotExistTest()
        {
            var c = new ParticleModifierCollection
            { new TestModifierA(false, true), new TestModifierA(true, true), new TestModifierA(true, false) };

            Assert.AreEqual(2, c.ReleaseModifiers.Count());
            Assert.AreEqual(2, c.UpdateModifiers.Count());
            ConsistencyAsserts(c);

            c.Remove(new TestModifierA(false, true));

            Assert.AreEqual(2, c.ReleaseModifiers.Count());
            Assert.AreEqual(2, c.UpdateModifiers.Count());
            ConsistencyAsserts(c);
        }

        [Test]
        public void RemoveNullTest()
        {
            var c = new ParticleModifierCollection
            { new TestModifierA(false, true), new TestModifierA(true, true), new TestModifierA(true, false) };
            var initial = c.ToArray();
            Assert.IsTrue(c.ContainSameElements(initial));
            ConsistencyAsserts(c);
            c.Remove(null);
            Assert.IsTrue(c.ContainSameElements(initial));
            ConsistencyAsserts(c);
        }

        [Test]
        public void RemoveOnlyReleaseModifierTest()
        {
            var item = new TestModifierA(true, false);
            var c = new ParticleModifierCollection { item };
            c.Remove(item);

            Assert.AreEqual(0, c.ReleaseModifiers.Count());
            Assert.AreEqual(0, c.UpdateModifiers.Count());

            ConsistencyAsserts(c);
        }

        [Test]
        public void RemoveOnlyUpdateModifierTest()
        {
            var item = new TestModifierA(false, true);
            var c = new ParticleModifierCollection { item };
            c.Remove(item);

            Assert.AreEqual(0, c.ReleaseModifiers.Count());
            Assert.AreEqual(0, c.UpdateModifiers.Count());

            ConsistencyAsserts(c);
        }

        [Test]
        public void RemoveReleaseModifierFromBackTest()
        {
            var item = new TestModifierA(true, false);
            var c = new ParticleModifierCollection { new TestModifierA(true, true), new TestModifierA(true, true), item };
            c.Remove(item);

            Assert.AreEqual(2, c.ReleaseModifiers.Count());
            Assert.AreEqual(2, c.UpdateModifiers.Count());

            ConsistencyAsserts(c);
        }

        [Test]
        public void RemoveReleaseModifierFromFrontTest()
        {
            var item = new TestModifierA(true, false);
            var c = new ParticleModifierCollection { item, new TestModifierA(true, true), new TestModifierA(true, true) };
            c.Remove(item);

            Assert.AreEqual(2, c.ReleaseModifiers.Count());
            Assert.AreEqual(2, c.UpdateModifiers.Count());

            ConsistencyAsserts(c);
        }

        [Test]
        public void RemoveReleaseModifierFromMiddleTest()
        {
            var item = new TestModifierA(true, false);
            var c = new ParticleModifierCollection { new TestModifierA(true, true), item, new TestModifierA(true, true) };
            c.Remove(item);

            Assert.AreEqual(2, c.ReleaseModifiers.Count());
            Assert.AreEqual(2, c.UpdateModifiers.Count());

            ConsistencyAsserts(c);
        }

        [Test]
        public void RemoveUpdateModifierFromBackTest()
        {
            var item = new TestModifierA(false, true);
            var c = new ParticleModifierCollection { new TestModifierA(true, true), new TestModifierA(true, true), item };
            c.Remove(item);

            Assert.AreEqual(2, c.ReleaseModifiers.Count());
            Assert.AreEqual(2, c.UpdateModifiers.Count());

            ConsistencyAsserts(c);
        }

        [Test]
        public void RemoveUpdateModifierFromFrontTest()
        {
            var item = new TestModifierA(false, true);
            var c = new ParticleModifierCollection { item, new TestModifierA(true, true), new TestModifierA(true, true) };
            c.Remove(item);

            Assert.AreEqual(2, c.ReleaseModifiers.Count());
            Assert.AreEqual(2, c.UpdateModifiers.Count());

            ConsistencyAsserts(c);
        }

        [Test]
        public void RemoveUpdateModifierFromMiddleTest()
        {
            var item = new TestModifierA(false, true);
            var c = new ParticleModifierCollection { new TestModifierA(true, true), item, new TestModifierA(true, true) };
            c.Remove(item);

            Assert.AreEqual(2, c.ReleaseModifiers.Count());
            Assert.AreEqual(2, c.UpdateModifiers.Count());

            ConsistencyAsserts(c);
        }

        [Test]
        public void SetNullTest()
        {
            var c = new ParticleModifierCollection
            { new TestModifierA(false, true), new TestModifierA(true, true), new TestModifierA(true, false) };
            var initial = c.ToArray();
            Assert.IsTrue(c.ContainSameElements(initial));
            ConsistencyAsserts(c);
            c[0] = null;
            Assert.IsTrue(c.ContainSameElements(initial));
            ConsistencyAsserts(c);
        }

        [Test]
        public void ToArrayTest()
        {
            var c = new ParticleModifierCollection
            { new TestModifierA(false, true), new TestModifierA(true, true), new TestModifierA(true, false) };

            var a = c.ToArray();
            Assert.IsTrue(c.ContainSameElements(a));
            Assert.AreEqual(c.Count, a.Length);
        }

        [Test]
        public void ToListTest()
        {
            var c = new ParticleModifierCollection
            { new TestModifierA(false, true), new TestModifierA(true, true), new TestModifierA(true, false) };

            var a = c.ToList();
            Assert.IsTrue(c.ContainSameElements(a));
            Assert.AreEqual(c.Count, a.Count);
        }

        #endregion

        class TestModifierA : ParticleModifier
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestModifierA"/> class.
            /// </summary>
            public TestModifierA() : this(true, true)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TestModifierA"/> class.
            /// </summary>
            /// <param name="processOnRelease">If <see cref="Particle"/>s will be processed after being released.</param>
            /// <param name="processOnUpdate">If <see cref="Particle"/>s will be processed after being updated.</param>
            /// <exception cref="ArgumentException">Both parameters are false.</exception>
            public TestModifierA(bool processOnRelease, bool processOnUpdate) : base(processOnRelease, processOnUpdate)
            {
            }

            /// <summary>
            /// When overridden in the derived class, handles processing the <paramref name="particle"/> when
            /// it is released. Only valid if <see cref="ParticleModifier.ProcessOnRelease"/> is set.
            /// </summary>
            /// <param name="emitter">The <see cref="ParticleEmitter"/> that the <paramref name="particle"/>
            /// came from.</param>
            /// <param name="particle">The <see cref="Particle"/> to process.</param>
            protected override void HandleProcessReleased(ParticleEmitter emitter, Particle particle)
            {
            }

            /// <summary>
            /// When overridden in the derived class, handles processing the <paramref name="particle"/> when
            /// it is updated. Only valid if <see cref="ParticleModifier.ProcessOnUpdate"/> is set.
            /// </summary>
            /// <param name="emitter">The <see cref="ParticleEmitter"/> that the <paramref name="particle"/>
            /// came from.</param>
            /// <param name="particle">The <see cref="Particle"/> to process.</param>
            /// <param name="elapsedTime">The amount of time that has elapsed since the <paramref name="emitter"/>
            /// was last updated.</param>
            protected override void HandleProcessUpdated(ParticleEmitter emitter, Particle particle, int elapsedTime)
            {
            }

            /// <summary>
            /// Reads the <see cref="ParticleModifier"/>'s custom values from the <see cref="reader"/>.
            /// </summary>
            /// <param name="reader"><see cref="IValueReader"/> to read the custom values from.</param>
            protected override void ReadCustomValues(IValueReader reader)
            {
            }

            /// <summary>
            /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
            /// </summary>
            /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
            protected override void WriteCustomValues(IValueWriter writer)
            {
            }
        }
    }
}