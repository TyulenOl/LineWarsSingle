using UnityEngine;

namespace LineWars.Controllers
{
    public abstract class WinOrLoseAction: ScriptableObject
    {
        public abstract void OnWin();
        public abstract void OnLose();
    }
}