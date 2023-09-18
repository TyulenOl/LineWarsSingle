using System;

namespace LineWars.Model
{
    public struct Hit
    {
        public ComponentUnit Source { get; }
        public IAlive Destination { get; }
        public int Damage { get; }
        public bool IsPenetrating { get; }
        public bool IsRangeAttack { get; }

        public Hit(int damage, ComponentUnit source, IAlive destination, bool isPenetrating = false, bool isRangeAttack = false)
        {
            Damage = damage >= 0 ? damage : throw new ArgumentException(nameof(damage));
            Source = source ? source : throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            IsPenetrating = isPenetrating;
            IsRangeAttack = isRangeAttack;
        }
    }
}