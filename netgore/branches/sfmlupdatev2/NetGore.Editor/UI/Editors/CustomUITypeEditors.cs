using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using NetGore.AI;
using NetGore.Audio;
using NetGore.Features.NPCChat;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using SFML.Graphics;

namespace NetGore.Editor.UI
{
    /// <summary>
    /// Helper methods for the custom <see cref="UITypeEditor"/>s.
    /// </summary>
    public static class CustomUITypeEditors
    {
        static bool _added = false;
        static bool _addedAIID = false;
        static bool _addedNPCChatManager = false;

        /// <summary>
        /// Adds the <see cref="AIID"/> editor.
        /// </summary>
        /// <param name="aiFactory">The <see cref="IAIFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="aiFactory"/> is null.</exception>
        public static void AddAIIDEditor(IAIFactory aiFactory)
        {
            if (aiFactory == null)
                throw new ArgumentNullException("aiFactory");

            if (_addedAIID)
                return;

            _addedAIID = true;

            AIIDUITypeEditorForm.AIFactory = aiFactory;

            AddEditorsHelper(new EditorTypes(typeof(AIID), typeof(AIIDEditor)), new EditorTypes(typeof(AIID?), typeof(AIIDEditor)));
        }

        /// <summary>
        /// Adds all of the custom <see cref="UITypeEditor"/>s that do not require any additional parameters.
        /// </summary>
        public static void AddEditors()
        {
            if (_added)
                return;

            _added = true;

            AddEditorsHelper(new EditorTypes(typeof(Grh), typeof(GrhEditor)), new EditorTypes(typeof(GrhIndex), typeof(GrhEditor)),
                new EditorTypes(typeof(GrhIndex?), typeof(GrhEditor)), new EditorTypes(typeof(GrhData), typeof(GrhEditor)),
                new EditorTypes(typeof(MusicID), typeof(MusicEditor)), new EditorTypes(typeof(MusicID?), typeof(MusicEditor)),
                new EditorTypes(typeof(SoundID), typeof(SoundEditor)), new EditorTypes(typeof(SoundID?), typeof(SoundEditor)),
                new EditorTypes(typeof(Color), typeof(SFMLColorEditor)), new EditorTypes(typeof(Color?), typeof(SFMLColorEditor)),
                new EditorTypes(typeof(ParticleModifierCollection), typeof(ParticleModifierCollectionEditor)),
                new EditorTypes(typeof(PolygonPointCollection), typeof(PolygonPointCollectionEditor)),
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

        /// <summary>
        /// Adds the <see cref="NPCChatDialogBase"/> editor.
        /// </summary>
        /// <param name="chatManager">The <see cref="NPCChatManagerBase"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="chatManager"/> is null.</exception>
        public static void AddNPCChatDialogEditor(NPCChatManagerBase chatManager)
        {
            if (chatManager == null)
                throw new ArgumentNullException("chatManager");

            if (_addedNPCChatManager)
                return;

            _addedNPCChatManager = true;

            NPCChatDialogUITypeEditorForm.NPCChatManager = chatManager;

            AddEditorsHelper(new EditorTypes(typeof(NPCChatDialogID), typeof(NPCChatDialogEditor)),
                new EditorTypes(typeof(NPCChatDialogID?), typeof(NPCChatDialogEditor)));
        }
    }
}