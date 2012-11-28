using System.Linq;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A collection of <see cref="ParticleEffectReference"/>s.
    /// </summary>
    public class MapParticleEffectCollection : VirtualList<ParticleEffectReference>
    {
        /// <summary>
        /// Reads the <see cref="MapParticleEffectCollection"/>'s items.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="nodeName">The name of the collection node.</param>
        public void Read(IValueReader reader, string nodeName)
        {
            Clear();
            var emitters = reader.ReadManyNodes(nodeName, r => new ParticleEffectReference(r));
            AddRange(emitters);
        }

        /// <summary>
        /// Writes the <see cref="MapParticleEffectCollection"/>'s items.
        /// </summary>
        /// <param name="writer"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="nodeName">The name of the collection node.</param>
        public void Write(IValueWriter writer, string nodeName)
        {
            // Get the particle effects in a consistent order
            var particleEffects = this
                .OrderBy(x => x.Position.Y)
                .ThenBy(x => x.Position.X)
                .ToImmutable();

            writer.WriteManyNodes(nodeName, particleEffects, (w, v) => v.WriteState(w));
        }
    }
}