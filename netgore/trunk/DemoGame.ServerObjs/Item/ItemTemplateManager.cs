using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Collections;

namespace DemoGame.Server
{
    public static class ItemTemplateManager
    {
        static readonly DArray<IItemTemplateTable> _itemTemplates = new DArray<IItemTemplateTable>(32, false);

        static readonly Random _rnd = new Random();
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets if this class has been initialized.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// Get a random ItemTemplate.
        /// </summary>
        /// <returns>A random ItemTemplate. Will not be null.</returns>
        public static IItemTemplateTable GetRandomTemplate()
        {
            IItemTemplateTable template;
            do
            {
                int i = _rnd.Next(0, _itemTemplates.Count);
                Debug.Assert(_itemTemplates.CanGet(i));
                template = _itemTemplates[i];
            }
            while (template == null);

            return template;
        }

        /// <summary>
        /// Gets the IItemTemplateTable with the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the ItemTemplate.</param>
        /// <returns>The ItemTemplate with the specified <paramref name="index"/>, or null if none found for the index
        /// or the index is invalid.</returns>
        public static IItemTemplateTable GetTemplate(ItemTemplateID index)
        {
            if (!_itemTemplates.CanGet((int)index))
                return null;

            IItemTemplateTable ret = _itemTemplates[(int)index];
            Debug.Assert(ret.ID == index);
            return ret;
        }

        public static void Initialize(DBController dbController)
        {
            if (IsInitialized)
                return;

            IsInitialized = true;

            if (dbController == null)
                throw new ArgumentNullException("dbController");

            // Load the item templates
            var itemTemplates = dbController.GetQuery<SelectItemTemplatesQuery>().Execute();
            foreach (IItemTemplateTable it in itemTemplates)
            {
                _itemTemplates[(int)it.ID] = it;

                if (log.IsInfoEnabled)
                    log.InfoFormat("Loaded ItemTemplate `{0}`", it);
            }

            // Trim the DArray
            _itemTemplates.Trim();
        }
    }
}