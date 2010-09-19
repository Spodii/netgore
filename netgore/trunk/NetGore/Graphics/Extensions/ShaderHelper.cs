using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics
{
    public static class ShaderHelper
    {
        /// <summary>
        /// Loads a <see cref="Shader"/> using code in memory.
        /// </summary>
        /// <param name="code">The GLSL code to use.</param>
        /// <returns>The loaded <see cref="Shader"/>.</returns>
        public static Shader LoadFromMemory(string code)
        {
            Shader ret;

            // To bypass the requirement of having to load from file, write the code to a temporary file then load the shader
            using (var tmpFile = new TempFile())
            {
                File.WriteAllText(tmpFile.FilePath, code);
                ret = new Shader(tmpFile.FilePath);
            }

            return ret;
        }
    }
}
