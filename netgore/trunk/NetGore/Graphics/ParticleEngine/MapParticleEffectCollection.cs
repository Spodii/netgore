using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A collection of <see cref="ParticleEmitter"/>s on a Map.
    /// </summary>
    public class MapParticleEffectCollection : List<ParticleEmitter>
    {
        /// <summary>
        /// Reads the <see cref="MapParticleEffectCollection"/>'s items.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="nodeName">The name of the collection node.</param>
        public void Read(IValueReader reader, string nodeName)
        {
            Clear();
            var emitters = reader.ReadManyNodes<ParticleEmitter>(nodeName, ParticleEmitterFactory.Read);
            AddRange(emitters);
        }

        /// <summary>
        /// Writes the <see cref="MapParticleEffectCollection"/>'s items.
        /// </summary>
        /// <param name="writer"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="nodeName">The name of the collection node.</param>
        public void Write(IValueWriter writer, string nodeName)
        {
            writer.WriteManyNodes(nodeName, ToArray(), ParticleEmitterFactory.Write);
        }
    }
}