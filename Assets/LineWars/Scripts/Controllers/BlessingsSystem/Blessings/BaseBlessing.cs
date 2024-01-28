using System;
using UnityEngine;

namespace LineWars.Model
{
    //по-идее, можно было реализовать как и экшоны, но в данном случае это лишнее
    //не использовать состояния! это может плохо сказаться на игре
    public abstract class BaseBlessing: ScriptableObject, IBlessing
    {
        protected Player Player => Player.LocalPlayer;
        public abstract event Action Completed;
        public abstract bool CanExecute();
        public abstract void Execute();
    }
}