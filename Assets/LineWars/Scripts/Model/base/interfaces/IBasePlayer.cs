using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IBasePlayer
    {
        public int Id { get; }
        
        public bool CanBuyPreset(UnitBuyPreset preset);
        public void BuyPreset(UnitBuyPreset preset);
    }
}