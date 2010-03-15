using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Describes the body for a character, such as the size and what components to use to perform certain types
    /// of animations.
    /// </summary>
    public class BodyInfo
    {
        const string _bodyValueKey = "Body";
        const string _fallValueKey = "Fall";
        const string _idValueKey = "ID";
        const string _jumpValueKey = "Jump";
        const string _punchValueKey = "Punch";
        const string _sizeValueKey = "Size";
        const string _standValueKey = "Stand";
        const string _walkValueKey = "Walk";

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyInfo"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public BodyInfo(IValueReader reader)
        {
            ID = reader.ReadBodyID(_idValueKey);
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
        public BodyID ID { get; private set; }
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
            writer.Write(_idValueKey, ID);
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