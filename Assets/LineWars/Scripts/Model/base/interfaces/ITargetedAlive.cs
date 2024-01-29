using System;
using UnityEngine.Events;

namespace LineWars.Model
{
    public interface IAlive
    {
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }

        public bool IsDied => CurrentHp == 0;
    }
    
    public interface ITargetedAlive: ITarget, IAlive
    {
    }
}