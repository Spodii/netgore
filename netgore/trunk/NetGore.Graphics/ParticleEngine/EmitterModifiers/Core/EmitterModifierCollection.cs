using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    public sealed class EmitterModifierCollection : List<EmitterModifier>
    {
        /// <summary>
        /// Creates a deep copy of the <see cref="EmitterModifierCollection"/>.
        /// </summary>
        /// <returns>A deep copy of the <see cref="EmitterModifierCollection"/>.</returns>
        public EmitterModifierCollection DeepCopy()
        {
            var ret = new EmitterModifierCollection();
            ret.AddRange(this);
            return ret;
        }

        public void ProcessEmitter(ParticleEmitter emitter, int elapsedTime)
        {
            foreach (var modifier in this)
            {
                modifier.Update(emitter, elapsedTime);
            }
        }

        /// <summary>
        /// Reads the <see cref="EmitterModifierCollection"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="nodeName">The name of the collection node.</param>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        public void Read(string nodeName, IValueReader reader)
        {
            // Read the modifiers
            var modifiers = reader.ReadManyNodes<EmitterModifier>(nodeName, EmitterModifier.Read);

            // Clear the collection and add the created modifiers
            Clear();
            AddRange(modifiers);
        }

        /// <summary>
        /// Writes the <see cref="EmitterModifierCollection"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="nodeName">The name to give the collection node.</param>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public void Write(string nodeName, IValueWriter writer)
        {
            writer.WriteManyNodes(nodeName, ToArray(), (w, mod) => mod.Write(w));
        }
    }
}