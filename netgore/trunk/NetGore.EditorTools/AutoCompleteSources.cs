using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.IO;

namespace NetGore.EditorTools
{
    public static class AutoCompleteSources
    {
        static readonly AutoCompleteStringCollection _textures;

        /// <summary>
        /// Initializes the <see cref="AutoCompleteSources"/> class.
        /// </summary>
        static AutoCompleteSources()
        {
            // Textures source
            _textures = new AutoCompleteStringCollection();
            var files = Directory.GetFiles(ContentPaths.Build.Grhs, "*." + ContentPaths.CompiledContentSuffix, SearchOption.AllDirectories);
            
            int start = ContentPaths.Build.Grhs.ToString().Length + 1;
            int trimEnd = ContentPaths.CompiledContentSuffix.Length + 1;
            files = files.Select(x => x.Substring(start, x.Length - start - trimEnd).Replace(Path.DirectorySeparatorChar, '/')).ToArray();
            _textures.AddRange(files);
        }

        /// <summary>
        /// Gets the <see cref="AutoCompleteStringCollection"/> for the textures.
        /// </summary>
        public static AutoCompleteStringCollection Textures
        {
            get
            {
                return _textures;
            }
        }
    }
}
