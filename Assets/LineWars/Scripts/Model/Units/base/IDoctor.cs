using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IDoctor
    {
        public bool IsMassHeal { get; }
        public int HealingAmount { get; }
        public bool HealLocked { get; set; }
        public bool CanHeal([NotNull] Unit target);
        public void Heal([NotNull] Unit target);
    }
}