using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeReleasePreparer
{
    public static class Paths
    {
        static string _root = null;

        /// <summary>
        /// Gets the path to the project root (trunk).
        /// </summary>
        public static string Root
        {
            get
            {
                if (_root == null)
                    _root = Path.GetFullPath(string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar));

                return _root;
            }
        }
    }
}
