using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore.Content;
using NetGore.Graphics;
using NetGore.IO;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// Handles the automatic GrhData generation.
    /// </summary>
    public static class AutomaticGrhDataUpdater
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// How many tasks there must be for us to show the <see cref="GrhDataUpdaterProgressForm"/>.
        /// </summary>
        const int _minTaskToShowTaskForm = 10;

        static readonly string[] _graphicFileSuffixes = {
            ".bmp", ".jpg", ".jpeg", ".dds", ".psd", ".png", ".gif", ".tga", ".hdr"
        };

        /// <summary>
        /// Finds all of the texture files from the root directory.
        /// </summary>
        /// <param name="rootDir">Root directory to search from.</param>
        /// <returns>List of the complete file paths from the <paramref name="rootDir"/>.</returns>
        static List<string> FindTextures(string rootDir)
        {
            var ret = new List<string>();
            var dirs = GetStationaryDirectories(rootDir);

            foreach (var dir in dirs)
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    var f = file;
                    if (!_graphicFileSuffixes.Any(x => f.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
                        continue;

                    ret.Add(f.Replace('\\', '/'));
                }
            }

            return ret;
        }

        /// <summary>
        /// Finds all of the used textures.
        /// </summary>
        /// <returns>Dictionary where the key is the virtual texture name (relative path minus the file extension) and
        /// the value is a list of GrhDatas that use that texture.</returns>
        static Dictionary<string, List<GrhData>> FindUsedTextures()
        {
            var ret = new Dictionary<string, List<GrhData>>(StringComparer.OrdinalIgnoreCase);

            // Loop through every stationary GrhData
            foreach (var gd in GrhInfo.GrhDatas.OfType<StationaryGrhData>())
            {
                var textureName = gd.TextureName.ToString();
                List<GrhData> dictList;

                // Get the existing list, or create a new one if the first entry
                if (!ret.TryGetValue(textureName, out dictList))
                {
                    dictList = new List<GrhData>();
                    ret.Add(textureName, dictList);
                }

                // Add the GrhData to the list
                dictList.Add(gd);
            }

            return ret;
        }

        /// <summary>
        /// Gets the directories that are for automatic animations.
        /// </summary>
        /// <param name="rootDir">The root directory.</param>
        /// <returns>The directories that contain textures and are for automatic animations.</returns>
        static IEnumerable<string> GetAnimatedDirectories(string rootDir)
        {
            if (AutomaticAnimatedGrhData.IsAutomaticAnimatedGrhDataDirectory(rootDir))
                yield return rootDir;
            else
            {
                foreach (var dir in Directory.GetDirectories(rootDir))
                {
                    foreach (var dir2 in GetAnimatedDirectories(dir))
                    {
                        yield return dir2;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="IAddGrhDataTask"/>s for animated <see cref="GrhData"/>s.
        /// </summary>
        /// <param name="rootGrhDir">The root grh dir.</param>
        /// <returns>The <see cref="IAddGrhDataTask"/>s for animated <see cref="GrhData"/>s.</returns>
        static IEnumerable<IAddGrhDataTask> GetAnimatedTasks(string rootGrhDir)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Searching for automatic animated GrhDatas from root `{0}`.", rootGrhDir);

            var ret = new List<IAddGrhDataTask>();

            // Find all directories that match the needed pattern
            var dirs = GetAnimatedDirectories(rootGrhDir);

            foreach (var dir in dirs)
            {
                // Grab the animation info from the directory
                var animInfo = AutomaticAnimatedGrhData.GetAutomaticAnimationInfo(dir);
                if (animInfo == null)
                    continue;

                // Get the virtual directory (remove the root)
                var partialDir = dir.Substring(rootGrhDir.Length);
                if (partialDir.StartsWith(Path.DirectorySeparatorChar.ToString()) ||
                    partialDir.StartsWith(Path.AltDirectorySeparatorChar.ToString()))
                    partialDir = partialDir.Substring(1);

                // Get the categorization
                var lastDirSep = partialDir.LastIndexOf(Path.DirectorySeparatorChar);
                if (lastDirSep < 0)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("Animated GrhData found at `{0}`, but could not be created because it has no category.",
                                       dir);
                    continue;
                }

                var categoryStr = partialDir.Substring(0, lastDirSep);

                var categorization = new SpriteCategorization(new SpriteCategory(categoryStr), new SpriteTitle(animInfo.Title));

                // Ensure the GrhData doesn't already exist
                if (GrhInfo.GetData(categorization) != null)
                    continue;

                // Create the task
                ret.Add(new AddAnimatedGrhDataTask(categorization));
            }

            return ret;
        }

        /// <summary>
        /// Gets the amount to trim off of a directory to make it relative to the specified <paramref name="rootDir"/>.
        /// </summary>
        /// <param name="rootDir">Root directory to make other directories relative to.</param>
        /// <returns>The amount to trim off of a directory to make it relative to the specified
        /// <paramref name="rootDir"/>.</returns>
        static int GetRelativeTrimLength(string rootDir)
        {
            var len = rootDir.Length;
            if (!rootDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                len++;
            return len;
        }

        static Form GetRootForm(Control c)
        {
            while (c != null && !(c is Form))
            {
                c = c.Parent;
            }

            return c as Form;
        }

        /// <summary>
        /// Gets the directories that are not for automatic animations.
        /// </summary>
        /// <param name="rootDir">The root directory.</param>
        /// <returns>The directories that contain textures that are not for automatic animations.</returns>
        static IEnumerable<string> GetStationaryDirectories(string rootDir)
        {
            yield return rootDir;

            foreach (var dir in Directory.GetDirectories(rootDir))
            {
                if (AutomaticAnimatedGrhData.IsAutomaticAnimatedGrhDataDirectory(dir))
                    continue;

                foreach (var dir2 in GetStationaryDirectories(dir))
                {
                    yield return dir2;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="IAddGrhDataTask"/>s for stationary <see cref="GrhData"/>s.
        /// </summary>
        /// <param name="rootGrhDir">The root grh dir.</param>
        /// <returns>The <see cref="IAddGrhDataTask"/>s for stationary <see cref="GrhData"/>s.</returns>
        static IEnumerable<IAddGrhDataTask> GetStationaryTasks(string rootGrhDir)
        {
            var dirSepChars = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

            if (log.IsInfoEnabled)
                log.InfoFormat("Searching for automatic stationary GrhDatas from root `{0}`.", rootGrhDir);

            // Get a List of all of the textures from the root directory
            var textures = FindTextures(rootGrhDir);

            // Get a List of all of the used textures
            var usedTextures = FindUsedTextures();

            // Grab the relative path instead of the complete file path since this
            // is how they are stored in the GrhData, then if it is in the usedTextures, remove it
            var trimLen = GetRelativeTrimLength(rootGrhDir);
            textures.RemoveAll(x => usedTextures.ContainsKey(TextureAbsoluteToRelativePath(trimLen, x)));

            // Check if there are any unused textures
            if (textures.Count == 0)
                return Enumerable.Empty<IAddGrhDataTask>();

            // Create the GrhDatas
            var ret = new List<IAddGrhDataTask>();
            foreach (var texture in textures)
            {
                // Go back to the relative path, and use it to figure out the categorization
                var relative = TextureAbsoluteToRelativePath(trimLen, texture);
                if (relative.LastIndexOfAny(dirSepChars) < 0)
                {
                    if (log.IsWarnEnabled)
                        log.WarnFormat("Stationary GrhData found at `{0}`, but could not be created because it has no category.",
                                       texture);
                    continue;
                }

                var categorization = SpriteCategorization.SplitCategoryAndTitle(relative);

                // Ensure the GrhData doesn't already exist
                if (GrhInfo.GetData(categorization) != null)
                    continue;

                // Create the task
                ret.Add(new AddStationaryGrhDataTask(categorization, relative));
            }

            return ret;
        }

        /// <summary>
        /// Converts an absolute path to a virtual texture path (relative path minus the extension).
        /// </summary>
        /// <param name="trimLen">Amount to trim off to make the path relative.</param>
        /// <param name="absolute">Absolute path to find the relative path of.</param>
        /// <returns>The relative path of the specified absolute path.</returns>
        static string TextureAbsoluteToRelativePath(int trimLen, string absolute)
        {
            absolute = absolute.Replace('\\', '/');

            // Trim down to the relative path
            var rel = absolute.Substring(trimLen);

            // Remove the file suffix since we don't use that
            var lastPeriod = rel.LastIndexOf('.');
            rel = rel.Substring(0, lastPeriod);

            return rel;
        }

        /// <summary>
        /// Updates all of the automaticly added GrhDatas.
        /// </summary>
        /// <param name="cm"><see cref="IContentManager"/> to use for new GrhDatas.</param>
        /// <param name="rootGrhDir">Root Grh texture directory.</param>
        /// <param name="sender">The <see cref="Control"/> that is invoking this event.</param>
        /// <returns>
        /// IEnumerable of all of the new GrhDatas created.
        /// </returns>
        public static IEnumerable<GrhData> Update(IContentManager cm, string rootGrhDir, Control sender = null)
        {
            var senderForm = GetRootForm(sender);

            // Clear the temporary content to make sure we have plenty of working memory
            cm.Unload(ContentLevel.Temporary, true);

            // Get the tasks
            var tasks = GetStationaryTasks(rootGrhDir).Concat(GetAnimatedTasks(rootGrhDir)).ToArray();

            // If we have a lot of tasks, show a "busy" form
            GrhDataUpdaterProgressForm frm = null;
            if (tasks.Length > _minTaskToShowTaskForm)
            {
                frm = new GrhDataUpdaterProgressForm(tasks.Length);

                if (senderForm != null)
                    frm.Show(senderForm);
                else
                    frm.Show();

                frm.Focus();
            }

            var createdGrhDatas = new List<GrhData>();

            if (senderForm != null)
            {
                senderForm.Enabled = false;
                senderForm.Visible = false;
            }

            try
            {
                // Start processing the tasks
                for (var i = 0; i < tasks.Length; i++)
                {
                    if (frm != null)
                        frm.UpdateStatus(i);

                    var newGD = tasks[i].Add(cm);
                    if (newGD != null)
                    {
                        createdGrhDatas.Add(newGD);
                        if (log.IsInfoEnabled)
                            log.InfoFormat("Created automatic GrhData `{0}` ({1} of {2} in batch).", newGD, i, tasks.Length);
                    }
                }
            }
            finally
            {
                // Dispose of the form, if needed
                if (frm != null)
                    frm.Dispose();

                if (senderForm != null)
                {
                    senderForm.Enabled = true;
                    senderForm.Visible = true;
                }
            }

            if (log.IsInfoEnabled)
                log.WarnFormat("Automatic GrhData creation update resulted in `{0}` new GrhData(s).", createdGrhDatas.Count);

            return createdGrhDatas;
        }

        /// <summary>
        /// Task to create a <see cref="AutomaticAnimatedGrhData"/>.
        /// </summary>
        class AddAnimatedGrhDataTask : IAddGrhDataTask
        {
            readonly SpriteCategorization _categorization;

            /// <summary>
            /// Initializes a new instance of the <see cref="AddAnimatedGrhDataTask"/> class.
            /// </summary>
            /// <param name="categorization">The <see cref="SpriteCategorization"/>.</param>
            public AddAnimatedGrhDataTask(SpriteCategorization categorization)
            {
                _categorization = categorization;
            }

            #region IAddGrhDataTask Members

            /// <summary>
            /// Adds the <see cref="GrhData"/>.
            /// </summary>
            /// <param name="cm">The <see cref="IContentManager"/> to use.</param>
            /// <returns>
            /// The <see cref="GrhData"/> that was added.
            /// </returns>
            public GrhData Add(IContentManager cm)
            {
                return GrhInfo.CreateAutomaticAnimatedGrhData(cm, _categorization);
            }

            #endregion
        }

        /// <summary>
        /// Task to add a <see cref="StationaryGrhData"/>.
        /// </summary>
        class AddStationaryGrhDataTask : IAddGrhDataTask
        {
            readonly SpriteCategorization _categorization;
            readonly string _relative;

            /// <summary>
            /// Initializes a new instance of the <see cref="AddStationaryGrhDataTask"/> class.
            /// </summary>
            /// <param name="categorization">The categorization.</param>
            /// <param name="relative">The relative path.</param>
            public AddStationaryGrhDataTask(SpriteCategorization categorization, string relative)
            {
                _categorization = categorization;
                _relative = relative;
            }

            #region IAddGrhDataTask Members

            /// <summary>
            /// Adds the GrhData.
            /// </summary>
            /// <param name="cm">The <see cref="IContentManager"/> to use.</param>
            public GrhData Add(IContentManager cm)
            {
                var gd = GrhInfo.CreateGrhData(cm, _categorization, _relative);
                gd.AutomaticSize = true;
                return gd;
            }

            #endregion
        }

        /// <summary>
        /// Interface for a task to add a <see cref="GrhData"/>.
        /// </summary>
        interface IAddGrhDataTask
        {
            /// <summary>
            /// Adds the <see cref="GrhData"/>.
            /// </summary>
            /// <param name="cm">The <see cref="IContentManager"/> to use.</param>
            /// <returns>The <see cref="GrhData"/> that was added.</returns>
            GrhData Add(IContentManager cm);
        }
    }
}