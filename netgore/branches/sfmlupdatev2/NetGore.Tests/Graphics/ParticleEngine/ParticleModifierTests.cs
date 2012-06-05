using System;
using System.Linq;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace NetGore.Tests.Graphics.ParticleEngine
{
    [TestFixture]
    public class ParticleModifierTests
    {
        #region Unit tests

        [Test]
        public void ConstructorTest()
        {
            new TestModifier(true, true);
            new TestModifier(true, false);
            new TestModifier(false, true);

            Assert.Throws<ArgumentException>(() => new TestModifier(false, false));
        }

        [Test]
        public void DeepCopyTest()
        {
            var a = new TestModifier { SerializedValue = 10, NonSerializedValue = 20 };
            var b = (TestModifier)a.DeepCopy();

            Assert.AreEqual(a.SerializedValue, b.SerializedValue);
            Assert.AreNotEqual(a.NonSerializedValue, b.NonSerializedValue);
        }

        #endregion

        class TestModifier : ParticleModifier
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestModifier"/> class.
            /// </summary>
            public TestModifier() : this(true, true)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TestModifier"/> class.
            /// </summary>
            /// <param name="processOnRelease">If <see cref="Particle"/>s will be processed after being released.</param>
            /// <param name="processOnUpdate">If <see cref="Particle"/>s will be processed after being updated.</param>
            /// <exception cref="ArgumentException">Both parameters are false.</exception>
            public TestModifier(bool processOnRelease, bool processOnUpdate) : base(processOnRelease, processOnUpdate)
            {
            }

            public int NonSerializedValue { get; set; }
            public int SerializedValue { get; set; }

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
                SerializedValue = reader.ReadInt("SerializedValue");
            }

            /// <summary>
            /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
            /// </summary>
            /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
            protected override void WriteCustomValues(IValueWriter writer)
            {
                writer.Write("SerializedValue", SerializedValue);
            }
        }
    }
}