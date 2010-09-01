using System.Linq;

namespace GoreUpdater
{
    public static class GlobalSettings
    {
        public static string ReplacerFileName
        {
            get { 
                // TODO: Does this string really need to be global?
                return "filereplacer.bat"; }
        }
    }
}