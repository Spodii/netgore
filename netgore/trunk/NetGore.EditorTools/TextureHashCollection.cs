using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    public class TextureHashCollection
    {
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
            _dataFile = ContentPaths.Build.Data.Join("grhhashes.dat");
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
            // TODO: If there are problems loading the hash file, should just force-rebuild it all
            if (!File.Exists(_dataFile))
                return;

            Clear();

            using (FileStream fs = File.Open(_dataFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new BinaryReader(fs))
                {
                    int entries = r.ReadInt32();
                    for (int i = 0; i < entries; i++)
                    {
                        string textureName = r.ReadString();
                        string hash = r.ReadString();
                        int fileSize = r.ReadInt32();
                        long lastMod = r.ReadInt64();

                        this[textureName] = new HashInfo(hash, fileSize, lastMod);
                    }
                }
            }
        }

        static string SanitizeTextureName(string textureName)
        {
            return textureName.Replace('\\', '/');
        }

        void Save()
        {
            using (FileStream fs = File.Open(_dataFile, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter w = new BinaryWriter(fs))
                {
                    w.Write(_textureHash.Count);
                    foreach (var kvp in _textureHash)
                    {
                        string textureName = kvp.Key;
                        string hash = kvp.Value.Hash;
                        int fileSize = kvp.Value.FileSize;
                        long lastMod = kvp.Value.LastModifiedTime;

                        w.Write(textureName);
                        w.Write(hash);
                        w.Write(fileSize);
                        w.Write(lastMod);
                    }
                }
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

            // Save the updates
            Save();
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