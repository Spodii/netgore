using System.Linq;

namespace NetGore.AI
{
    public enum Heuristics : byte
    {
        // TODO: Documentation

        DiagonalShortCut,
        DXDY,
        Euclidean,
        Manhattan
    }
}