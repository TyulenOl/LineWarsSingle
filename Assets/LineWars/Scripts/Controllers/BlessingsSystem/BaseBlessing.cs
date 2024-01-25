using System;
using UnityEngine;

namespace LineWars.Model
{
    //по-идее, можно было реализовать как и экшоны, но в данном случае это лишнее
    public abstract class BaseBlessing: ScriptableObject
    {
        public abstract event Action Completed;
        public abstract bool CanExecute();
        public abstract void Execute();
    }
}