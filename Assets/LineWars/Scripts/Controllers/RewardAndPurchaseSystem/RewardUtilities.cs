using System;
using System.Collections.Generic;
using LineWars.Model;

namespace LineWars.Controllers
{
    public static class RewardUtilities
    {
        private static Dictionary<string, PrizeType> keyToPrize = new()
        {
            {"diamonds", PrizeType.Diamonds},
            {"gold", PrizeType.Gold},
            {"upgradeCards", PrizeType.UpgradeCards}
        };
        
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

        public static Prize DecodePurchaseId(string id)
        {
            var keys = id.Split("_");
            return new Prize(keyToPrize[keys[0]], int.Parse(keys[1]));
        }
        
        public static bool CanDecodePurchaseId(string id)
        {
            var keys = id.Split("_");
            return keys.Length == 2 
                   && keyToPrize.ContainsKey(keys[0]) 
                   && int.TryParse(keys[1], out _);
        }
    }
}