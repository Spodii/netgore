using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    public class ParticleEffectManager
    {
        const string _particleEffectsNodeName = "ParticleEffects";
        const string _rootNodeName = "ParticleEffectManager";
        static readonly ParticleEffectManager _instance;

        readonly Dictionary<string, ParticleEffect> _effects = new Dictionary<string, ParticleEffect>(ParticleEmitter.EmitterNameComparer);

        /// <summary>
        /// Initializes the <see cref="ParticleEffectManager"/> class.
        /// </summary>
        static ParticleEffectManager()
        {
            _instance = new ParticleEffectManager();
            _instance.Load(ContentPaths.Build);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectManager"/> class.
        /// </summary>
        ParticleEffectManager()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for when an instance of this class
        /// writes itself out to a new <see cref="GenericValueWriter"/>. If null, the format to use
        /// will be inherited from <see cref="GenericValueWriter.DefaultFormat"/>.
        /// Default value is null.
        /// </summary>
        public static GenericValueIOFormat? EncodingFormat { get; set; }

        /// <summary>
        /// Gets the <see cref="ParticleEffectManager"/> instance.
        /// </summary>
        public static ParticleEffectManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the name of all the <see cref="ParticleEffect"/>s in this <see cref="ParticleEffectManager"/>.
        /// </summary>
        public IEnumerable<string> ParticleEffectNames
        {
            get { return _effects.Keys; }
        }

        /// <summary>
        /// Adds a <see cref="ParticleEffect"/> to this <see cref="ParticleEffectManager"/>.
        /// </summary>
        /// <param name="effect">The <see cref="ParticleEffect"/> to add.</param>
        internal void AddEffect(ParticleEffect effect)
        {
            if (effect == null)
            {
                Debug.Fail("effect is null.");
                return;
            }

            Debug.Assert(!_effects.ContainsKey(effect.Name));

            _effects.Add(effect.Name, effect);
        }

        /// <summary>
        /// Sets a ParticleEffect template. This should only be used through the editor.
        /// </summary>
        public void Set(string name, ParticleEffect effect)
        {
            _effects.Remove(name);

            if (effect != null)
            {
                effect.Name = name;
                AddEffect(effect);
            }
        }

        /// <summary>
        /// Gets a name for a <see cref="ParticleEffect"/> in this <see cref="ParticleEffectManager"/> that is guaranteed to not already exist
        /// in this <see cref="ParticleEffectManager"/>.
        /// </summary>
        /// <param name="baseEffectName">The base name to use.</param>
        /// <returns>The name to use for a <see cref="ParticleEffect"/>, derived from the <paramref name="baseEffectName"/>, that
        /// is not already in use in this <see cref="ParticleEffectManager"/>.</returns>
        internal string GenerateUniqueEffectName(string baseEffectName)
        {
            // Initial check - see if the base name is available
            if (!_effects.ContainsKey(baseEffectName))
                return baseEffectName;

            // Base name not available, so start appending an incrementing value. So if baseEffectName = MyEffect, it will look like:
            //  MyEffect (1)
            //  MyEffect (2)
            //  MyEffect (3)
            //  ... and so on
            var i = 1;
            string newName;
            do
            {
                newName = baseEffectName + " (" + i + ")";
                i++;
            }
            while (_effects.ContainsKey(newName));

            Debug.Assert(!_effects.ContainsKey(newName));

            return newName;
        }

        /// <summary>
        /// Gets the file path to use for loading and saving the <see cref="ParticleEffectManager"/>.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to use.</param>
        /// <returns>The file path to the <see cref="ParticleEffectManager"/> file.</returns>
        public static string GetFilePath(ContentPaths contentPath)
        {
            return contentPath.Data.Join("particleeffects" + EngineSettings.DataFileSuffix);
        }

        /// <summary>
        /// Loads the <see cref="ParticleEffectManager"/> from file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to load from.</param>
        void Load(ContentPaths contentPath)
        {
            _effects.Clear();

            var filePath = GetFilePath(contentPath);
            var reader = GenericValueReader.CreateFromFile(filePath, _rootNodeName);
            var readEffects = reader.ReadManyNodes(_particleEffectsNodeName, r => new ParticleEffect(r));

            // Ensure the ParticleEffects were properly loaded into here
            Debug.Assert(readEffects.All(x => _effects[x.Name] == x));
        }

        /// <summary>
        /// Notifies this <see cref="ParticleEffectManager"/> that a <see cref="ParticleEffect"/>'s name has changed.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        internal void NotifyNameChanged(string oldName, string newName)
        {
            ParticleEffect pe;
            if (!_effects.TryGetValue(oldName, out pe))
            {
                Debug.Fail("Why did the key not exist?");
                return;
            }

            // Remove from the old name
            if (!string.IsNullOrEmpty(oldName))
                _effects.Remove(oldName);

            Debug.Assert(!_effects.ContainsKey(newName));

            // Add to the new name
            _effects.Add(newName, pe);
        }

        /// <summary>
        /// Removes a <see cref="ParticleEffect"/> from this <see cref="ParticleEffectManager"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="ParticleEffect"/> to remove.</param>
        /// <returns>True if the <see cref="ParticleEffect"/> with the given <paramref name="name"/> was successfully removed;
        /// otherwise false.</returns>
        public bool RemoveEffect(string name)
        {
            return _effects.Remove(name);
        }

        /// <summary>
        /// Saves the <see cref="ParticleEffectManager"/> to file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to save to.</param>
        public void Save(ContentPaths contentPath)
        {
            var filePath = GetFilePath(contentPath);
            using (var writer = GenericValueWriter.Create(filePath, _rootNodeName, EncodingFormat))
            {
                writer.WriteManyNodes(_particleEffectsNodeName, _effects.Values, (w, v) => v.WriteState(w));
            }
        }

        /// <summary>
        /// Tries to create a <see cref="ParticleEffect"/> instance from its name.
        /// </summary>
        /// <param name="name">The name of the <see cref="ParticleEffect"/> to create.</param>
        /// <returns>The <see cref="ParticleEffect"/> instance created from the given <paramref name="name"/>, or null if
        /// no such effect exists.</returns>
        public IParticleEffect TryCreateEffect(string name)
        {
            ParticleEffect pe;
            if (!_effects.TryGetValue(name, out pe))
                return null;

            return pe.DeepCopy();
        }
    }
}