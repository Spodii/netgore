using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame;
using DemoGame.DbObjs;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.RPGComponents;

// TODO: $$ Move the rest of this stuff into the SideScrollerMapBase

namespace DemoGame
{
    public abstract class MapBase : SideScrollerMapBase, IMapTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapBase"/> class.
        /// </summary>
        /// <param name="mapIndex">Index of the map.</param>
        /// <param name="getTime">Interface used to get the time.</param>
        protected MapBase(MapIndex mapIndex, IGetTime getTime) : base(mapIndex, getTime)
        {
        }

        /// <summary>
        /// Gets all the items that intersect a specified area
        /// </summary>
        /// <param name="rect">Rectangle of the area to check</param>
        /// <returns>A list containing all ItemEntityBases that intersect the specified area</returns>
        public List<ItemEntityBase> GetItems(Rectangle rect)
        {
            return GetEntities<ItemEntityBase>(rect);
        }

        /// <summary>
        /// Gets all the ItemEntityBases that intersect a specified area
        /// </summary>
        /// <param name="min">Min point of the collision area</param>
        /// <param name="max">Max point of the collision area</param>
        /// <returns>A list containing all ItemEntityBases that intersect the specified area</returns>
        public List<ItemEntityBase> GetItems(Vector2 min, Vector2 max)
        {
            var size = max - min;
            return GetItems(new Rectangle((int)min.X, (int)min.Y, (int)size.X, (int)size.Y));
        }

        /// <summary>
        /// Gets the first IUsableEntity that intersects a specified area
        /// </summary>
        /// <param name="rect">Rectangle of the area to check</param>
        /// <param name="charEntity">CharacterEntity that must be able to use the IUsableEntity</param>
        /// <returns>First IUsableEntity that intersects the specified area that the charEntity
        /// is able to use, or null if none</returns>
        public IUsableEntity GetUsable(Rectangle rect, CharacterEntity charEntity)
        {
            // Predicate that will check if an Entity inherits interface IUsableEntity,
            // and if it can be used by the specified CharacterEntity
            Predicate<Entity> pred = delegate(Entity entity)
                                     {
                                         var usable = entity as IUsableEntity;
                                         if (usable == null)
                                             return false;

                                         return usable.CanUse(charEntity);
                                     };

            return GetEntity(rect, pred) as IUsableEntity;
        }

        /// <summary>
        /// Gets the first IUsableEntity that intersects a specified area
        /// </summary>
        /// <param name="cb">CollisionBox of the area to check</param>
        /// <param name="charEntity">CharacterEntity that must be able to use the IUsableEntity</param>
        /// <returns>First IUsableEntity that intersects the specified area that the charEntity
        /// is able to use, or null if none</returns>
        public IUsableEntity GetUsable(CollisionBox cb, CharacterEntity charEntity)
        {
            return GetUsable(cb.ToRectangle(), charEntity);
        }

        #region IMapTable Members

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IMapTable IMapTable.DeepCopy()
        {
            return new MapTable(this);
        }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        MapIndex IMapTable.ID
        {
            get { return Index; }
        }

        #endregion
    }
}