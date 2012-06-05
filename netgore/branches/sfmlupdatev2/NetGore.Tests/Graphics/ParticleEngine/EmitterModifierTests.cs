using System.Linq;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.Graphics.ParticleEngine
{
    [TestFixture]
    public class EmitterModifierTests
    {
        #region Unit tests

        [Test]
        public void DeepCopyTest()
        {
            var a = new TestModifier { SerializedValue = 10, NonSerializedValue = 20 };
            var b = (TestModifier)a.DeepCopy();

            Assert.AreEqual(a.SerializedValue, b.SerializedValue);
            Assert.AreNotEqual(a.NonSerializedValue, b.NonSerializedValue);
        }

        #endregion

        class TestModifier : EmitterModifier
        {
            public int NonSerializedValue { get; set; }
            public int SerializedValue { get; set; }

            /// <summary>
            /// When overridden in the derived class, handles reverting changes made to the <see cref="ParticleEmitter"/>
            /// by this <see cref="EmitterModifier"/>.
            /// </summary>
            /// <param name="emitter">The <see cref="ParticleEmitter"/> to revert the changes to.</param>
            protected override void HandleRestore(ParticleEmitter emitter)
            {
            }

            /// <summary>
            /// When overridden in the derived class, handles updating the <see cref="ParticleEmitter"/>.
            /// </summary>
            /// <param name="emitter">The <see cref="ParticleEmitter"/> to be modified.</param>
            /// <param name="elapsedTime">The amount of time that has elapsed since the last update.</param>
            protected override void HandleUpdate(ParticleEmitter emitter, int elapsedTime)
            {
            }

            /// <summary>
            /// When overridden in the derived class, reads the <see cref="EmitterModifier"/>'s custom values
            /// from the <see cref="reader"/>.
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