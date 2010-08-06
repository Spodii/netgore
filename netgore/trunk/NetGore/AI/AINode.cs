using System.Linq;

namespace NetGore.AI
{
    public struct AINode
    {
        // TODO: Documentation
        // TODO: Too large for a struct. Should probably a class.

        public int F;
        public int G;
        public int H;
        public ushort PX;
        public ushort PY;
        public byte Status;
        public int X;
        public int Y;
    }
}