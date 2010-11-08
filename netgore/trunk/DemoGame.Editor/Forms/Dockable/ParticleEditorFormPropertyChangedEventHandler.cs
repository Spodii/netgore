using System.Linq;

namespace DemoGame.Editor
{
    /// <summary>
    /// Delegate for handling events for properties changing on the <see cref="ParticleEditorForm"/>.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    public delegate void ParticleEditorFormPropertyChangedEventHandler<in T>(ParticleEditorForm sender, T oldValue, T newValue);
}