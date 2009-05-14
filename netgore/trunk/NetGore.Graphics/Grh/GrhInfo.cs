using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework.Content;
using NetGore.Collections;

namespace NetGore.Graphics
{
    /// <summary>
    /// Holds the GrhData and related methods
    /// </summary>
    public static class GrhInfo
    {
        /// <summary>
        /// List of all the GrhData where the array index is the GrhIndex
        /// </summary>
        public static DArray<GrhData> GrhDatas;

        /// <summary>
        /// Dictionary of categories, which contains a dictionary of all the names of the GrhDatas in
        /// that category, which contains the GrhData of that given name and category
        /// </summary>
        static readonly Dictionary<string, Dictionary<string, GrhData>> _catDic =
            new Dictionary<string, Dictionary<string, GrhData>>(_comparer);

        /// <summary>
        /// The StringComparer used for the GrhData dictionaries. This must be used for all of the
        /// dictionaries created for the GrhData categorization.
        /// </summary>
        static readonly StringComparer _comparer = StringComparer.OrdinalIgnoreCase;

        /// <summary>
        /// Adds a GrhData to the dictionary
        /// </summary>
        /// <param name="gd">GrhData to add to the dictionary</param>
        static void AddToDictionary(GrhData gd)
        {
            Dictionary<string, GrhData> titleDic;

            // Check if the category exists
            if (!_catDic.TryGetValue(gd.Category, out titleDic))
            {
                // Category does not exist, so create and add it
                titleDic = new Dictionary<string, GrhData>(_comparer);
                _catDic.Add(gd.Category, titleDic);
            }

            // Add the GrhData to the sub-dictionary by its title
            titleDic.Add(gd.Title, gd);
        }

        public static GrhData CreateGrhData(ContentManager contentManager, string category)
        {
            if (category == null)
                category = string.Empty;

            ushort grhIndex = NextFreeIndex();
            string title = "tmp" + grhIndex;

            // Create the new GrhData
            GrhData gd = new GrhData();
            gd.Load(contentManager, grhIndex, string.Empty, 0, 0, 0, 0, category, title);
            GrhDatas[gd.GrhIndex] = gd;

            return gd;
        }

        /// <summary>
        /// Gets the GrhData by the given information
        /// </summary>
        /// <param name="category">Category of the GrhData</param>
        /// <param name="title">Title of the GrhData</param>
        /// <returns>GrhData matching the given information if found, or null if no matches</returns>
        public static GrhData GetData(string category, string title)
        {
            // Get the dictionary for the category
            var titleDic = GetData(category);

            // If we failed to get it, return null
            if (titleDic == null)
                return null;

            // Try and get the GrhData for the given title, returning the GrhData
            // if it exists, or null if it does not exist in the dictionary
            GrhData ret;
            if (titleDic.TryGetValue(title, out ret))
                return ret;
            else
                return null;
        }

        /// <summary>
        /// Gets the GrhData by the given information
        /// </summary>
        /// <param name="category">Category of the GrhData</param>
        /// <returns>Dictionary of all GrhDatas from the given category with their Title as the key, or
        /// null if the category was invalid</returns>
        public static Dictionary<string, GrhData> GetData(string category)
        {
            Dictionary<string, GrhData> titleDic;

            // Return the whole dictionary for the given catgory, or null if it does not exist
            if (_catDic.TryGetValue(category, out titleDic))
                return titleDic;
            else
                return null;
        }

        /// <summary>
        /// Gets the GrhData by the given information
        /// </summary>
        /// <param name="grhIndex">GrhIndex of the GrhData</param>
        /// <returns>GrhData matching the given information if found, or null if no matches</returns>
        public static GrhData GetData(int grhIndex)
        {
            if (GrhDatas.CanGet(grhIndex))
                return GrhDatas[grhIndex];
            else
                return null;
        }

