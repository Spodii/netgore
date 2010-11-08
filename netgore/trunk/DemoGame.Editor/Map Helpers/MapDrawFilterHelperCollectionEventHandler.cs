using System.Collections.Generic;
using System.Linq;

namespace DemoGame.Editor
{
    /// <summary>
    /// Delegate for handling events from the <see cref="MapDrawFilterHelperCollection"/>.
    /// </summary>
    /// <param name="sender">The <see cref="MapDrawFilterHelperCollection"/> the event came from.</param>
    /// <param name="filter">The name of the filter and the filter that the event is related to.</param>
    public delegate void MapDrawFilterHelperCollectionEventHandler(
        MapDrawFilterHelperCollection sender, KeyValuePair<string, MapDrawFilterHelper> filter);
}