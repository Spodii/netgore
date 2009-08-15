using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.Client
{
    public delegate void UseSkillHandler(SkillType skillType);

    public class SkillsForm : Form, IRestorableSettings
    {
        public SkillsForm(Vector2 position, Control parent)
            : base(parent.GUIManager, "Skills", position, new Vector2(200, 200), parent)
        {
            var allSkillTypes = Enum.GetValues(typeof(SkillType)).Cast<SkillType>();
            Vector2 offset = Vector2.Zero;
            foreach (var skillType in allSkillTypes)
            {
                CreateSkillLabel(offset, skillType);
                offset += new Vector2(0, Font.LineSpacing);
            }
        }

        void CreateSkillLabel(Vector2 position, SkillType skillType)
        {
            var skillInfo = SkillInfoManager.GetSkillInfo(skillType);
            var skillLabel = new SkillLabel(this, skillInfo, position);
            skillLabel.OnClick += SkillLabel_OnClick;
        }

        void SkillLabel_OnClick(object sender, MouseClickEventArgs e)
        {
            if (OnUseSkill != null)
            {
                SkillLabel source = (SkillLabel)sender;
                OnUseSkill(source.SkillInfo.SkillType);
            }
        }

        public event UseSkillHandler OnUseSkill;

        /// <summary>
        /// Loads the values supplied by the <paramref name="items"/> to reconstruct the settings.
        /// </summary>
        /// <param name="items">NodeItems containing the values to restore.</param>
        public void Load(IDictionary<string, string> items)
        {
            Position = new Vector2(float.Parse(items["X"]), float.Parse(items["Y"]));
            IsVisible = bool.Parse(items["IsVisible"]);
        }

        /// <summary>
        /// Returns the key and value pairs needed to restore the settings.
        /// </summary>
        /// <returns>The key and value pairs needed to restore the settings.</returns>
        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[] { new NodeItem("X", Position.X), new NodeItem("Y", Position.Y), new NodeItem("IsVisible", IsVisible) };
        }

        class SkillLabel : Label
        {
            public SkillInfo SkillInfo { get; private set; }

            public SkillLabel(Control parent, SkillInfo skillInfo, Vector2 position)
                : base(skillInfo.Name, position, parent)
            {
                SkillInfo = skillInfo;
            }
        }
    }
}
