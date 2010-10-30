using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Features.NPCChat;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.Editor.UI
{
    public class ParticleEffectUITypeEditorForm : UITypeEditorListForm<IParticleEffect>
    {
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The currently selected <see cref="NPCChatDialogBase"/>.
        /// Multiple different types are supported. Can be null.</param>
        public ParticleEffectUITypeEditorForm(object selected)
        {
            _selected = selected;
        }

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item to get the display string for.</param>
        /// <returns>The string to display for the <paramref name="item"/>.</returns>
        protected override string GetItemDisplayString(IParticleEffect item)
        {
            return item.Name;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<IParticleEffect> GetListItems()
        {
            var names = ParticleEffectManager.Instance.ParticleEffectNames;
            var instances = names.Select(x => ParticleEffectManager.Instance.TryCreateEffect(x));
            var validInstances = instances.Where(x => x != null);
            return validInstances.OrderBy(x => x.Name, NaturalStringComparer.Instance);
        }

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="item"/> is valid to be
        /// used as the returned item.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        /// <returns>
        /// If the given <paramref name="item"/> is valid to be used as the returned item.
        /// </returns>
        protected override bool IsItemValid(IParticleEffect item)
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
        protected override IParticleEffect SetDefaultSelectedItem(IEnumerable<IParticleEffect> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            var stringComp = StringComparer.Ordinal;

            if (_selected is string)
            {
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x.Name, asString));
            }

            if (_selected is IParticleEffect)
            {
                var asPE = (IParticleEffect)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x.Name, asPE));
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}