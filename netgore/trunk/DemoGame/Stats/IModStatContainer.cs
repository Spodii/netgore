using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame
{
    public interface IModStatContainer
    {
        int GetStatModBonus(StatType statType);
    }
}