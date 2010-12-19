using System.Linq;

namespace DoxyPacker
{
    enum FileType : byte
    {
        Unknown,
        Class,
        File,
        Directory,
        ClassTemplate,
        SourceFile,
        Interface,
        Package,
        Struct,
        InterfaceTemplate,
        StructTemplate
    }
}