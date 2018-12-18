using System.IO;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extension methods for the <see cref="Shader"/> class.
    /// </summary>
    public static class ShaderExtensions
    {
        /// <summary>
        /// Loads a <see cref="Shader"/> using code in memory.
        /// </summary>
        /// <param name="code">The GLSL code to use.</param>
        /// <returns>The loaded <see cref="Shader"/>.</returns>
        public static Shader LoadFromMemory(string code)
        {
            // To bypass the requirement of having to load from file, write the code to a temporary file then load the shader
            using (var tmpFile = new TempFile())
            {
                File.WriteAllText(tmpFile.FilePath, code);
                return new Shader(null, tmpFile.FilePath);
            }
        }
    }
}