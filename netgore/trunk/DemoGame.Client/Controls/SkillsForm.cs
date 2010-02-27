using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Features.Skills;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    public delegate void UseSkillHandler(SkillType skillType);

    public class SkillsForm : Form
    {
        static readonly Vector2 _iconSize = new Vector2(32, 32);
        readonly ISkillCooldownManager _cooldownManager;
        readonly int _lineSpacing;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillsForm"/> class.
        /// </summary>
        /// <param name="cooldownManager">The skill cooldown manager.</param>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public SkillsForm(ISkillCooldownManager cooldownManager, Vector2 position, Control parent)
            : base(parent, position, new Vector2(150, 100))
        {
            _cooldownManager = cooldownManager;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            var fontLineSpacing = Font.LineSpacing;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            // Find the spacing to use between lines
            _lineSpacing = (int)Math.Max(fontLineSpacing, _iconSize.Y);

            // Create all the skills
            Vector2 offset = Vector2.Zero;
            foreach (var skillType in EnumHelper<SkillType>.Values)
            {
                CreateSkillEntry(offset, skillType);
                offset += new Vector2(0, _lineSpacing);
            }
        }

        /// <summary>
        /// Notifies listeners when a a request has been made to use a skill.
        /// </summary>
        public event UseSkillHandler RequestUseSkill;

        public ISkillCooldownManager CooldownManager
        {
            get { return _cooldownManager; }
        }

        void CreateSkillEntry(Vector2 position, SkillType skillType)
        {
            var skillInfo = SkillInfoManager.Instance.GetAttribute(skillType);

            PictureBox pb = new SkillPictureBox(this, skillInfo, position);
            pb.Clicked += SkillPicture_Clicked;

            SkillLabel skillLabel = new SkillLabel(this, skillInfo, position + new Vector2(_iconSize.X + 4, 0));
            skillLabel.Clicked += SkillLabel_Clicked;
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Skills";
        }

        void SkillLabel_Clicked(object sender, MouseClickEventArgs e)
        {
            if (RequestUseSkill != null)
            {
                SkillLabel source = (SkillLabel)sender;
                RequestUseSkill(source.SkillInfo.Value);
            }
        }

        void SkillPicture_Clicked(object sender, MouseClickEventArgs e)
        {
            if (RequestUseSkill != null)
            {
                SkillPictureBox source = (SkillPictureBox)sender;
                RequestUseSkill(source.SkillInfo.Value);
            }
        }

        sealed class SkillLabel : Label
        {
            public SkillLabel(Control parent, SkillInfoAttribute skillInfo, Vector2 position) : base(parent, position)
            {
                SkillInfo = skillInfo;
                Text = SkillInfo.DisplayName;
            }

            public SkillInfoAttribute SkillInfo { get; private set; }
        }

        sealed class SkillPictureBox : PictureBox
        {
            readonly ISkillCooldownManager _cooldownManager;
            bool _isCoolingDown = false;

            public SkillPictureBox(SkillsForm parent, SkillInfoAttribute skillInfo, Vector2 position)
                : base(parent, position, _iconSize)
            {
                SkillInfo = skillInfo;
                Sprite = new Grh(GrhInfo.GetData(SkillInfo.Icon));
                _cooldownManager = parent.CooldownManager;
            }

            public SkillInfoAttribute SkillInfo { get; private set; }

            /// <summary>
            /// Draws the <see cref="Control"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            protected override void DrawControl(ISpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);

                if (_isCoolingDown)
                {
                    var pos = ScreenPosition + new Vector2(Border.LeftWidth, Border.TopHeight);
                    Rectangle r = new Rectangle((int)pos.X, (int)pos.Y, (int)ClientSize.X, (int)ClientSize.Y);
                    XNARectangle.Draw(spriteBatch, r, new Color(0, 0, 0, 150));
                }
            }

            /// <summary>
            /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
            /// base class's method to ensure that changes to settings are hierchical.
            /// </summary>
            protected override void SetDefaultValues()
            {
                base.SetDefaultValues();

                StretchSprite = false;
                Size = _iconSize;
            }

            protected override void UpdateControl(int currentTime)
            {
                _isCoolingDown = _cooldownManager.IsCoolingDown(SkillInfo.CooldownGroup, currentTime);

                base.UpdateControl(currentTime);
            }
        }
    }
}