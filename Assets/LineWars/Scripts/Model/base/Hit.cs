using System;

namespace LineWars.Model
{
    public struct Hit
    {
        public IAttackerVisitor Source { get; }
        public IAlive Destination { get; }
        public int Damage { get; }
        public bool IsPenetrating { get; }
        public bool IsRangeAttack { get; }

        public Hit(int damage, IAttackerVisitor source, IAlive destination, bool isPenetrating = false, bool isRangeAttack = false)
        {
            Damage = damage >= 0 ? damage : throw new ArgumentException(nameof(damage));
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            IsPenetrating = isPenetrating;
            IsRangeAttack = isRangeAttack;
        }
    }
}