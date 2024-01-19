using UnityEngine;

namespace LineWars.Controllers
{
    public abstract class WinOrLoseAction: ScriptableObject
    {
        public abstract int MoneyAfterBattle { get; }
        
        public abstract int DiamondsAfterBattle { get; }
        
        public abstract void OnWin();
        public abstract void OnLose();
    }
}