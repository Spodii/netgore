using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using log4net;
using NetGore.Content;
using NetGore.IO;

namespace NetGore.EditorTools
{
    /// <summary>
    ///   A collection of hashes for textures.
    /// </summary>
    public class TextureHashCollection
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string _fileSizeValueKey = "FileSize";
        const string _hashValueKey = "Hash";
        const string _lastModifiedValueKey = "LastModified";
        const string _rootNodeName = "TextureHashes";
        const string _textureNameValueKey = "TextureName";

        readonly string _dataFile;
        readonly string _rootTextureDir;

        readonly Dictionary<string, HashInfo> _textureHash =
            new Dictionary<string, HashInfo>(StringComparer.CurrentCultureIgnoreCase);

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TextureHashCollection" /> class.
        /// </summary>
        public TextureHashCollection()
        {
            _dataFile = ContentPaths.Build.Data.Join("grhhashes"+EngineSettings.Instance.DataFileSuffix);
            _rootTextureDir = ContentPaths.Build.Grhs;

            if (!_rootTextureDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                _rootTextureDir += Path.DirectorySeparatorChar.ToString();

            Load();
            UpdateHashes();
        }

        /// <summary>
        ///   Gets or sets the <see cref = "HashInfo" /> for a texture.
        /// </summary>
        /// <param name = "textureName">The name of the texture.</param>
        /// <returns>The <see cref = "HashInfo" /> for the <paramref name = "textureName" />.</returns>
        HashInfo this[string textureName]
        {
            get { return _textureHash[SanitizeTextureName(textureName)]; }
            set { _textureHash.Add(SanitizeTextureName(textureName), value); }
        }

        /// <summary>
        ///   Clears the <see cref = "TextureHashCollection" /> of all the stored hashes.
        /// </summary>
        void Clear()
        {
            _textureHash.Clear();
        }

        /// <summary>
        ///   Checks if this <see cref = "TextureHashCollection" /> contains the given texture.
        /// </summary>
        /// <param name = "textureName">The name of the texture.</param>
        /// <returns>True if the <paramref name = "textureName" /> is in this collection; otherwise false.</returns>
        bool Contains(string textureName)
        {
            return _textureHash.ContainsKey(SanitizeTextureName(textureName));
        }

        /// <summary>
        ///   Checks which existing texture file closest resembles the specified <paramref name = "matchTextureName" />.
        /// </summary>
        /// <param name = "matchTextureName">Name of the texture to to find the best match for.</param>
        /// <returns>Name of the texture file that best matches the specified <paramref name = "matchTextureName" />, or
        ///   null if no decent matches were found.</returns>
        public string FindBestMatchTexture(string matchTextureName)
        {
            // Check that we even have this texture
            if (!Contains(matchTextureName))
                return null;

            // Get the info for the texture to match against
            var target = this[matchTextureName];

            // Loop through every existing hash until we find one with the same size and hash, but the file exists
            foreach (var kvp in _textureHash)
            {
                var hashInfo = kvp.Value;
                if (hashInfo == target || hashInfo.FileSize != target.FileSize || hashInfo.Hash != target.Hash)
                    continue;

                var textureName = kvp.Key;
                var assetName = new ContentAssetName(textureName);
                if (assetName.ContentExists())
                    return textureName;
            }

            return null;
        }

        /// <summary>
        ///   Gets the hash for a file.
        /// </summary>
        /// <param name = "filePath">The path to the file to get the hash for.</param>
        /// <returns>The hash for the given <paramref name = "filePath" />.</returns>
        static string GetFileHash(string filePath)
        {
            byte[] hash;
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                hash = md5.ComputeHash(fs);
            }

            var sb = new StringBuilder(hash.Length);
            foreach (var hex in hash)
            {
                sb.Append(Convert.ToString(hex, 16));
            }

            return sb.ToString();
        }

        /// <summary>
        ///   Gets the file size and last modified time.
        /// </summary>
        /// <param name = "filePath">The path to the file.</param>
        /// <param name = "size">Contains the size of the file in bytes.</param>
        /// <param name = "lastMod">Contains when the file was last modified.</param>
        static void GetFileSizeAndLastModified(string filePath, out int size, out long lastMod)
        {
            var fi = new FileInfo(filePath);
            size = (int)fi.Length;
            lastMod = fi.LastWriteTime.ToBinary();
        }

        /// <summary>
        /// Gets the paths of the files to get the hash for.
        /// </summary>
        /// <param name="rootDir">The root directory.</param>
        /// <returns>The paths of the files to get the hash for.</returns>
        static IEnumerable<string> GetFilesToHash(string rootDir)
        {
            // Skip Subversion directories
            if (rootDir.EndsWith(".svn", StringComparison.OrdinalIgnoreCase))
                yield break;

            // Recursively scan the child directories
            var childDirs = Directory.GetDirectories(rootDir, "*", SearchOption.TopDirectoryOnly);
            foreach (var cd in childDirs.SelectMany(GetFilesToHash))
            {
                yield return cd;
            }

            // Return the files
            var files = Directory.GetFiles(rootDir, "*" + ContentPaths.ContentFileSuffix, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                yield return file;
            }
        }

