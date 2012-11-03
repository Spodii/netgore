using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using NetGore.IO;

// Highest priority (do these first!):
// TODO: Only show a single map in one screen (if trying to load a form with a map that is already open, just focus the existing form)
// TODO: Right-click instead of left-click while holding control will, instead of place object, delete (for MapGrh cursor only)

// Medium priority (do these only when the highest priority ones are done):
// TODO: Selected objects form be able to choose how the split for the listing is done (horizontal or vertical)
// TODO: Ability to toggle the ToolTip display on the map cursors
// TODO: Display some indication on which object the cursor is over when multiple are available
// TODO: MapPreviewTool should let you pick the file path to save to (using one of those file browser dialog box thingies)
// TODO: Command-line switch to save previews of all maps to a specific directory
// TODO: Be able to specify a custom filter (probably from the MapDisplayFilterTool) when using the MapPreviewTool

// Lowest priority (save these for the very end):
// TODO: Move most of the keys & other configs into the Settings.settings so they can be changed at runtime
// TODO: Make the NPCChatEditorForm more "dock-alicious" and able to support opening & editing multiple different dialogs at once
// TODO: Make the SkeletonEditorForm more "dock-alicious" and able to support opening & editing multiple different dialogs at once
// TODO: Also split up the DbEditorForm into separate forms and able to open multiple instances at a time

namespace DemoGame.Editor
{
    static class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Handles the <see cref="CommandLineSwitch.SaveMaps"/> switch.
        /// </summary>
        /// <param name="values">The values passed to the switch at the command line.</param>
        /// <returns>True if completed without errors; false if there were any errors.</returns>
        static bool HandleSwitch_SaveMaps(string[] values)
        {
            var ret = true;

            var camera = new Camera2D(GameData.ScreenSize);
            var dynamicEntityFactory = EditorDynamicEntityFactory.Instance;
            var contentPath = ContentPaths.Dev;

            // Get the maps
            var mapInfos = MapHelper.FindAllMaps();

            // For each map, load it then save it
            foreach (var mapInfo in mapInfos)
            {
                // Load
                EditorMap map;
                try
                {
                    map = new EditorMap(mapInfo.ID, camera, GetTimeDummy.Instance);
                    map.Load(contentPath, true, dynamicEntityFactory);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to load map ID `{0}`. Exception: {1}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, mapInfo.ID, ex);
                    Debug.Fail(string.Format(errmsg, mapInfo.ID, ex));
                    map = null;
                    ret = false;
                }

                // Save
                try
                {
                    if (map != null)
                        MapHelper.SaveMap(map, false);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to save map `{0}`. Exception: {1}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, mapInfo, ex);
                    Debug.Fail(string.Format(errmsg, mapInfo, ex));
                    ret = false;
                }
            }

            return ret;
        }

        /// <summary>
        /// Handles the command-line switches that are not applicable to the <see cref="MainForm"/>.
        /// </summary>
        /// <param name="switches">The switches to handle.</param>
        /// <returns>True if the program should close; false if the editor form should be loaded like normal.</returns>
        static bool HandleSwitches(IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> switches)
        {
            var hasErrors = false;
            var ret = false;

            // Loop through the switches
            foreach (var s in switches)
            {
                try
                {
                    // Process the switch
                    switch (s.Key)
                    {
                        case CommandLineSwitch.SaveMaps:
                            if (!HandleSwitch_SaveMaps(s.Value))
                                hasErrors = true;
                            break;

                        case CommandLineSwitch.Close:
                            ret = true;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Catch any unhandled exceptions from the switch handler
                    const string errmsg = "Failed to handle switch `{0}` (values: `{1}`). Terminating program. Exception: {2}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, s.Key, s.Value.Implode(), ex);
                    Debug.Fail(string.Format(errmsg, s.Key, s.Value.Implode(), ex));
                    hasErrors = true;
                    ret = true;
                }
            }

            // Display message when there were any errors
            if (hasErrors)
            {
                const string errmsg =
                    "WARNING: One or more command-line switches threw an error while executing." + " See the log for details.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                MessageBox.Show(errmsg, "Error handling switches");
            }

            return ret;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            log.Info("Starting editor...");

            ThreadAsserts.IsMainThread();

#if DEBUG
            WinFormExceptionHelper.AddUnhandledExceptionHooks();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check for a valid path to the development content
            if (ContentPaths.Dev == null)
            {
                const string errmsg =
                    @"Could not find the path to the development content (ContentPaths.Dev). The file containing this path should be located at:
    \Content\Data\devpath.txt

The path to the development content is required by the editor. See the file mentioned above for details.";
                MessageBox.Show(errmsg, "Error finding content path", MessageBoxButtons.OK);
                return;
            }

            // Ensure the content is copied over
            if (!ContentPaths.TryCopyContent(userArgs: CommonConfig.TryCopyContentArgs))
            {
                const string errmsg =
                    "Failed to copy the content from the dev to build path." +
                    " Content in the build path will likely not update to reflect changes made in the content in the dev path.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg);
                Debug.Fail(errmsg);
            }

            // Initialize stuff
            EngineSettingsInitializer.Initialize();
            try
            {
                GlobalState.Initialize();

                // Get the command-line switches
                var switches = CommandLineSwitchHelper.GetCommandsUsingEnum<CommandLineSwitch>(args).ToArray();
                var showEditor = !HandleSwitches(switches);

                if (showEditor)
                {
                    // Start up the application
                    Application.Run(new MainForm());
                }
            }
            finally
            {
                GlobalState.Destroy();
            }
        }
    }
}