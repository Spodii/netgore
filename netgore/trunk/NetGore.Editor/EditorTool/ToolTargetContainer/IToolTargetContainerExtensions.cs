using System.Linq;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Extension methods for the <see cref="IToolTargetContainer"/> interface.
    /// </summary>
    public static class IToolTargetContainerExtensions
    {
        /// <summary>
        /// Gets the <see cref="IToolTargetContainer"/> as a <see cref="IToolTargetMapContainer"/>.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to try to cast to the
        /// desired derived tool container type.</param>
        /// <returns>The <paramref name="c"/> as <see cref="IToolTargetMapContainer"/>, or null
        /// if <paramref name="c"/> is not of the expected type.</returns>
        public static IToolTargetMapContainer AsMapContainer(this IToolTargetContainer c)
        {
            return c as IToolTargetMapContainer;
        }
    }
}