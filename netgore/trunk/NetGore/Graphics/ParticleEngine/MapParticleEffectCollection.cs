using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A collection of <see cref="ParticleEffectReference"/>s.
    /// </summary>
    public class MapParticleEffectCollection : List<ParticleEffectReference>
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
            writer.WriteManyNodes(nodeName, ToArray(), (w, v) => v.WriteState(w));
        }
    }
}