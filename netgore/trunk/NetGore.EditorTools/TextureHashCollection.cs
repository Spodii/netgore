using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using log4net;
using NetGore;
using NetGore.Graphics;
using NetGore.IO;

namespace NetGore.EditorTools
{
    public class TextureHashCollection
    {
        const string _fileSizeValueKey = "FileSize";
        const string _hashValueKey = "Hash";
        const string _lastModifiedValueKey = "LastModified";
        const string _rootNodeName = "TextureHashes";
        const string _textureNameValueKey = "TextureName";
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string _dataFile;
        readonly string _rootTextureDir;

        readonly Dictionary<string, HashInfo> _textureHash =
            new Dictionary<string, HashInfo>(StringComparer.CurrentCultureIgnoreCase);

        HashInfo this[string textureName]
        {
            get { return _textureHash[SanitizeTextureName(textureName)]; }
            set { _textureHash.Add(SanitizeTextureName(textureName), value); }
        }

        public TextureHashCollection()
        {
            _dataFile = ContentPaths.Build.Data.Join("grhhashes.xml");
            _rootTextureDir = ContentPaths.Build.Grhs;

            if (!_rootTextureDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                _rootTextureDir += Path.DirectorySeparatorChar.ToString();

            Load();
            UpdateHashes();
        }

        void Clear()
        {
            _textureHash.Clear();
        }

        bool Contains(string textureName)
        {
            return _textureHash.ContainsKey(SanitizeTextureName(textureName));
        }

        /// <summary>
        /// Checks which existing texture file closest resembles the specified <paramref name="matchTextureName"/>.
        /// </summary>
        /// <param name="matchTextureName">Name of the texture to to find the best match for.</param>
        /// <returns>Name of the texture file that best matches the specified <paramref name="matchTextureName"/>, or
        /// null if no decent matches were found.</returns>
        public string FindBestMatchTexture(string matchTextureName)
        {
            // Check that we even have this texture
            if (!Contains(matchTextureName))
                return null;

            // Get the info for the texture to match against
            HashInfo target = this[matchTextureName];

            // Loop through every existing hash until we find one with the same size and hash, but the file exists
            foreach (var kvp in _textureHash)
            {
                HashInfo hashInfo = kvp.Value;
                if (hashInfo != target && hashInfo.FileSize == target.FileSize && hashInfo.Hash == target.Hash)
                {
                    string textureName = kvp.Key;
                    if (GrhInfo.GrhTextureExists(textureName))
                        return textureName;
                }
            }

            return null;
        }

        static string GetFileHash(string filePath)
        {
            byte[] hash;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                hash = md5.ComputeHash(fs);
            }

            StringBuilder sb = new StringBuilder(hash.Length);
            foreach (byte hex in hash)
            {
                sb.Append(Convert.ToString(hex, 16));
            }

            return sb.ToString();
        }

        static void GetFileSizeAndLastModified(string filePath, out int size, out long lastMod)
        {
            FileInfo fi = new FileInfo(filePath);
            size = (int)fi.Length;
            lastMod = fi.LastWriteTime.ToBinary();
        }

        void Load()
        {
            Clear();

            if (!File.Exists(_dataFile))
                return;

            try
            {
                XmlValueReader reader = new XmlValueReader(_dataFile, _rootNodeName);
                var loadedHashes = reader.ReadManyNodes<KeyValuePair<string, HashInfo>>(_rootNodeName, Read);

                foreach (var hash in loadedHashes)
                {
                    this[hash.Key] = hash.Value;
                }
            }
            catch (Exception ex)
            {
                Clear();

                const string errmsg = "Failed to load TextureHashCollection from file `{0}`.";
                Debug.Fail(string.Format(errmsg, _dataFile));
                if (log.IsErrorEnabled)
                    log.Error(string.Format(errmsg, _dataFile), ex);
            }
        }

        static KeyValuePair<string, HashInfo> Read(IValueReader r)
        {
            string key = r.ReadString(_textureNameValueKey);
            string hash = r.ReadString(_hashValueKey);
            int fileSize = r.ReadInt(_fileSizeValueKey);
            long lastModified = r.ReadLong(_lastModifiedValueKey);

            HashInfo hashInfo = new HashInfo(hash, fileSize, lastModified);
            return new KeyValuePair<string, HashInfo>(key, hashInfo);
        }

        static string SanitizeTextureName(string textureName)
        {
            return textureName.Replace('\\', '/');
        }

        void Save()
        {
            using (IValueWriter w = new XmlValueWriter(_dataFile, _rootNodeName))
            {
                w.WriteManyNodes(_rootNodeName, _textureHash, Write);
            }
        }

        /// <summary>
        /// Updates the HashInfo for all of the files in the specified root directory.
        /// </summary>
        void UpdateHashes()
        {
            var files = Directory.GetFiles(_rootTextureDir, "*.xnb", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                // Get the current info
                string textureName = GrhInfo.GrhTextureNameFromFile(file);
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
                string hash = GetFileHash(file);
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

            // Write the updates
            Save();
        }

        static void Write(IValueWriter w, KeyValuePair<string, HashInfo> item)
        {
            w.Write(_textureNameValueKey, item.Key);
            w.Write(_hashValueKey, item.Value.Hash);
            w.Write(_fileSizeValueKey, item.Value.FileSize);
            w.Write(_lastModifiedValueKey, item.Value.LastModifiedTime);
        }

        class HashInfo
        {
            public int FileSize;
            public string Hash;
            public long LastModifiedTime;

            public HashInfo(string hash, int fileSize, long lastMod)
            {
                Hash = hash;
                FileSize = fileSize;
                LastModifiedTime = lastMod;
            }
        }
    }
}