using System.Collections.Generic;

namespace DemoGame.Editor
{
    /// <summary>
    /// Delegate for handling events from the <see cref="MapDrawFilterHelperCollection"/>.
    /// </summary>
    /// <param name="sender">The <see cref="MapDrawFilterHelperCollection"/> the event came from.</param>
    /// <param name="filter">The name of the filter and the filter that the event is related to.</param>
    /// <param name="oldName">The old name of the filter.</param>
    public delegate void MapDrawFilterHelperCollectionRenamedEventHandler(
        MapDrawFilterHelperCollection sender, KeyValuePair<string, MapDrawFilterHelper> filter, string oldName);
}