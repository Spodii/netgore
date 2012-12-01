using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Uses an interpolation to animate between multiple skeletons.
    /// </summary>
    public class SkeletonAnimation
    {
        readonly List<SkeletonBody> _bodyLayers = new List<SkeletonBody>();

        /// <summary>
        /// Skeleton used for animating. Unlike _frames, the nodes of this skeleton
        /// are altered to interpolate between frames.
        /// </summary>
        readonly Skeleton _skel;

        /// <summary>
        /// Current keyframe.
        /// </summary>
        SkeletonFrame _currFrame;

        /// <summary>
        /// Current frame (unrounded).
        /// </summary>
        float _frame = 0.0f;

        /// <summary>
        /// Time the skeleton was last updated.
        /// </summary>
        TickCount _lastTime = 0;

        /// <summary>
        /// Skeleton animation modifier.
        /// </summary>
        SkeletonAnimation _mod = null;

        /// <summary>
        /// Next keyframe.
        /// </summary>
        SkeletonFrame _nextFrame;

        /// <summary>
        /// Contains the parent SkeletonAnimation if this SkeletonAnimation is a modifier. If null, this
        /// SkeletonAnimation is not a modifier.
        /// </summary>
        SkeletonAnimation _parent = null;

        /// <summary>
        /// Scaling modifier.
        /// </summary>
        float _scale = 1.0f;

        /// <summary>
        /// Skeleton body information.
        /// </summary>
        SkeletonBody _skelBody = null;

        /// <summary>
        /// Contains the frames used to animate the skeleton.
        /// </summary>
        SkeletonSet _skelSet;

        /// <summary>
        /// Speed modifier for animating.
        /// </summary>
        float _speed = 1.0f;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonAnimation"/> class.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="skeletonSet"><see cref="SkeletonSet"/> to use for the keyframes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="skeletonSet" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">skeletonSet contains no KeyFrames.</exception>
        public SkeletonAnimation(TickCount currentTime, SkeletonSet skeletonSet)
        {
            if (skeletonSet == null)
                throw new ArgumentNullException("skeletonSet");
            if (skeletonSet.KeyFrames.Length == 0)
                throw new ArgumentException("skeletonSet contains no KeyFrames.", "skeletonSet");

            _lastTime = currentTime;
            _skelSet = skeletonSet;
            _currFrame = _skelSet.KeyFrames[0];
            _nextFrame = _skelSet.KeyFrames.Length > 1 ? _skelSet.KeyFrames[1] : _skelSet.KeyFrames[0];
            _skel = CurrentFrame.Skeleton.DeepCopy();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonAnimation"/> class.
        /// </summary>
        /// <param name="currentTime">Current time.</param>
        /// <param name="frame">Single frame to use for the keyframe.</param>
        public SkeletonAnimation(TickCount currentTime, SkeletonFrame frame) : this(currentTime, new SkeletonSet(new[] { frame }))
        {
        }

        /// <summary>
        /// Notifies listeners when the skeleton animation has rolled back over to the first frame.
        /// </summary>
        public event EventHandler Looped;

        /// <summary>
        /// Gets the collection of additional layers that can be stacked on top of the original.
        /// <see cref="SkeletonBody"/>.
        /// </summary>
        public ICollection<SkeletonBody> BodyLayers
        {
            get { return _bodyLayers; }
        }

        /// <summary>
        /// Gets the skeleton for the current frame.
        /// </summary>
        public SkeletonFrame CurrentFrame
        {
            get { return _currFrame; }
        }

        /// <summary>
        /// Gets the frame number of the animation.
        /// </summary>
        public float Frame
        {
            get { return _frame; }
        }

        /// <summary>
        /// Gets the SkeletonAnimation used to modify this SkeletonAnimation.
        /// </summary>
        public SkeletonAnimation Modifier
        {
            get { return _mod; }
        }

        /// <summary>
        /// Gets the skeleton for the next frame.
        /// </summary>
        public SkeletonFrame NextFrame
        {
            get { return _nextFrame; }
        }

        /// <summary>
        /// Gets or sets the scale of the resulting skeleton.
        /// </summary>
        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// Gets the skeleton used by the animation to interpolate between frames and draw.
        /// </summary>
        public Skeleton Skeleton
        {
            get { return _skel; }
        }

        /// <summary>
        /// Gets the skeleton body used by the animator.
        /// </summary>
        public SkeletonBody SkeletonBody
        {
            get { return _skelBody; }
            set { _skelBody = value; }
        }

        /// <summary>
        /// Gets the skeleton set currently in use by the animator.
        /// </summary>
        public SkeletonSet SkeletonSet
        {
            get { return _skelSet; }
        }

        /// <summary>
        /// Gets or sets the speed multiplier of the animation in percent (1.0 for normal speed).
        /// </summary>
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        /// <summary>
        /// Adds a SkeletonAnimation modifier to this SkeletonAnimation.
        /// </summary>
        /// <param name="modifier">SkeletonAnimation to use as a modifier.</param>
        /// <param name="loop">If true, the modifier will loop forever until Detach() is called on the
        /// modifier or RemoveModifiers() is called on the parent.</param>
        public void AddModifier(SkeletonAnimation modifier, bool loop = false)
        {
            if (modifier == null)
            {
                Debug.Fail("modifier is null.");
                return;
            }

            // Remove any current modifier
            if (_mod != null)
                _mod.Detach();

            // Attach the modifier to this SkeletonAnimation
            modifier._parent = this;
            _mod = modifier;

            // If not looping, detach once the animation finishes
            if (!loop)
            {
                modifier.Looped -= modifier_Looped;
                modifier.Looped += modifier_Looped;
            }
        }

        /// <summary>
        /// Changes the SkeletonSet used to animate.
        /// </summary>
        /// <param name="newSet">New SkeletonSet to use.</param>
        public void ChangeSet(SkeletonSet newSet)
        {
            if (newSet == null)
            {
                Debug.Fail("newSet is null.");
                return;
            }
            if (newSet.KeyFrames.Length == 0)
            {
                Debug.Fail("newSet contains no KeyFrames.");
                return;
            }

            // Set the new animation and clear the frame count
            _frame = 0;
            _nextFrame = newSet.KeyFrames[0];
            _skelSet = newSet;

            // Create a temporary new current keyframe by duplicating the current state and making
            // that our new current keyframe, resulting in a smooth translation to the next animation
            float delay;
            if (CurrentFrame.Delay == 0)
                delay = _nextFrame.Delay;
            else
                delay = CurrentFrame.Delay;

            _currFrame = new SkeletonFrame("_worker_", _skel.DeepCopy(), delay);
        }

        /// <exception cref="ArgumentNullException"><paramref name="set" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="skel" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="set"/> contians no KeyFrames.</exception>
        public static SkeletonSet CreateSmoothedSet(SkeletonSet set, Skeleton skel)
        {
            if (set == null)
                throw new ArgumentNullException("set");
            if (skel == null)
                throw new ArgumentNullException("skel");
            if (set.KeyFrames.Length == 0)
                throw new ArgumentException("Parameter `set` contians no KeyFrames.", "set");

            // Create the new frames
            var frames = new SkeletonFrame[set.KeyFrames.Length + 2];

            // Move the old frames into the new frames array
            for (var i = 0; i < set.KeyFrames.Length; i++)
            {
                frames[i + 1] = set.KeyFrames[i];
            }

            // Set the first and last frame to the skeleton
            var lastFrame = frames.Length - 1;
            frames[0] = new SkeletonFrame(string.Empty, skel, frames[1].Delay);
            frames[lastFrame] = new SkeletonFrame(string.Empty, skel, frames[lastFrame - 1].Delay);

            // Copy over the IsModifier properties from the last frame from the old set
            // This is required to properly animate the new skeleton set
            // The last frame is used instead of the first since it is more important that
            // we transist out of the animation smoother than translating in, under the rare
            // and undesirable case that all IsModifier properties are not equal
            frames[0].Skeleton.CopyIsModifier(frames[lastFrame - 1].Skeleton);

            return new SkeletonSet(frames);
        }

        /// <summary>
        /// If the <see cref="SkeletonAnimation"/> is used as a modifier, this will detach it from its parent.
        /// </summary>
        public void Detach()
        {
            if (_parent != null)
            {
                _parent._mod = null;
                _parent = null;
            }
        }

        /// <summary>
        /// Draws the skeleton animation.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        public void Draw(ISpriteBatch sb)
        {
            Draw(sb, Vector2.Zero);
        }

        /// <summary>
        /// Draws the skeleton animation.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="position">Position offset to draw at.</param>
        /// <param name="color">The color.</param>
        /// <param name="effect">SpriteEffect to use when drawing.</param>
        public void Draw(ISpriteBatch sb, Vector2 position, Color color, SpriteEffects effect = SpriteEffects.None)
        {
            if (sb == null)
            {
                Debug.Fail("sb is null.");
                return;
            }
            if (sb.IsDisposed)
            {
                Debug.Fail("sb is disposed.");
                return;
            }

            if (_skelBody == null)
                return;

            // Draw the body and all layers
            // FUTURE: This is horribly inefficient. Can improve later by, whenever a body layer is added or removed
            // use a dictionary where the key is the _skelBody.BodyItems, and the value is an IEnumerable of all body layer
            // items to draw for that layer (giving us O(1) look-up). Then update that dictionary every time the collection
            // is changed. Will have to change the way the BodyLayers is exposed so we can keep track of adds/removes.
            foreach (var item in _skelBody.BodyItems)
            {
                item.Draw(sb, position, _scale, color, effect);
                foreach (var bodyLayer in BodyLayers)
                {
                    foreach (var item2 in bodyLayer.BodyItems)
                    {
                        if (item2.Source == item.Source && item.Dest == item2.Dest)
                            item2.Draw(sb, position, _scale, color, effect);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the skeleton animation.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="position">Position offset to draw at.</param>
        /// <param name="effect">SpriteEffect to use when drawing.</param>
        public void Draw(ISpriteBatch sb, Vector2 position, SpriteEffects effect = SpriteEffects.None)
        {
            Draw(sb, position, Color.White, effect);
        }

        /// <summary>
        /// Recursively updates all the children of a node.
        /// </summary>
        /// <param name="srcA">Source skeleton node for the current frame.</param>
        /// <param name="srcB">Source skeleton node for the next frame.</param>
        /// <param name="srcP">Parent skeleton node (use null if theres no parent).</param>
        /// <param name="dest">Destination skeleton node to have the two sources applied to.</param>
        /// <param name="framePercent">A value between 0.0 and 1.0 stating how far along the animation is
        /// from the current frame.</param>
        void RecursiveUpdate(SkeletonNode srcA, SkeletonNode srcB, SkeletonNode srcP, SkeletonNode dest, float framePercent)
        {
            // Set the position
            Vector2 vA;
            Vector2 vB;
            if (_scale == 1.0f)
            {
                vA = srcA.Position;
                vB = srcB.Position;
            }
            else
            {
                vA = srcA.Position * _scale;
                vB = srcB.Position * _scale;
            }
            dest.Position = Vector2.Lerp(vA, vB, framePercent);

            // Check if the node is part of a modifier animation
            if (srcP == null)
            {
                // Set the length
                dest.SetLength(srcA.GetLength() * _scale);
            }
            else
            {
                // This is a modifier so check for inheriting node values
                dest.SetLength(srcP.GetLength());
                if (!srcP.IsModifier && srcP.Parent != null)
                    dest.SetAngle(srcP.GetAngle());
            }

            // Update the child nodes (if there is any)
            for (var i = 0; i < srcA.internalNodes.Count; i++)
            {
                var nextSrcP = (srcP == null ? null : srcP.internalNodes[i]);
                RecursiveUpdate(srcA.internalNodes[i], srcB.internalNodes[i], nextSrcP, dest.internalNodes[i], framePercent);
            }
        }

        /// <summary>
        /// Updates the parent <see cref="SkeletonAnimation"/> that this modifier modifies.
        /// </summary>
        /// <param name="src">Source root <see cref="SkeletonNode"/>.</param>
        /// <param name="dest">Destination root <see cref="SkeletonNode"/>.</param>
        static void RecursiveUpdateParent(SkeletonNode src, SkeletonNode dest)
        {
            // Update modified values
            if (src.IsModifier && dest.Parent != null)
                dest.SetAngle(src.GetAngle());

            // Update the child nodes (if there is any)
            for (var i = 0; i < src.internalNodes.Count; i++)
            {
                RecursiveUpdateParent(src.internalNodes[i], dest.internalNodes[i]);
            }
        }

        /// <summary>
        /// Sets the speed of the animation so that it finishes within a specified time limit.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        void SetTargetTime(TickCount currentTime)
        {
            var totalTime = 0.0f;

            //Find the total time taken to run through all of the frames
            foreach (var frame in _skelSet.KeyFrames)
            {
                totalTime += frame.Delay;
            }

            //Set the speed of the animation
            _speed = totalTime / currentTime;
        }

        /// <summary>
        /// Updates the skeleton animation.
        /// </summary>
        /// <param name="currentTime">Current time.</param>
        public void Update(TickCount currentTime)
        {
            // If theres no frames and no modifier, don't update
            if (_mod == null && (_skelSet.KeyFrames.Length == 1) && (CurrentFrame.Skeleton == _skelSet.KeyFrames[0].Skeleton))
                _lastTime = currentTime;
            else
            {
                // Find the time that has elapsed since the last update
                float elapsedTime = currentTime - _lastTime;
                _lastTime = currentTime;

                // Calculate the new frame
                float delay;
                if (CurrentFrame.Delay == 0f)
                    delay = NextFrame.Delay;
                else
                    delay = CurrentFrame.Delay;

                var newFrame = _frame + ((elapsedTime * _speed) / delay);
                if (_skelSet.KeyFrames.Length != 1)
                    newFrame %= _skelSet.KeyFrames.Length;

                // Set the new keyframe references if the frame changed
                if ((int)newFrame != (int)_frame)
                {
                    if (_skelSet.KeyFrames.Length == 1)
                    {
                        _nextFrame = _skelSet.KeyFrames[0];
                        _currFrame = _nextFrame;
                    }
                    else
                    {
                        // If we have reached the first frame, raise the OnLoop event
                        if ((int)newFrame == 0)
                        {
                            if (Looped != null)
                                Looped.Raise(this, EventArgs.Empty);
                        }

                        // Store the new frame references
                        _currFrame = _skelSet.KeyFrames[(int)newFrame];
                        _nextFrame = _skelSet.KeyFrames[((int)newFrame + 1) % _skelSet.KeyFrames.Length];
                    }
                }

                // Set the new frame
                _frame = newFrame;

                // Update the nodes of the working skeleton
                var framePercent = _frame - (int)_frame;
                SkeletonNode parentNode;
                if (_parent == null)
                    parentNode = null;
                else
                    parentNode = _parent.Skeleton.RootNode;
                RecursiveUpdate(CurrentFrame.Skeleton.RootNode, NextFrame.Skeleton.RootNode, parentNode, _skel.RootNode,
                    framePercent);
            }

            // Update the body
            if (_skelBody != null)
                _skelBody.Update(currentTime);

            foreach (var bodyLayer in BodyLayers)
            {
                bodyLayer.Update(currentTime);
            }

            // If there is a parent, apply the changes to it
            if (_parent != null)
                RecursiveUpdateParent(_skel.RootNode, _parent.Skeleton.RootNode);

            // Apply the modifiers
            if (_mod != null)
                _mod.Update(currentTime);
        }

        /// <summary>
        /// Removes a SkeletonAnimation after a single loop by hooking to the OnLoop event.
        /// </summary>
        static void modifier_Looped(object sender, EventArgs e)
        {
            var src = sender as SkeletonAnimation;
            if (src == null)
            {
                Debug.Fail("src == null - unable to convert sender to SkeletonAnimation.");
                return;
            }

            // Remove all the events from the modifier SkeletonAnimation
            src.Looped = null;

            // If it has a parent (which it should), remove the references
            src.Detach();
        }
    }
}