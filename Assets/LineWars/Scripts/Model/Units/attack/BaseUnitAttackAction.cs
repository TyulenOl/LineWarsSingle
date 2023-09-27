using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public abstract class BaseUnitAttackAction: MonoUnitAction
    {
        [SerializeField] private int damage;
        [SerializeField] private bool isPenetratingDamage;
        public int Damage => damage;
        public bool IsPenetratingDamage => isPenetratingDamage;
    }
}