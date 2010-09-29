using System.Resources;

namespace NetGore.EditorTools.Docking
{
    static class ResourceHelper
    {
        static ResourceManager _resourceManager = null;

        static ResourceManager ResourceManager
        {
            get
            {
                if (_resourceManager == null)
                    _resourceManager = new ResourceManager("NetGore.EditorTools.Docking.Strings", typeof(ResourceHelper).Assembly);
                return _resourceManager;
            }
        }

        public static string GetString(string name)
        {
            return ResourceManager.GetString(name);
        }
    }
}