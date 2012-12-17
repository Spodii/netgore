using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SFML.Graphics;

namespace NetGore.Tests
{
    /// <summary>
    /// General helper methods for NetGore tests.
    /// </summary>
    public static class TestHelper
    {
        static string _devContentDir;

        /// <summary>
        /// Gets the path to the DevContent directory.
        /// </summary>
        public static string DevContentDir
        {
            get
            {
                if (_devContentDir == null)
                {
                    string exeLoc = Assembly.GetExecutingAssembly().Location;

                    // Keep going up the tree until we find DevContent
                    DirectoryInfo parent = Directory.GetParent(exeLoc);
                    while (parent != null)
                    {
                        if (parent.EnumerateDirectories().Any(x => StringComparer.OrdinalIgnoreCase.Equals("DevContent", x.Name)))
                        {
                            _devContentDir = parent.FullName;
                            break;
                        }
                        else
                        {
                            parent = parent.Parent;
                        }
                    }

                    // Check if we succeeded
                    if (_devContentDir == null)
                        throw new Exception("Failed to find DevContent directory from exe location: " + exeLoc);
                }

                return _devContentDir;
            }
        }

        static Font _defaultFont;

        /// <summary>
        /// Gets the defualt font.
        /// </summary>
        public static Font DefaultFont
        {
            get
            {
                if (_defaultFont == null)
                {
                    _defaultFont = LoadFont("Arial");

                    if (_defaultFont == null)
                        throw new Exception("Failed to create default font");
                }

                return _defaultFont;
            }
        }

        /// <summary>
        /// Gets the path to a font file in DevContent.
        /// </summary>
        static string GetFontPath(string fontName)
        {
            return Path.Combine(DevContentDir, "Font", fontName + ".ttf");
        }

        /// <summary>
        /// Loads a font by name.
        /// </summary>
        public static Font LoadFont(string fontName, uint defaultSize = 16)
        {
            string path = GetFontPath(fontName);
            return new Font(path) { DefaultSize = defaultSize };
        }
    }
}
