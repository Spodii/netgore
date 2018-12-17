using System;
using System.Linq;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Control"/> that is used to display the casting progress of a skill the user is casting for skills that have a
    /// casting/using delay.
    /// </summary>
    public class SkillCastProgressBar : TextControl
    {
        TickCount _castStartTime;
        TickCount _currentCastTime;
        SkillType? _skillType;
        Vector2 _textOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillCastProgressBar"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public SkillCastProgressBar(Control parent) : base(parent, Vector2.Zero, new Vector2(200, 20))
        {
            ForeColor = Color.White;
            Position = (new Vector2(0.5f, 0.85f) * parent.Size) - (Size / 2);
            IsVisible = false;
        }

        public SkillType? CurrentSkillType
        {
            get { return _skillType; }
        }

        /// <summary>
        /// Draws the <see cref="Control"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected override void DrawControl(ISpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            // Draw the progress bar
            float elapsedTime = TickCount.Now - _castStartTime;
            var percent = elapsedTime / _currentCastTime;
            var startDrawPos = ScreenPosition + new Vector2(Border.LeftWidth, Border.TopHeight);
            var drawWidth = ClientSize.X * Math.Min(percent, 1);
            var r = new Rectangle(startDrawPos.X, startDrawPos.Y, drawWidth, ClientSize.Y);
            RenderRectangle.Draw(spriteBatch, r, new Color(255, 0, 0, 200));

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
        public void StartCasting(SkillType skillType, TickCount castTime)
        {
            _currentCastTime = castTime;
            _castStartTime = TickCount.Now;
            _skillType = skillType;

            Text = GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.CombatCastingBegin, skillType.ToString());

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
    }
}