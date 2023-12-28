using System;

namespace LineWars.Model
{
    [Obsolete]
    public interface IActionWithDamage
    {
        int Damage { get; }
    }
}