        /// <summary>
        ///   Loads the collection's values from file.
        /// </summary>
        void Load()
        {
            Clear();

            if (!File.Exists(_dataFile))
                return;

            try
            {
                var reader = new XmlValueReader(_dataFile, _rootNodeName);
                var loadedHashes = reader.ReadManyNodes(_rootNodeName, Read);

                foreach (var hash in loadedHashes)
                {
                    this[hash.Key] = hash.Value;
                }
            }
            catch (Exception ex)
            {
                Clear();

                const string errmsg = "Failed to load TextureHashCollection from file `{0}`.";
                if (log.IsErrorEnabled)
                    log.Error(string.Format(errmsg, _dataFile), ex);
                Debug.Fail(string.Format(errmsg, _dataFile));
            }
        }

        /// <summary>
        ///   Reads a <see cref = "KeyValuePair{T,U}" /> from an <see cref = "IValueReader" />.
        /// </summary>
        /// <param name = "r">The <see cref = "IValueReader" /> to read from.</param>
        /// <returns>The read <see cref = "KeyValuePair{T,U}" />.</returns>
        static KeyValuePair<string, HashInfo> Read(IValueReader r)
        {
            var key = r.ReadString(_textureNameValueKey);
            var hash = r.ReadString(_hashValueKey);
            var fileSize = r.ReadInt(_fileSizeValueKey);
            var lastModified = r.ReadLong(_lastModifiedValueKey);

            var hashInfo = new HashInfo(hash, fileSize, lastModified);
            return new KeyValuePair<string, HashInfo>(key, hashInfo);
        }

        /// <summary>
        ///   Sanitizes a texture's name.
        /// </summary>
        /// <param name = "textureName">The name to sanitize.</param>
        /// <returns>The sanitized name.</returns>
        static string SanitizeTextureName(string textureName)
        {
            return textureName.Replace('\\', '/');
        }

        /// <summary>
        ///   Saves the collection to file.
        /// </summary>
        void Save()
        {
            using (IValueWriter w = new XmlValueWriter(_dataFile, _rootNodeName))
            {
                w.WriteManyNodes(_rootNodeName, _textureHash, Write);
            }
        }

        /// <summary>
        ///   Updates the HashInfo for all of the files in the specified root directory.
        /// </summary>
        void UpdateHashes()
        {
            var files = GetFilesToHash(_rootTextureDir);
            foreach (var file in files)
            {
                try
                {
                    // Get the current info
                    var contentAssetName = ContentAssetName.FromAbsoluteFilePath(file, _rootTextureDir).Value;
                    var textureName = new TextureAssetName(contentAssetName).Value;
                    HashInfo hashInfo = null;
                    if (Contains(textureName))
                        hashInfo = this[textureName];

                    // Get the file size and last modified time
                    int size;
                    long lastMod;
                    GetFileSizeAndLastModified(file, out size, out lastMod);

                    // If we already have the hash info, and the size and last modified time have not changed,
                    // we can skip updating the hash
                    if (hashInfo != null && hashInfo.FileSize == size && hashInfo.LastModifiedTime == lastMod)
                        continue;

                    // Get the new hash and add the new file or update the existing values
                    var hash = GetFileHash(file);
                    if (hashInfo == null)
                    {
                        hashInfo = new HashInfo(hash, size, lastMod);
                        this[textureName] = hashInfo;
                    }
                    else
                    {
                        hashInfo.FileSize = size;
                        hashInfo.Hash = hash;
                        hashInfo.LastModifiedTime = lastMod;
                    }
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }

            // Write the updates
            Save();
        }

        /// <summary>
        ///   Writes a <see cref = "KeyValuePair{T,U}" />.
        /// </summary>
        /// <param name = "w">The <see cref = "IValueWriter" /> to write to.</param>
        /// <param name = "item">The value to write.</param>
        static void Write(IValueWriter w, KeyValuePair<string, HashInfo> item)
        {
            w.Write(_textureNameValueKey, item.Key);
            w.Write(_hashValueKey, item.Value.Hash);
            w.Write(_fileSizeValueKey, item.Value.FileSize);
            w.Write(_lastModifiedValueKey, item.Value.LastModifiedTime);
        }

        /// <summary>
        ///   Describes the hash information for a single texture.
        /// </summary>
        class HashInfo
        {
            /// <summary>
            ///   Initializes a new instance of the <see cref = "HashInfo" /> class.
            /// </summary>
            /// <param name = "hash">The hash.</param>
            /// <param name = "fileSize">Size of the file.</param>
            /// <param name = "lastMod">The file's last modified time.</param>
            public HashInfo(string hash, int fileSize, long lastMod)
            {
                Hash = hash;
                FileSize = fileSize;
                LastModifiedTime = lastMod;
            }

            /// <summary>
            ///   Gets or sets the size of the file.
            /// </summary>
            public int FileSize { get; set; }

            /// <summary>
            ///   Gets or sets the texture's hash.
            /// </summary>
            public string Hash { get; set; }

            /// <summary>
            ///   Gets or sets the last modified time of the file.
            /// </summary>
            public long LastModifiedTime { get; set; }
        }
    }
}