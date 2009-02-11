using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Platyform.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// Interface for an Entity that the server has to notify the client about, such as Characters and
    /// Items on the map. Other Entities, like basic Walls, do not need this because the client already
    /// knows about them from the map file.
    /// </summary>
    public interface IDynamicEntity
    {
        /// <summary>
        /// Gets the byte array that needs to be sent to the Client to create this Entity on the Client.
        /// </summary>
        /// <returns>PacketWriter containing the data used by the Client to create this Entity. Can be null if the Entity
        /// can not be created in it's current state.</returns>
        PacketWriter GetCreationData();

        /// <summary>
        /// Gets the byte array that needs to be send to the Client to destroy this Entity on the Client.
        /// </summary>
        /// <returns>PacketWriter containing the data used by the Client to destroy this Entity. Can be null if the Entity
        /// can not be destroyed in it's current state.</returns>
        PacketWriter GetRemovalData();
    }
}