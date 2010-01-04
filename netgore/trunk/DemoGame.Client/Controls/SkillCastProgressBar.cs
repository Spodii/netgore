using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    public class SkillCastProgressBar : TextControl
    {
        int _castStartTime;
        int _currentCastTime;
        int _currentTime;
        SkillType? _skillType;
        Vector2 _textOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillCastProgressBar"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public SkillCastProgressBar(Control parent) : base(parent, Vector2.Zero, new Vector2(200, 40))
        {
            ForeColor = Color.White;
            Position = (new Vector2(0.5f, 0.75f) * parent.Size) - (Size / 2);
            IsVisible = false;
        }

        public SkillType? CurrentSkillType
        {
            get { return _skillType; }
        }

        /// <summary>
        /// Draws the <see cref="Control"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            // Draw the progress bar
            float elapsedTime = _currentTime - _castStartTime;
            float percent = elapsedTime / _currentCastTime;
            var startDrawPos = ScreenPosition + new Vector2(Border.LeftWidth, Border.TopHeight);
            var drawWidth = ClientSize.X * Math.Min(percent, 1);
            Rectangle r = new Rectangle((int)startDrawPos.X, (int)startDrawPos.Y, (int)drawWidth, (int)ClientSize.Y);
            XNARectangle.Draw(spriteBatch, r, new Color(255, 0, 0, 200));

            // Draw the name
            spriteBatch.DrawStringShaded(Font, Text, Position + _textOffset, ForeColor, Color.Black);

            // Stop drawing if we hit 100%
            if (percent >= 1.00f)
                StopCasting();
        }

        /// <summary>
        /// Starts the display of a skill being casted.
        /// </summary>
        /// <param name="skillType">Type of the skill.</param>
        /// <param name="castTime">The time it will take for the skill to be casted.</param>
        public void StartCasting(SkillType skillType, int castTime)
        {
            _currentCastTime = castTime;
            _castStartTime = _currentTime;
            _skillType = skillType;

            Text = "Casting " + skillType;

            var textSize = Font.MeasureString(Text);
            _textOffset = (Size / 2f) - (textSize / 2f);

            IsVisible = true;
        }

        /// <summary>
        /// Forces the current skill cast progress to terminate.
        /// </summary>
        public void StopCasting()
        {
            IsVisible = false;
            _skillType = null;
        }

        /// <summary>
        /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(int currentTime)
        {
            _currentTime = currentTime;

            base.UpdateControl(currentTime);
        }
    }
}