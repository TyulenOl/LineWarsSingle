using System;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace LineWars.Model
{
    public interface IAlive
    {
        public int CurrentHp { get; }
        public bool IsDied { get; }
        
        /// <summary>
        /// Вызывается перед смертью
        /// <param name="unit">Умирающий юнит</param>
        /// </summary>
        public UnityEvent<Unit> Died { get; }

        public void TakeDamage(Hit hit);
        public void HealMe(int healAmount);
    }
}