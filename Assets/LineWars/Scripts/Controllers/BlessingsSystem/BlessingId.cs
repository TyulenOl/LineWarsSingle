using System;
using UnityEngine;

namespace LineWars.Model
{
    [Serializable]
    public struct BlessingId: IEquatable<BlessingId>
    {
        public static BlessingId Null = new(BlessingType.None, Rarity.Common);
        
        [SerializeField] private BlessingType blessingType;
        [SerializeField] private Rarity rarity;


        public BlessingType BlessingType => blessingType;
        public Rarity Rarity => rarity;

        public BlessingId(BlessingType blessingType, Rarity rarity)
        {
            this.blessingType = blessingType;
            this.rarity = rarity;
        }

        public static bool operator ==(BlessingId left, BlessingId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BlessingId left, BlessingId right)
        {
            return !Equals(left, right);
        }

        public bool Equals(BlessingId other)
        {
            return BlessingType == other.BlessingType && Rarity == other.Rarity;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BlessingId) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) blessingType, (int) rarity);
        }

        public override string ToString()
        {
            return $"{BlessingType} {Rarity}";
        }
    }
}