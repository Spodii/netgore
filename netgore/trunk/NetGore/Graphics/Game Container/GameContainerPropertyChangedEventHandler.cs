using System.Linq;

namespace NetGore.Graphics
{
    public delegate void GameContainerPropertyChangedEventHandler<in T>(IGameContainer sender, T oldValue, T newValue);
}