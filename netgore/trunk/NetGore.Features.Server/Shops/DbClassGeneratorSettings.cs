using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Db.ClassCreator;

namespace NetGore.Features.Shops
{
    /// <summary>
    /// Implementation of the <see cref="IDbClassGeneratorSettingsProvider"/> for the shop database tables.
    /// </summary>
    public class DbClassGeneratorSettings : IDbClassGeneratorSettingsProvider
    {
        /// <summary>
        /// Applies the custom settings to the <see cref="DbClassGenerator"/>.
        /// </summary>
        /// <param name="gen">The <see cref="DbClassGenerator"/> to apply the custom settings to.</param>
        public void ApplySettings(DbClassGenerator gen)
        {
            gen.Formatter.AddAlias("shop_id", "ShopID");

            gen.AddCustomType(typeof(ShopID), "shop", "id");

            gen.AddCustomType(typeof(ShopID), "*", "shop_id");
        }
    }
}
