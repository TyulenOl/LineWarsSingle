using System;
using UnityEngine;

namespace LineWars.Model
{
    [Serializable]
    public class BlessingId: IEquatable<BlessingId>
    {
        [SerializeField] private BlessingType blessingType;
        [SerializeField] private Rarity rarity;


        public BlessingType BlessingType => blessingType;
        public Rarity Rarity => rarity;

        public BlessingId(BlessingType blessingType, Rarity rarity)
        {
            this.blessingType = blessingType;
            this.rarity = rarity;
        }

        public bool Equals(BlessingId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return BlessingType == other.BlessingType && Rarity == other.Rarity;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BlessingId) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) BlessingType, (int) Rarity);
        }
    }
}