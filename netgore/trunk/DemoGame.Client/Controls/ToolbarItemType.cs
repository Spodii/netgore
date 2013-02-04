using System.Linq;

namespace DemoGame.Client
{
    /// <summary>
    /// All of the different ToolbarItems available. The sprite associated with the ToolbarItem is
    /// based on the string value of the ToolbarItem.
    /// </summary>
    enum ToolbarItemType
    {
        Inventory,
        Equipped,
        Stats,
        Skills,
        Guild,
        Friends,
        Users
    }
}