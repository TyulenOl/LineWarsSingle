using System;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace LineWars.Model
{
    public interface IAlive
    {
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
    }
    
    public interface ITargetedAlive: ITarget, IAlive
    {
    }
}