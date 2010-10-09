using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore;
using NetGore.Editor.WinForms;
using NetGore.IO;

// Highest priority (do these first!):
// TODO: Map loading
// TODO: Map saving + save as
// TODO: Creating a new map
// TODO: Displaying the map propeties
// TODO: Only show a single map in one screen (if trying to load a form with a map that is already open, just focus the existing form)
// TODO: Snapping to grid while placing, resizing, and moving spatials
// TODO: Right-click instead of left-click while holding control will, instead of place object, delete (for MapGrh cursor only)
// TODO: Selected objects form needs to be improved (dock the controls, only show list when more than one item available)
// TODO: Add back the editor for MapGrh-bound walls

// Medium priority (do these only when the highest priority ones are done):
// TODO: Figure out why the GrhDatas view takes so long to load
// TODO: Selected objects form be able to choose how the split for the listing is done (horizontal or vertical)
// TODO: Ability to toggle the ToolTip display on the map cursors
// TODO: Display some indication on which object the cursor is over when multiple are available

// Lowest priority (save these for the very end):
// TODO: Save the layout settings
// TODO: Move most of the keys & other configs into the Settings.settings so they can be changed at runtime
// TODO: Try to optimize the loading so that the editor loads as fast as possible, and move as much as possible into background loading
// TODO: Make the NPCChatEditorForm more "dock-alicious" and able to support opening & editing multiple different dialogs at once
// TODO: Make the SkeletonEditorForm more "dock-alicious" and able to support opening & editing multiple different dialogs at once
// TODO: Also split up the DbEditorForm into separate forms and able to open multiple instances at a time

namespace DemoGame.Editor
{
    static class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            if (!ContentPaths.TryCopyContent(userArgs: "--clean=\"[Engine,Font,Fx,Grh,Languages,Maps,Music,Skeletons,Sounds]\""))
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
            GlobalState.Initailize();

            // Get the command-line switches
            var switches = CommandLineSwitchHelper.GetCommandsUsingEnum<CommandLineSwitch>(args).ToArray();

            // Ensure the content is copied over
            if (!ContentPaths.TryCopyContent(userArgs: "--clean=\"[Engine,Font,Fx,Grh,Languages,Maps,Music,Skeletons,Sounds]\""))
            {
                const string errmsg =
                    "Failed to copy the content from the dev to build path." +
                    " Content in the build path will likely not update to reflect changes made in the content in the dev path.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg);
                Debug.Fail(errmsg);
            }

            // Start up the application
            Application.Run(new MainForm());
        }
    }
}