        /// <summary>
        /// Loads all of the GrhData information
        /// </summary>
        /// <param name="path">Path to the file containing the GrhData information</param>
        /// <param name="cm">ContentManager to apply to the created GrhDatas</param>
        public static void Load(string path, ContentManager cm)
        {
            if (cm == null)
                throw new ArgumentNullException("cm");

            if (!File.Exists(path))
                throw new ArgumentException("Specified file path does not exist.", "path");

            // Create the GrhData DArray
            GrhDatas = new DArray<GrhData>(1024, false);
            _catDic.Clear();
            GrhDatas.OnAdd += OnAdd;
            GrhDatas.OnRemove += OnRemove;

            // Load the GrhData from the file
            var grhDataFile = XmlInfoReader.ReadFile(path, true);
            if (grhDataFile.Count <= 0)
                throw new Exception("Error loading GrhData information from the specified file.");

            // Go through once to create all the objects to avoid reference conflicts
            foreach (var dic in grhDataFile)
            {
                ushort currGrhIndex = ushort.Parse(dic["Grh.Index"]);
                GrhDatas[currGrhIndex] = new GrhData();
            }

            // Load the information into the objects
            foreach (var dic in grhDataFile)
            {
                ushort currGrhIndex = ushort.Parse(dic["Grh.Index"]);
                GrhData currGrh = GrhDatas[currGrhIndex];
                int numFrames = int.Parse(dic["Grh.Frames.Count"]);

                // Categorization
                string category = dic["Grh.Category.Name"];
                string title = dic["Grh.Category.Title"];

                if (numFrames == 1)
                {
                    // Single frame
                    string file = dic["Grh.Texture.File"];
                    int x = int.Parse(dic["Grh.Texture.X"]);
                    int y = int.Parse(dic["Grh.Texture.Y"]);
                    int w = int.Parse(dic["Grh.Texture.W"]);
                    int h = int.Parse(dic["Grh.Texture.H"]);
                    currGrh.Load(cm, currGrhIndex, file, x, y, w, h, category, title);
                }
                else
                {
                    // Multiple frames
                    float speed = float.Parse(dic["Grh.Anim.Speed"]);
                    var frames = new ushort[numFrames];
                    for (ushort i = 0; i < frames.Length; i++)
                    {
                        frames[i] = ushort.Parse(dic["Grh.Frames.F" + (i + 1)]);
                    }
                    currGrh.Load(currGrhIndex, frames, 1f / speed, category, title);
                }
            }

            // Trim down the GrhData array
            GrhDatas.Trim();
        }

        /// <summary>
        /// Finds the next free GrhData
        /// </summary>
        /// <returns>Next free GrhData index</returns>
        public static ushort NextFreeIndex()
        {
            if (GrhDatas.TrackFree)
                return (ushort)GrhDatas.NextFreeIndex();

            // Start at the first index
            ushort i = 1;

            // Just loop through the indicies until we find one thats not in use or
            // passes the length of the GrhDatas array (which means its obviously not in use)
            while (GrhDatas.CanGet(i) && GrhDatas[i] != null)
            {
                i++;
            }

            return i;
        }

        /// <summary>
        /// Handles when a GrhData is added to the GrhDatas DArray
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        static void OnAdd(object sender, DArrayModifyEventArgs<GrhData> e)
        {
            GrhData gd = e.Item;
            if (gd == null)
            {
                Debug.Fail("gd is null.");
                return;
            }

            // Set the categorization event so we can keep up with any changes to the categorization.
            gd.OnChangeCategorization += OnChangeCategorization;

            // If the title and category are not null, which would happen if the GrhData
            // was created and initialized, THEN added to the array, then add it to the dictionary
            if (gd.Category != null && gd.Title != null)
                AddToDictionary(gd);
        }

        /// <summary>
        /// Handles when the category of a GrhData in the DArray changes
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        static void OnChangeCategorization(object sender, EventArgs e)
        {
            GrhData gd = sender as GrhData;
            if (gd == null)
            {
                Debug.Fail("gd is null.");
                return;
            }

            RemoveFromDictionary(gd);
            AddToDictionary(gd);
        }

        /// <summary>
        /// Handles when a GrhData is removed from the DArray
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        static void OnRemove(object sender, DArrayModifyEventArgs<GrhData> e)
        {
            RemoveFromDictionary(e.Item);
        }

        /// <summary>
        /// Removes a GrhData from the dictionary
        /// </summary>
        /// <param name="gd">GrhData to remove</param>
        static void RemoveFromDictionary(GrhData gd)
        {
            Dictionary<string, GrhData> titleDic;
            if (_catDic.TryGetValue(gd.Category, out titleDic))
                titleDic.Remove(gd.Title);
        }

        /// <summary>
        /// Saves all of the GrhData information to the specified file.
        /// </summary>
        /// <param name="path">Path of the file to save the GrhData information to.</param>
        public static void Save(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            string tempPath = path + ".temp";

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            // Open the FileStream
            using (FileStream stream = new FileStream(tempPath, FileMode.Create))
            {
                // Create the XmlWriter and settings
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter w = XmlWriter.Create(stream, settings))
                {
                    if (w == null)
                        throw new Exception("Failed to create XmlWriter for saving GrhInfo.");

                    // Begin
                    w.WriteStartDocument();
                    w.WriteStartElement("Grhs");

                    // Get all of the single and multiple frame GrhDatas
                    var nonNullDatas = GrhDatas.Where(gd => gd != null);
                    var singleFrames = nonNullDatas.Where(gd => gd.Frames == null);
                    var multiFrames = nonNullDatas.Where(gd => gd.Frames != null);

                    // Save the single frame GrhDatas first, then the animated GrhDatas
                    foreach (GrhData grhData in singleFrames)
                    {
                        grhData.Save(w);
                    }
                    foreach (GrhData grhData in multiFrames)
                    {
                        grhData.Save(w);
                    }

                    // End
                    w.WriteEndElement();
                    w.WriteEndDocument();
                    w.Flush();
                }
            }

            // Now that the temporary file has been successfully written, replace the existing file with it
            File.Delete(path);
            File.Move(tempPath, path);
        }
    }
}