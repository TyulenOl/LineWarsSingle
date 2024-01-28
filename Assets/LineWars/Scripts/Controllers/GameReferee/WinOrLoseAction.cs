using UnityEngine;

namespace LineWars.Controllers
{
    public abstract class WinOrLoseAction: ScriptableObject
    {
        //TODO переделать на статистику, чем больше ящеров убил, тем больше денег
        
        [SerializeField] protected int minGold = 50;
        [SerializeField] protected int maxGold = 125;
        [SerializeField] protected int minDiamonds = 0;
        [SerializeField] protected int maxDiamonds = 10;

        protected virtual int GetMoneyAfterBattle() => Random.Range(minGold, maxGold);
        public virtual int GetDiamondsAfterBattle() => Random.Range(minDiamonds, maxDiamonds);
        
        
        public abstract void OnWin();
        public abstract void OnLose();
    }
}