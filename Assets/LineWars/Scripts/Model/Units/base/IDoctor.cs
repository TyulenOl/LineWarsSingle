using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IDoctor
    {
        public bool CanHeal([NotNull] Unit target);
        public void Heal([NotNull] Unit target);
    }
}