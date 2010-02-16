using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Information for a character's body
    /// </summary>
    public class BodyInfo
    {
        const string _bodyValueKey = "Body";
        const string _fallValueKey = "Fall";
        const string _indexValueKey = "Index";
        const string _jumpValueKey = "Jump";
        const string _punchValueKey = "Punch";
        const string _sizeValueKey = "Size";
        const string _standValueKey = "Stand";
        const string _walkValueKey = "Walk";

        public BodyInfo(IValueReader reader)
        {
            Index = reader.ReadBodyIndex(_indexValueKey);
            Body = reader.ReadString(_bodyValueKey);
            Fall = reader.ReadString(_fallValueKey);
            Jump = reader.ReadString(_jumpValueKey);
            Punch = reader.ReadString(_punchValueKey);
            Stand = reader.ReadString(_standValueKey);
            Walk = reader.ReadString(_walkValueKey);
            Size = reader.ReadVector2(_sizeValueKey);
        }

        public string Body { get; private set; }
        public string Fall { get; private set; }
        public BodyIndex Index { get; private set; }
        public string Jump { get; private set; }
        public string Punch { get; private set; }
        public Vector2 Size { get; private set; }
        public string Stand { get; private set; }
        public string Walk { get; private set; }

        public static BodyInfo Read(IValueReader reader)
        {
            return new BodyInfo(reader);
        }

        public void Write(IValueWriter writer)
        {
            writer.Write(_indexValueKey, Index);
            writer.Write(_bodyValueKey, Body);
            writer.Write(_fallValueKey, Fall);
            writer.Write(_jumpValueKey, Jump);
            writer.Write(_punchValueKey, Punch);
            writer.Write(_standValueKey, Stand);
            writer.Write(_walkValueKey, Walk);
            writer.Write(_sizeValueKey, Size);
        }
    }
}