using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.IO;

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

        public AnimatedGrhData(GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            _frames = new StationaryGrhData[0];
            _speed = 1f / 300f;
        }

        AnimatedGrhData(IValueReader r, GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            var speed = r.ReadInt(_speedValueKey);
            var frames = r.ReadMany(_framesNodeName, (xreader, xname) => xreader.ReadGrhIndex(xname));

            _speed = 1f / speed;
            _frames = CreateFrames(frames);
            _size = GetMaxSize(_frames);
        }

        /// <summary>
        /// Gets the frames in the <see cref="AnimatedGrhData"/>.
        /// </summary>
        public IEnumerable<StationaryGrhData> Frames
        {
            get { return _frames; }
        }

        /// <summary>
        /// Gets the number of frames in this <see cref="AnimatedGrhData"/>.
        /// </summary>
        public int FramesCount
        {
            get { return _frames.Length; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the size of the <see cref="GrhData"/>'s sprite in pixels.
        /// </summary>
        /// <value></value>
        public override Vector2 Size
        {
            get { return _size; }
        }

        /// <summary>
        /// Gets or sets the speed multiplier of the Grh animation where each frame lasts 1f/Speed milliseconds.
        /// </summary>
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        StationaryGrhData[] CreateFrames(GrhIndex[] frameIndices)
        {
            var frames = new StationaryGrhData[frameIndices.Length];
            for (int i = 0; i < frameIndices.Length; i++)
            {
                frames[i] = GrhInfo.GetData(frameIndices[i]) as StationaryGrhData;
                if (frames[i] == null)
                {
                    const string errmsg =
                        "Failed to load GrhData `{0}`. GrhData `{1}` needs it for frame index `{2}` (0-based), out of `{3}` frames total.";
                    string err = string.Format(errmsg, frames[i], this, i, frameIndices.Length);
                    throw new Exception(err);
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

            var copy = new AnimatedGrhData(newGrhIndex, newCategorization) { _frames = copyArray, Speed = Speed };

            return copy;
        }

        /// <summary>
        /// Gets the <see cref="StationaryGrhData"/> for the given frame.
        /// </summary>
        /// <param name="frameIndex">The index of the frame.</param>
        /// <returns>The <see cref="StationaryGrhData"/> for the given frame, or null if invalid.</returns>
        public StationaryGrhData GetFrame(int frameIndex)
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