using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Audio;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Helper methods for the custom <see cref="UITypeEditor"/>s.
    /// </summary>
    public static class CustomUITypeEditors
    {
        static bool _added = false;

        /// <summary>
        /// Adds all of the custom <see cref="UITypeEditor"/>s.
        /// </summary>
        public static void AddEditors()
        {
            if (_added)
                return;

            _added = true;

            AddEditorsHelper(new EditorTypes(typeof(GrhIndex), typeof(GrhEditor)), new EditorTypes(typeof(Grh), typeof(GrhEditor)),
                             new EditorTypes(typeof(MusicID), typeof(MusicEditor)),
                             new EditorTypes(typeof(SoundID), typeof(SoundEditor)),
                             new EditorTypes(typeof(Color), typeof(XnaColorEditor)),
                             new EditorTypes(typeof(ParticleModifierCollection), typeof(ParticleModifierCollectionEditor)),
                             new EditorTypes(typeof(EmitterModifierCollection), typeof(EmitterModifierCollectionEditor)));
        }

        /// <summary>
        /// Adds the <see cref="EditorAttribute"/>s for the given <see cref="Type"/>s.
        /// </summary>
        /// <param name="items">The <see cref="Type"/>s and <see cref="Type"/> of the editor.</param>
        public static void AddEditorsHelper(params EditorTypes[] items)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                var attrib = new EditorAttribute(item.EditorType, typeof(UITypeEditor));
                TypeDescriptor.AddAttributes(item.Type, attrib);
            }
        }
    }
}