using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public class ParticleEmitterUITypeEditorForm : UITypeEditorListForm<ParticleEmitter>
    {
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The currently selected <see cref="NPCChatDialogBase"/>.
        /// Multiple different types are supported. Can be null.</param>
        public ParticleEmitterUITypeEditorForm(object selected)
        {
            _selected = selected;
        }

        /// <summary>
        /// When overridden in the derived class, draws the <paramref name="item"/>.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawItemEventArgs"/> instance containing the event data.</param>
        /// <param name="item">The item being drawn.</param>
        protected override void DrawListItem(DrawItemEventArgs e, ParticleEmitter item)
        {
            e.DrawBackground();

            if (item == null)
                return;

            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(item.Name, e.Font, brush, e.Bounds);
            }
        }

        /// <summary>
        /// Loads the <see cref="ParticleEmitter"/> instances from their name, only returning those
        /// who were created without error.
        /// </summary>
        /// <param name="emitterNames">The <see cref="ParticleEmitter"/> names.</param>
        /// <returns>The <see cref="ParticleEmitter"/>s that successfully loaded.</returns>
        static IEnumerable<ParticleEmitter> EmitterLoader(IEnumerable<string> emitterNames)
        {
            var ret = new List<ParticleEmitter>();

            foreach (var name in emitterNames)
            {
                try
                {
                    var pe = ParticleEmitterFactory.LoadEmitter(ContentPaths.Build, name);
                    pe.SetEmitterLife(0, 0);
                    ret.Add(pe);
                }
                catch (ParticleEmitterNotFoundException ex)
                {
                    Debug.Fail(ex.ToString());
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.ToString());
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<ParticleEmitter> GetListItems()
        {
            var emitterNames = ParticleEmitterFactory.GetEffectsInPath(ContentPaths.Build);
            var validEffects = EmitterLoader(emitterNames);
            return validEffects;
        }

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="item"/> is valid to be
        /// used as the returned item.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        /// <returns>
        /// If the given <paramref name="item"/> is valid to be used as the returned item.
        /// </returns>
        protected override bool IsItemValid(ParticleEmitter item)
        {
            return item != null;
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override ParticleEmitter SetDefaultSelectedItem(IEnumerable<ParticleEmitter> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            var stringComp = StringComparer.Ordinal;

            if (_selected is string)
            {
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x.Name, asString));
            }

            if (_selected is ParticleEmitter)
            {
                var asEmitter = (ParticleEmitter)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x.Name, asEmitter));
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}