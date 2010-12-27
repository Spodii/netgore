using System.Linq;
using NetGore;
using NetGore.Graphics;
using NetGore.IO;
using SFML.Graphics;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyInfo"/> class.
        /// </summary>
        public BodyInfo()
        {
        }

        /// <summary>
        /// Gets the name of the <see cref="SkeletonBody"/> (when using <see cref="SkeletonCharacterSprite"/>) or
        /// sprite category (when using <see cref="GrhCharacterSprite"/>) to use for this body.
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        /// Gets the name of the <see cref="SkeletonSet"/> (when using <see cref="SkeletonCharacterSprite"/>) or
        /// sprite category (when using <see cref="GrhCharacterSprite"/>) to use for this body when drawing
        /// the character is falling.
        /// </summary>
        public string Fall { get; private set; }

        /// <summary>
        /// Gets the <see cref="BodyID"/> of this body.
        /// </summary>
        public BodyID ID { get; private set; }

        /// <summary>
        /// Gets the name of the <see cref="SkeletonSet"/> (when using <see cref="SkeletonCharacterSprite"/>) or
        /// sprite category (when using <see cref="GrhCharacterSprite"/>) to use for this body when drawing
        /// the character is jumping.
        /// </summary>
        public string Jump { get; private set; }

        /// <summary>
        /// Gets the name of the <see cref="SkeletonSet"/> (when using <see cref="SkeletonCharacterSprite"/>) or
        /// sprite category (when using <see cref="GrhCharacterSprite"/>) to use for this body when drawing
        /// the character is punching.
        /// </summary>
        public string Punch { get; private set; }

        /// <summary>
        /// Gets the size of the body in pixels. While this should be roughly equal to the size of the body when
        /// drawn, it is not required at all.
        /// </summary>
        public Vector2 Size { get; private set; }

        /// <summary>
        /// Gets the name of the <see cref="SkeletonSet"/> (when using <see cref="SkeletonCharacterSprite"/>) or
        /// sprite category (when using <see cref="GrhCharacterSprite"/>) to use for this body when drawing
        /// the character is standing still.
        /// </summary>
        public string Stand { get; private set; }

        /// <summary>
        /// Gets the name of the <see cref="SkeletonSet"/> (when using <see cref="SkeletonCharacterSprite"/>) or
        /// sprite category (when using <see cref="GrhCharacterSprite"/>) to use for this body when drawing
        /// the character is walking.
        /// </summary>
        public string Walk { get; private set; }

        /// <summary>
        /// Reads a <see cref="BodyInfo"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <returns>The read <see cref="BodyInfo"/>.</returns>
        public static BodyInfo Read(IValueReader reader)
        {
            return new BodyInfo(reader);
        }

        /// <summary>
        /// Writes this <see cref="BodyInfo"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
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