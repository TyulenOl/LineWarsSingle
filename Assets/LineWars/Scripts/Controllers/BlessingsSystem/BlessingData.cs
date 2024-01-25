using System;

namespace LineWars.Model
{
    public class BlessingData: IEquatable<BlessingData>
    {
        public BlessingType BlessingType { get; }
        public Rarity Rarity { get; }

        public BlessingData(BlessingType blessingType, Rarity rarity)
        {
            BlessingType = blessingType;
            Rarity = rarity;
        }

        public bool Equals(BlessingData other)
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
            return Equals((BlessingData) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) BlessingType, (int) Rarity);
        }
    }
}