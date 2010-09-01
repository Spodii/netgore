using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Describes an <see cref="IDownloadSource"/> and provides a way to read and write the descriptor along with
    /// instantiate the <see cref="IDownloadSource"/> it describes.
    /// </summary>
    public class DownloadSourceDescriptor
    {
        readonly string _rootPath;
        readonly DownloadSourceType _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadSourceDescriptor"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootPath">The root path.</param>
        public DownloadSourceDescriptor(DownloadSourceType type, string rootPath)
        {
            _rootPath = rootPath;
            _type = type;
        }

        /// <summary>
        /// Gets the root path of the download source.
        /// </summary>
        public string RootPath
        {
            get { return _rootPath; }
        }

        /// <summary>
        /// Gets the type of download source.
        /// </summary>
        public DownloadSourceType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Creates many <see cref="DownloadSourceDescriptor"/>s from a file.
        /// </summary>
        /// <param name="filePath">The path to the file to read.</param>
        /// <returns>The <see cref="DownloadSourceDescriptor"/>s.</returns>
        public static IEnumerable<DownloadSourceDescriptor> FromDescriptorFile(string filePath)
        {
            var ret = new List<DownloadSourceDescriptor>();

            if (!File.Exists(filePath))
                return Enumerable.Empty<DownloadSourceDescriptor>();

            // Read the file
            var lines = File.ReadAllLines(filePath);

            // Try to create a descriptor from each line
            foreach (var line in lines)
            {
                // Create the descriptor
                DownloadSourceDescriptor desc;
                try
                {
                    desc = FromDescriptorString(line);
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.ToString());
                    desc = null;
                }

                // If successful, then add it to the list only if it is unique
                if (desc != null)
                {
                    if (!ret.Any(x => x.IsIdenticalTo(desc)))
                        ret.Add(desc);
                }
            }

            return ret;
        }

        /// <summary>
        /// Creates a <see cref="DownloadSourceDescriptor"/> from a string.
        /// </summary>
        /// <param name="descriptorString">The descriptor string.</param>
        public static DownloadSourceDescriptor FromDescriptorString(string descriptorString)
        {
            var split = descriptorString.Split('|');
            var type = (DownloadSourceType)Enum.Parse(typeof(DownloadSourceType), split[0], true);
            var rootPath = split[1].Trim();

            return new DownloadSourceDescriptor(type, rootPath);
        }

        /// <summary>
        /// Gets a string that can be used to reconstruct this <see cref="DownloadSourceDescriptor"/>.
        /// </summary>
        /// <returns>A string that can be used to reconstruct this <see cref="DownloadSourceDescriptor"/>.</returns>
        public string GetDescriptorString()
        {
            return string.Format("{0}|{1}", Type, RootPath);
        }

        /// <summary>
        /// Gets a string that can be used to reconstruct a <see cref="DownloadSourceDescriptor"/>.
        /// </summary>
        /// <param name="type">The <see cref="DownloadSourceType"/>.</param>
        /// <param name="rootPath">The download host.</param>
        /// <returns>
        /// A string that can be used to reconstruct a <see cref="DownloadSourceDescriptor"/>.
        /// </returns>
        public static string GetDescriptorString(DownloadSourceType type, string rootPath)
        {
            return string.Format("{0}|{1}", type, rootPath);
        }

        /// <summary>
        /// Creates an instance of the <see cref="IDownloadSource"/> described by this <see cref="DownloadSourceDescriptor"/>.
        /// </summary>
        /// <returns>The <see cref="IDownloadSource"/> instance.</returns>
        /// <exception cref="NotSupportedException">The <see cref="DownloadSourceType"/> is not supported.</exception>
        public IDownloadSource Instantiate()
        {
            switch (Type)
            {
                case DownloadSourceType.Http:
                    return new HttpDownloadSource(RootPath);

                default:
                    throw new NotSupportedException("Unsupported DownloadSourceType: " + Type);
            }
        }

        /// <summary>
        /// Checks if this <see cref="DownloadSourceDescriptor"/> is identical to another <see cref="DownloadSourceDescriptor"/>.
        /// </summary>
        /// <param name="other">The other <see cref="DownloadSourceDescriptor"/>.</param>
        /// <returns>True if they are identical; otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public bool IsIdenticalTo(DownloadSourceDescriptor other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            return this == other || Type == other.Type || RootPath == other.RootPath;
        }
    }
}