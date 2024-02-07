using System;

namespace LineWars.Controllers
{
    public static class RewardUtilities
    {
        public static (PrizeType prizeType, int amount) DecodeId(int id, int priseTypeLenInBits)
        {
            if (priseTypeLenInBits < 1)
                throw new InvalidOperationException();
            var prizeId = id % (int)Math.Pow(2, priseTypeLenInBits);
            var amount = (id - prizeId) / (int)Math.Pow(2, priseTypeLenInBits);

            return ((PrizeType) prizeId, amount);
        }

        public static int EncodeId(PrizeType prizeType, int amount, int priseTypeLenInBits)
        {
            if (priseTypeLenInBits < 1)
                throw new InvalidOperationException();
            if (amount < 0 || amount > Math.Pow(2, 32 - priseTypeLenInBits))
                throw new InvalidOperationException();
            var prizeId = (int) prizeType;
            if (prizeId < 0 || prizeId > Math.Pow(2, priseTypeLenInBits))
                throw new InvalidOperationException();

            var result = prizeId;
            result += amount * (int) Math.Pow(2, priseTypeLenInBits);
            return result;
        }
    }
}