using System.Linq;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    public sealed class EmitterModifierCollection : VirtualList<EmitterModifier>
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

        /// <summary>
        /// Applies the <see cref="EmitterModifier"/>s in this collection to the given <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to apply the effects to.</param>
        /// <param name="elapsedTime">The elapsed time since the last update.</param>
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
            var modifiers = reader.ReadManyNodes(nodeName, EmitterModifier.Read);

            // Clear the collection and add the created modifiers
            Clear();
            AddRange(modifiers);
        }

        /// <summary>
        /// Reverts the changes from the <see cref="EmitterModifier"/>s.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to revert the changes on.</param>
        public void RestoreEmitter(ParticleEmitter emitter)
        {
            foreach (var modifier in this.Reverse())
            {
                modifier.Restore(emitter);
            }
        }

        /// <summary>
        /// Writes the <see cref="EmitterModifierCollection"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="nodeName">The name to give the collection node.</param>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public void Write(string nodeName, IValueWriter writer)
        {
            writer.WriteManyNodes(nodeName, this.ToArray(), (w, mod) => mod.Write(w));
        }
    }
}