using System.Linq;

namespace NetGore.AI
{
    public enum Heuristics : byte
    {
        DiagonalShortCut,
        DXDY,
        Euclidean,
        Manhattan
    }
}