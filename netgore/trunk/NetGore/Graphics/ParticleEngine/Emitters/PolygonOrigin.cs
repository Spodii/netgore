using System.Linq;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// The origin of a polygon relative to its points.
    /// </summary>
    public enum PolygonOrigin : byte
    {
        Default = 0,
        Center = 1,
        Origin = 2
    }
}