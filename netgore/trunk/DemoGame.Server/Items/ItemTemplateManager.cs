using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Collections;

namespace DemoGame.Server
{
    public static class ItemTemplateManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly DArray<ItemTemplate> _itemTemplates = new DArray<ItemTemplate>(32, false);

        /// <summary>
        /// Gets if this class has been initialized.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        public static void Initialize(DBController dbController)
        {
            if (IsInitialized)
                return;

            IsInitialized = true;

            if (dbController == null)
                throw new ArgumentNullException("dbController");

            // Load the item templates
            var itemTemplates = dbController.GetQuery<SelectItemTemplatesQuery>().Execute();
            foreach (ItemTemplate it in itemTemplates)
            {
                _itemTemplates[(int)it.ID] = it;

                if (log.IsInfoEnabled)
                    log.InfoFormat("Loaded ItemTemplate `{0}`", it);
            }

            // Trim the DArray
            _itemTemplates.Trim();
        }

        static readonly Random _rnd = new Random();

        /// <summary>
        /// Get a random ItemTemplate.
        /// </summary>
        /// <returns>A random ItemTemplate. Will not be null.</returns>
        public static ItemTemplate GetRandomTemplate()
        {
            ItemTemplate template;
            do
            {
                int i = _rnd.Next(0, _itemTemplates.Count);
                Debug.Assert(_itemTemplates.CanGet(i));
                template = _itemTemplates[i];
            } while (template == null);

            return template;
        }

        /// <summary>
        /// Gets the ItemTemplate with the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the ItemTemplate.</param>
        /// <returns>The ItemTemplate with the specified <paramref name="index"/>, or null if none found for the index
        /// or the index is invalid.</returns>
        public static ItemTemplate GetTemplate(ItemTemplateID index)
        {
            if (!_itemTemplates.CanGet((int)index))
                return null;

            return _itemTemplates[(int)index];
        }
    }
}