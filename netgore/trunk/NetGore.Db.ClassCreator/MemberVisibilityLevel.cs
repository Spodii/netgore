using System.Linq;

namespace NetGore.Db.ClassCreator
{
    public enum MemberVisibilityLevel : byte
    {
        Private,
        Protected,
        Internal,
        ProtectedInternal,
        Public
    }
}