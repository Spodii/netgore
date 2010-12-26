using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A <see cref="GrhData"/> that contains multiple frames to create an animation.
    /// </summary>
    public sealed class AnimatedGrhData : GrhData
    {
        const string _framesNodeName = "Frames";
        const string _speedValueKey = "Speed";

        StationaryGrhData[] _frames;
        Vector2 _size;
        float _speed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedGrhData"/> class.
        /// </summary>
        /// <param name="grhIndex">The <see cref="GrhIndex"/>.</param>
        /// <param name="cat">The <see cref="SpriteCategorization"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cat"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="grhIndex"/> is equal to GrhIndex.Invalid.</exception>
        public AnimatedGrhData(GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            _frames = new StationaryGrhData[0];
            _speed = 1f / 300f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedGrhData"/> class.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/>.</param>
        /// <param name="grhIndex">The <see cref="GrhIndex"/>.</param>
        /// <param name="cat">The <see cref="SpriteCategorization"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cat"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="grhIndex"/> is equal to GrhIndex.Invalid.</exception>
        AnimatedGrhData(IValueReader r, GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            var speed = r.ReadInt(_speedValueKey);
            var frames = r.ReadMany(_framesNodeName, (xreader, xname) => xreader.ReadGrhIndex(xname));

            _speed = 1f / speed;
            _frames = CreateFrames(frames);
            _size = GetMaxSize(_frames);
        }

        /// <summary>
        /// When overridden in the derived class, gets the frames in an animated <see cref="GrhData"/>, or an
        /// IEnumerable containing a reference to its self if stationary.
        /// </summary>
        public override IEnumerable<StationaryGrhData> Frames
        {
            get { return _frames; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the number of frames in this <see cref="GrhData"/>. If this
        /// is not an animated <see cref="GrhData"/>, this value will always return 0.
        /// </summary>
        public override int FramesCount
        {
            get { return _frames.Length; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the size of the <see cref="GrhData"/>'s sprite in pixels.
        /// </summary>
        public override Vector2 Size
        {
            get { return _size; }
        }

        /// <summary>
        /// Gets or sets the speed multiplier of the Grh animation where each frame lasts 1f/Speed milliseconds.
        /// </summary>
        public override float Speed
        {
            get { return _speed; }
        }

        /// <summary>
        /// Creates the array of frames for an <see cref="AnimatedGrhData"/>.
        /// </summary>
        /// <param name="frameIndices">The indices of the frames.</param>
        /// <returns>The array of <see cref="StationaryGrhData"/> frames.</returns>
        /// <exception cref="GrhDataException">A frame in this <see cref="AnimatedGrhData"/> failed to be loaded.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GrhData")]
        StationaryGrhData[] CreateFrames(IList<GrhIndex> frameIndices)
        {
            var frames = new StationaryGrhData[frameIndices.Count];
            for (var i = 0; i < frameIndices.Count; i++)
            {
                frames[i] = GrhInfo.GetData(frameIndices[i]) as StationaryGrhData;
                if (frames[i] == null)
                {
                    const string errmsg =
                        "Failed to load GrhData `{0}`. GrhData `{1}` needs it for frame index `{2}` (0-based), out of `{3}` frames total.";
                    var err = string.Format(errmsg, frames[i], this, i, frameIndices.Count);
                    throw new GrhDataException(this, err);
                }
            }

            return frames;
        }

        /// <summary>
        /// When overridden in the derived class, creates a new <see cref="GrhData"/> equal to this <see cref="GrhData"/>
        /// except for the specified parameters.
        /// </summary>
        /// <param name="newCategorization">The <see cref="SpriteCategorization"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <param name="newGrhIndex">The <see cref="GrhIndex"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <returns>
        /// A deep copy of this <see cref="GrhData"/>.
        /// </returns>
        protected override GrhData DeepCopy(SpriteCategorization newCategorization, GrhIndex newGrhIndex)
        {
            var copyArray = new StationaryGrhData[_frames.Length];
            Array.Copy(_frames, copyArray, _frames.Length);

            var copy = new AnimatedGrhData(newGrhIndex, newCategorization) { _frames = copyArray };
            copy.SetSpeed(Speed);

            return copy;
        }

        /// <summary>
        /// When overridden in the derived class, gets the frame in an animated <see cref="GrhData"/> with the
        /// corresponding index, or null if the index is out of range. If stationary, this will always return
        /// a reference to its self, no matter what the index is.
        /// </summary>
        /// <param name="frameIndex">The index of the frame to get.</param>
        /// <returns>
        /// The frame with the given <paramref name="frameIndex"/>, or null if the <paramref name="frameIndex"/>
        /// is invalid, or a reference to its self if this is not an animated <see cref="GrhData"/>.
        /// </returns>
        public override StationaryGrhData GetFrame(int frameIndex)
        {
            if (frameIndex < 0 || frameIndex >= _frames.Length)
                return null;

            return _frames[frameIndex];
        }

        /// <summary>
        /// Reads a <see cref="GrhData"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> to read from.</param>
        /// <returns>
        /// The <see cref="GrhData"/> read from the <see cref="IValueReader"/>.
        /// </returns>
        public static AnimatedGrhData Read(IValueReader r)
        {
            GrhIndex grhIndex;
            SpriteCategorization categorization;
            ReadHeader(r, out grhIndex, out categorization);

            return new AnimatedGrhData(r, grhIndex, categorization);
        }

        public void SetFrames(IEnumerable<GrhIndex> frameIndices)
        {
            SetFrames(CreateFrames(frameIndices.ToArray()));
        }

        public void SetFrames(IEnumerable<StationaryGrhData> frames)
        {
            _frames = frames.ToArray();
            _size = GetMaxSize(_frames);
        }

        /// <summary>
        /// Sets the speed of the <see cref="AnimatedGrhData"/>.
        /// </summary>
        /// <param name="newSpeed">The new speed.</param>
        public void SetSpeed(float newSpeed)
        {
            // Ensure we are using the right units
            if (newSpeed > 1.0f)
                newSpeed = 1f / newSpeed;

            _speed = newSpeed;
        }

        /// <summary>
        /// When overridden in the derived class, writes the values unique to this derived type to the
        /// <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
            var frameIndices = _frames.Select(x => x.GrhIndex).ToArray();

            writer.Write(_speedValueKey, (int)(1f / Speed));
            writer.WriteMany(_framesNodeName, frameIndices, writer.Write);
        }
    }
}