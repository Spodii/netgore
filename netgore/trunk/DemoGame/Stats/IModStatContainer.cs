using System.Linq;

namespace DemoGame
{
    public interface IModStatContainer
    {
        int GetStatModBonus(StatType statType);
    }
}