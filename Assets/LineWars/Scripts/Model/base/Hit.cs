using System;

namespace LineWars.Model
{
    public struct Hit
    {
        public int Damage { get; }
        public Hit(int damage)
        {
            if (damage < 0)
                throw new ArgumentException(nameof(damage));
            this.Damage = damage;
        }
        
    }
}