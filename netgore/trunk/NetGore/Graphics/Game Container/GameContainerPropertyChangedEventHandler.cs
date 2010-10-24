using System.Linq;

namespace NetGore.Graphics
{
    public delegate void GameContainerPropertyChangedEventHandler<T>(IGameContainer sender, T oldValue, T newValue);
}