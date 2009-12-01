using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A collection of <see cref="ParticleEmitter"/>s on a Map.
    /// </summary>
    public class MapParticleEffectCollection : List<ParticleEmitter>
    {
        public void Write(IValueWriter writer, string nodeName)
        {
            writer.WriteManyNodes(nodeName, ToArray(), ParticleEmitterFactory.Write);
        }

        public void Read(IValueReader reader, string nodeName)
        {
            Clear();
            var emitters = reader.ReadManyNodes<ParticleEmitter>(nodeName, ParticleEmitterFactory.Read);
            AddRange(emitters);
        }
    }
}
