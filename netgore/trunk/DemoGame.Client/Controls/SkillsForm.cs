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
            IsVisible = false;

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

        public sealed class SkillPictureBox : PictureBox, IDragDropProvider, IQuickBarItemProvider
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

            /// <summary>
            /// Gets the <see cref="SkillInfoAttribute"/> for the skill represented by this control.
            /// </summary>
            public SkillInfoAttribute SkillInfo { get; private set; }

            /// <summary>
            /// Draws the <see cref="Control"/>.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            protected override void DrawControl(ISpriteBatch spriteBatch)
            {
                base.DrawControl(spriteBatch);

                if (_isCoolingDown)
                    XNARectangle.Draw(spriteBatch, GetScreenArea(), new Color(0, 0, 0, 150));
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

            /// <summary>
            /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(int currentTime)
            {
                _isCoolingDown = _cooldownManager.IsCoolingDown(SkillInfo.CooldownGroup, currentTime);

                base.UpdateControl(currentTime);
            }

            /// <summary>
            /// Gets if this <see cref="IDragDropProvider"/> can be dragged. In the case of something that only
            /// supports having items dropped on it but not dragging, this will always return false. For items that can be
            /// dragged, this will return false if there is currently nothing to drag (such as an empty inventory slot) or
            /// there is some other reason that this item cannot currently be dragged.
            /// </summary>
            bool IDragDropProvider.CanDragContents
            {
                get { return SkillInfo != null; }
            }

            /// <summary>
            /// Draws the item that this <see cref="IDragDropProvider"/> contains for when this item
            /// is being dragged.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
            /// <param name="position">The position to draw the sprite at.</param>
            /// <param name="color">The color to use when drawing the item.</param>
            void IDragDropProvider.DrawDraggedItem(ISpriteBatch spriteBatch, Vector2 position, Color color)
            {
                if (Sprite != null)
                    Sprite.Draw(spriteBatch, position, color);
            }

            /// <summary>
            /// Gets if the specified <see cref="IDragDropProvider"/> can be dropped on this <see cref="IDragDropProvider"/>.
            /// </summary>
            /// <param name="source">The <see cref="IDragDropProvider"/> to check if can be dropped on this
            /// <see cref="IDragDropProvider"/>. This value will never be null.</param>
            /// <returns>True if the <paramref name="source"/> can be dropped on this <see cref="IDragDropProvider"/>;
            /// otherwise false.</returns>
            bool IDragDropProvider.CanDrop(IDragDropProvider source)
            {
                return false;
            }

            /// <summary>
            /// Handles when the specified <see cref="IDragDropProvider"/> is dropped on this <see cref="IDragDropProvider"/>.
            /// </summary>
            /// <param name="source">The <see cref="IDragDropProvider"/> that is being dropped on this
            /// <see cref="IDragDropProvider"/>.</param>
            void IDragDropProvider.Drop(IDragDropProvider source)
            {
            }

            /// <summary>
            /// Draws a visual highlighting on this <see cref="IDragDropProvider"/> for when an item is being
            /// dragged onto it but not yet dropped.
            /// </summary>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
            void IDragDropProvider.DrawDropHighlight(ISpriteBatch spriteBatch)
            {
            }

            /// <summary>
            /// Gets the <see cref="QuickBarItemType"/> and value to add to the quick bar.
            /// </summary>
            /// <param name="type">When this method returns true, contains the <see cref="QuickBarItemType"/>
            /// to add.</param>
            /// <param name="value">When this method returns true, contains the value for for the quick bar item.</param>
            /// <returns>True if the item can be added to the quick bar; otherwise false.</returns>
            bool IQuickBarItemProvider.TryAddToQuickBar(out QuickBarItemType type, out int value)
            {
                type = QuickBarItemType.Skill;
                value = 0;

                if (SkillInfo == null)
                    return false;

                value = (int)SkillInfo.Value;

                return true;
            }
        }
    }
}